import { Module } from 'vuex';
import { RootState } from '../types';
import { PatientsClient, IPatientVm, IPatientProfileVm } from '../../IUGOCare-api';
import axiosClient from '@/auth/axiosInstance';

export interface PatientsState {
  currentPatient: IPatientVm;
  patientPromise: Promise<IPatientVm>;
  currentPatientProfile: IPatientProfileVm;
  patientProfilePromise: Promise<IPatientProfileVm>;
}

const state: PatientsState = {
  currentPatient: null,
  patientPromise: null,
  currentPatientProfile: null,
  patientProfilePromise: null,
};

const client = new PatientsClient(process.env.VUE_APP_API_URL, axiosClient);

const namespaced = true;

export const patients: Module<PatientsState, RootState> = {
  namespaced,
  state,
  actions: {
    async fetchCurrentPatient(context) {
      if (state.currentPatient) {
        return state.currentPatient;
      }

      if (state.patientPromise) {
        return state.patientPromise;
      }

      const promise = client.getPatient()
        .then((vm) => {
          if (vm.givenName === null && vm.familyName === null) {
            vm.givenName = window.localStorage.username;
          }
          context.commit('setCurrentPatient', vm);
          return vm;
        });

      context.commit('setPatientPromise', promise);
    },
    async fetchCurrentPatientProfile(context) {
      if (state.patientProfilePromise) {
        return state.patientProfilePromise;
      }

      const promise = client.getPatientProfile()
        .then((vm) => {
          context.commit('setCurrentPatientProfile', vm);
          return vm;
        });

      context.commit('setPatientProfilePromise', promise);
    },
  },
  mutations: {
    setCurrentPatient(patientState, patient: IPatientVm) {
      patientState.currentPatient = patient;
      patientState.patientPromise = null;
    },
    setPatientPromise(patientState, promise: Promise<IPatientVm>) {
      patientState.patientPromise = promise;
    },
    setCurrentPatientProfile(patientState, profile: IPatientProfileVm) {
      patientState.currentPatientProfile = profile;
      patientState.patientProfilePromise = null;
    },
    setPatientProfilePromise(patientState, promise: Promise<IPatientProfileVm>) {
      patientState.patientProfilePromise = promise;
    },
  },
};
