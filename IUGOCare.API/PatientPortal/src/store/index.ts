import Vue from 'vue';
import Vuex, { StoreOptions } from 'vuex';
import { RootState } from './types';
import { observations } from './observations/index';
import { patients } from './patients/index';
import { settings } from './settings/index';
import { translations } from './translations/index';
import { organizations } from './organizations/index';
import { providers } from './providers/index';
import { careManagementPrograms } from './careManagementPrograms/index';
import { targetRanges } from './target-ranges/index';
import { demo } from './demo/index';

Vue.use(Vuex);

const store: StoreOptions<RootState> = {
  state: {
    version: '5.0.1',
  },
  modules: {
    observations,
    patients,
    settings,
    translations,
    organizations,
    providers,
    careManagementPrograms,
    targetRanges,
    demo,
  },
};

export default new Vuex.Store<RootState>(store);
