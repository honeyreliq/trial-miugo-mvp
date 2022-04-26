import { Module } from 'vuex';
import { RootState } from '../types';
import { CommandTestsClient, IOrganizationDto } from '../../IUGOCare-api';
import axiosClient from '@/auth/axiosInstance';

interface OrganizationsState {
  loadingOrganizations: boolean;
  organizations: IOrganizationDto[];
}

const state: OrganizationsState = {
  loadingOrganizations: false,
  organizations: [],
};

const client = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);

const namespaced = true;

export const organizations: Module<OrganizationsState, RootState> = {
  namespaced,
  state,
  actions: {
    async fetchOrganizations(context) {
      context.commit('setLoading', true);
      client.getOrganizations()
        .then((vm) => {
          context.commit('setOrganizations', vm.organizations);
          context.commit('setLoading', false);
        });
    },
  },
  mutations: {
    setOrganizations(organizationState, items: IOrganizationDto[]) {
      organizationState.organizations = items;
    },
    setLoading(organizationState, payload: boolean) {
      organizationState.loadingOrganizations = payload;
    },
  },
};
