import { Module } from 'vuex';
import { pathOr } from 'ramda';
import { RootState } from '../types';

export interface SettingsState {
  drawer: boolean;
}
const state: SettingsState = {
  drawer: false,
};

export const settings: Module<SettingsState, RootState> = {
  namespaced: true,
  state,
  getters: {
    currentPatientId: (_, __, rootState): string  => {
      const clinicPatientIds = pathOr([], ['patients', 'currentPatient', 'clinicPatientIds'], rootState) as string[];
      return clinicPatientIds.length > 0 ? clinicPatientIds[0] : '';
    },
  },
  mutations: {
    SET_DRAWER(localState, payload: boolean) {
      localState.drawer = payload;
    },
  },
};

