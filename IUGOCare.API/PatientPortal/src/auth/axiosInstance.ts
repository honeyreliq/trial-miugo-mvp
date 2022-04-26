import { getInstance } from './index';
import axios, { AxiosResponse, AxiosRequestConfig } from 'axios';

const interceptedAxios = axios.create();

interceptedAxios.interceptors.request.use((config: AxiosRequestConfig) => {
    config.headers.Authorization = `Bearer ${window.localStorage['auth0:token']}`;
    config.validateStatus = (status) => status >= 200 && status < 300;
    return config;
});

const okay = (x: any) => x;

const retryOn401 = async (err: AxiosResponse<any>) => {
    if (err.config && err.request.status === 401 && !err.request.__isRetryRequest) {
        const $auth = getInstance();
        const token = await $auth.getTokenSilently();
        window.localStorage['auth0:token'] = token;
        err.config.headers.Authorization = `Bearer ${token}`;
        (err.config as any).__isRetryRequest = true;
        return axios(err.config);
    } else {
        return new Promise((_, reject) => reject(err));
    }
};

interceptedAxios.interceptors.response.use(okay, retryOn401);

export default interceptedAxios;
