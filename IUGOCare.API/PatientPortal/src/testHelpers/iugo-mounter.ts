import i18n from '../i18n';
import vuetify from '../plugins/vuetify';
import { shallowMount, VueClass } from '@vue/test-utils';
import Vue from 'vue';
import Vuex from 'vuex';
import Vuetify from 'vuetify';

/* eslint-disable @typescript-eslint/no-var-requires */
const BaseButton = require('../components/base/BaseButton.vue');
const BaseCard = require('../components/base/BaseCard.vue');
const BaseToolbar = require('../components/base/BaseToolbar.vue');
/* eslint-enable @typescript-eslint/no-var-requires */

Vue.use(Vuex);
Vue.use(Vuetify);

Vue.component('BaseButton', BaseButton);
Vue.component('BaseCard', BaseCard);
Vue.component('BaseToolbar', BaseToolbar);

export const mount = (component: VueClass<Vue>, propsData: { [name: string]: any } = {}, storeOptions = {}) => {
  const store = new Vuex.Store(storeOptions);

  return shallowMount(component, {
    propsData, i18n, vuetify, store,
  });
};
