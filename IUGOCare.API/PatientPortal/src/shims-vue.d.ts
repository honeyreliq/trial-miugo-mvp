import { AxiosInstance } from 'axios';
import { Store } from 'vuex';
import VueRouter from 'vue-router';
import Vue from 'vue';

declare module '*.vue' {
  export default Vue;
}

declare module 'vue/types/vue' {

  interface VueConstructor {
    $http: AxiosInstance;
    $store: Store<any>;
    $router: VueRouter;
  }

  interface Vue {
    $http: AxiosInstance;
    $store: Store<any>;
    $router: VueRouter;
  }
}
