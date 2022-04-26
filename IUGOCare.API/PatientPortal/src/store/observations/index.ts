import { Module } from 'vuex';
import { RootState } from '../types';
import { ObservationsClient, PatientObservationDto } from '../../IUGOCare-api';
import axiosClient from '@/auth/axiosInstance';

interface ObservationsState {
  error: boolean;
  errorMessage: string;
  loading: boolean;
  patientObservations: PatientObservationDto[];
  recentPatientObservationTypes: string[];
}

const state: ObservationsState = {
  error: false,
  errorMessage: '',
  loading: false,
  patientObservations: [],
  recentPatientObservationTypes: [],
};

const client = new ObservationsClient(process.env.VUE_APP_API_URL, axiosClient);

const namespaced = true;

export const observations: Module<ObservationsState, RootState> = {
  namespaced,
  state,
  actions: {
    async fetchPatientObservations(context, { observationCodes, effectiveDateStart, effectiveDateEnd }) {
      const clinicPatientId = context.rootGetters['settings/currentPatientId'];

      context.commit('setLoading', true);
      try {
        context.commit('setPatientObservations', []);
        const response = await client.getPatientObservations(
          clinicPatientId, observationCodes, effectiveDateStart, effectiveDateEnd,
        );
        context.commit('setPatientObservations', response.observations || []);
      } catch (e) {
        context.commit('setError', `Error while loading observations: ${e.message}`);
      }
      context.commit('setLoading', false);
    },
    async fetchRecentPatientObservationTypes(context) {
      context.commit('setLoading', true);
      try {
        context.commit('setRecentPatientObservationTypes', []);
        const response = await client.getRecentPatientObservationTypes();
        context.commit('setRecentPatientObservationTypes', response.observationTypes || []);
      } catch (e) {
        context.commit('setError', `Error while loading observation types: ${e.message}`);
      }
      context.commit('setLoading', false);
    },
  },
  mutations: {
    setError(localState, error: string) {
      localState.error = true;
      localState.errorMessage = error;
    },
    setPatientObservations(localState, observationDtos: PatientObservationDto[]) {
      localState.patientObservations = observationDtos;
    },
    setRecentPatientObservationTypes(localState, recentObservationTypes: string[]) {
      localState.recentPatientObservationTypes = recentObservationTypes;
    },
    setLoading(localState, payload: boolean) {
      localState.loading = payload;
      localState.error = false;
      localState.errorMessage = '';
    },
  },
};
