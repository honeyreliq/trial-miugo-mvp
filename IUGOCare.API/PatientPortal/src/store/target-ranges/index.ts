import { Module } from 'vuex';
import { RootState } from '../types';
import { TargetRangesClient, TargetRangeDto } from '../../IUGOCare-api';
import axiosClient from '@/auth/axiosInstance';

interface TargetRangesState {
    error: boolean;
    errorMessage: string;
    loading: boolean;
    targetRanges: TargetRangeDto[];
}

const state: TargetRangesState = {
    error: false,
    errorMessage: '',
    loading: false,
    targetRanges: [],
};

const client = new TargetRangesClient(process.env.VUE_APP_API_URL, axiosClient);

const namespaced = true;

export const targetRanges: Module<TargetRangesState, RootState> = {
    namespaced,
    state,
    actions: {
      async fetchPatientTargetRanges(context) {
        context.commit('setLoading', true);
        try {
          context.commit('setPatientTargetRanges', []);
          const response = await client.getPatientTargetRanges();
          context.commit('setPatientTargetRanges', response.targetRanges || []);
        } catch (e) {
            context.commit('setError', `Error while loading patient target ranges: ${e.message}`);
        }
        context.commit('setLoading', false);
      },
    },
    mutations: {
      setError(localState, error: string) {
        localState.error = true;
        localState.errorMessage = error;
      },
      setPatientTargetRanges(localState, patientTargetRanges: TargetRangeDto[]) {
          localState.targetRanges = patientTargetRanges;
      },
      setLoading(localState, payload: boolean) {
        localState.loading = payload;
        localState.error = false;
        localState.errorMessage = '';
      },
    },
};
