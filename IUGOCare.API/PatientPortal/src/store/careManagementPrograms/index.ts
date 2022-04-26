import { Module } from 'vuex';
import { RootState } from '../types';
import { CommandTestsClient, ICareManagementProgramDto } from '../../IUGOCare-api';
import axiosClient from '@/auth/axiosInstance';

interface CareManagementProgramsState {
  loadingCareManagementPrograms: boolean;
  careManagementPrograms: ICareManagementProgramDto[];
}

const state: CareManagementProgramsState = {
  loadingCareManagementPrograms: false,
  careManagementPrograms: [],
};

const client = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);

const namespaced = true;

export const careManagementPrograms: Module<CareManagementProgramsState, RootState> = {
  namespaced,
  state,
  actions: {
    async fetchCareManagementPrograms(context) {
      context.commit('setLoading', true);
      client.getCareManagementPrograms()
        .then((vm) => {
          context.commit('setCareManagementPrograms', vm.careManagementPrograms);
          context.commit('setLoading', false);
        });
    },
  },
  mutations: {
    setCareManagementPrograms(careManagementProgramState, items: ICareManagementProgramDto[]) {
      careManagementProgramState.careManagementPrograms = items;
    },
    setLoading(careManagementProgramState, payload: boolean) {
      careManagementProgramState.loadingCareManagementPrograms = payload;
    },
  },
};
