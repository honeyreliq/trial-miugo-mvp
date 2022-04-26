import Vue, {VueConstructor} from 'vue';
// @ts-ignore
import UpdateEmailAddress from '../UpdateEmailAddress.vue';
import {shallowMount, Wrapper} from '@vue/test-utils';
import i18n from '@/i18n';
import vuetify from '@/plugins/vuetify';
import Vuex from 'vuex';
import Vuetify from 'vuetify';
import VueRouter from 'vue-router';
import {IServerResponse, IServer, getServer} from '@/testHelpers/axiosMock';

const server: IServer = getServer();
/* eslint-disable @typescript-eslint/no-var-requires */
// jest mocks are hoisted, that's why we use require in here
jest.mock('axios', () => {
  const axiosMock = require('../../../testHelpers/axiosMock');
  return axiosMock.server().init();
});

const BaseButton = require('../../../components/base/BaseButton.vue');
const BaseCard = require('../../../components/base/BaseCard.vue');
/* eslint-enable @typescript-eslint/no-var-requires */

let wrapper: Wrapper<Vue>;

function mountComponent(VueComponent: VueConstructor<any>): Wrapper<Vue> {
  Vue.use(Vuex);
  Vue.use(Vuetify);
  Vue.use(VueRouter);

  const router = new VueRouter({
    routes: [
      {
        path: '/',
        children: [
          {
            name: 'UpdateEmailAddress',
            path: '/UpdateEmailAddress/:token?',
          },
        ],
      },
    ],
  });
  router.push({name: 'UpdateEmailAddress', params: {token: 'my-token'}});

  Vue.component('BaseButton', BaseButton);
  Vue.component('BaseCard', BaseCard);

  return shallowMount((VueComponent as any), {
    i18n, vuetify, router,
  });
}

function createComponent(success = true) {
  server.createServerResponse({
    method: 'GET',
    url: '/api/Patients/updateEmail/my-token',
    status: (success ? 200 : 500),
  } as IServerResponse);
  server.createServerResponse({method: 'PUT', url: '/api/Patients/updateEmail/my-token'} as IServerResponse);

  wrapper = mountComponent(UpdateEmailAddress);
}

describe('UpdateEmailAddress.vue', () => {
  afterEach(() => {
    server.reset();
  });

  test('should exists', () => {
    createComponent();

    expect(wrapper.vm).toBeTruthy();
  });

  test('should render processing at the beginning', (done) => {
    createComponent();

    setTimeout(() => {
      // @ts-ignore
      wrapper.vm.component.processing = true;
      // @ts-ignore
      wrapper.vm.component.success = false;

      setTimeout(() => {
        expect(wrapper.text().includes('Please wait...')).toBeTruthy();
        done();
      });

    }, 100);
  });

  test('should render success message', (done) => {
    createComponent();

    setTimeout(() => {
      expect(wrapper.text().includes('Email updated successfully!')).toBeTruthy();
      done();
    }, 100);
  });

  test('should render fail message', (done) => {
    createComponent(false);

    setTimeout(() => {
      expect(wrapper.text().includes('Verification could not be completed! Please try again.')).toBeTruthy();
      done();
    }, 100);
  });

});
