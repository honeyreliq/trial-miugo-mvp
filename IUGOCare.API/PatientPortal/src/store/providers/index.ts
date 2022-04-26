import { Module } from 'vuex';
import { RootState } from '../types';
import { CommandTestsClient, IProviderDto } from '../../IUGOCare-api';
import axiosClient from '@/auth/axiosInstance';

interface ProvidersState {
  loadingProviders: boolean;
  providers: IProviderDto[];
}

const state: ProvidersState = {
  loadingProviders: false,
  providers: [],
};

const client = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);

const namespaced = true;

export const providers: Module<ProvidersState, RootState> = {
  namespaced,
  state,
  actions: {
    async fetchProviders(context) {
      context.commit('setLoading', true);
      client.getProviders()
        .then((vm) => {
          context.commit('setProviders', vm.providers);
          context.commit('setLoading', false);
        });
    },
  },
  mutations: {
    setProviders(providersState, items: IProviderDto[]) {
      providersState.providers = items;
    },
    setLoading(providersState, payload: boolean) {
      providersState.loadingProviders = payload;
    },
  },
};
