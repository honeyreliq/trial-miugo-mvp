import Vue from 'vue';
// @ts-ignore
import App from './App.vue';
import router from './router';
import store from './store/index';
import './plugins/base';
import './plugins/chartist';
import './plugins/vee-validate';
import './plugins/vue-tour';
import vuetify from './plugins/vuetify';
import i18n from './i18n';
import { library } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { fas } from '@fortawesome/pro-solid-svg-icons';
import { far } from '@fortawesome/pro-regular-svg-icons';
import { fal } from '@fortawesome/pro-light-svg-icons';
import { fad } from '@fortawesome/pro-duotone-svg-icons';
import VueApexCharts from 'vue-apexcharts';
import titleMixin from './mixins/titleMixin';
// Not sure why these show as not found. Everythign works as expected.
// @ts-ignore
import ResizeSensor from 'resize-sensor';
// @ts-ignore
import VueStickyDirective from '@renatodeleao/vue-sticky-directive';



/* eslint-disable */
/**
 * Q: Why are we using a require, instead of an import, which forces us to
 *    disable eslint?
 * A: There is a TypeScript error in v2.0.3 of v-tooltip (see here: https://github.com/Akryum/v-tooltip/issues/459).
 *    There are currently two open PRs to fix the error (#462 & #564), but the
 *    maintainer seems to no longer be interested in maintaining this project.
 *    The other alternative is to disable all type checking on imported libs by
 *    setting `"skipLibCheck": true` in our tsconfig, but I'd rather not do that
 *    unless it becomes necessary (eg, by another library having an issue).
 */
const VTooltip = require('v-tooltip');
/* eslint-enable */

// Import the plugin here
import { Auth0Plugin } from './auth';

// Install the authentication plugin here
Vue.use(Auth0Plugin, {
  onRedirectCallback: (appState: any) => {
    router.push(
      appState && appState.targetUrl
        ? appState.targetUrl
        : window.location.pathname,
    );
  },
});

Vue.use(VTooltip);

Vue.component('fa-icon', FontAwesomeIcon); // Register component globally
library.add(fas); // Include needed icons.
library.add(far); // Include needed icons.
library.add(fal); // Include needed icons.
library.add(fad); // Include needed icons.

Vue.use(vuetify, {
  iconfont: 'faSvg',
});

// @ts-ignore
window.ResizeSensor = ResizeSensor;
Vue.use(VueStickyDirective);

Vue.use(VueApexCharts);
Vue.component('apexchart', VueApexCharts);

Vue.mixin(titleMixin);

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  vuetify,
  i18n,
  render: (h) => h(App),
}).$mount('#app');
