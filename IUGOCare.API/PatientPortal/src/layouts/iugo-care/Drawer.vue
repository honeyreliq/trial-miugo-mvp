<template>
  <v-navigation-drawer
      id="iugo-care-drawer"
      v-model="isVisible"
      :mini-variant="isMini"
      :temporary="isMobile"
      dark
      app
      v-bind="$attrs"
      :mini-variant-width="miniWidth"
      width="300"
      height="100%"
      :mobile-breakpoint="0"
  >
    <div class="py-0 drawer-wrapper">
      <router-link to="" class="d-flex mt-4 pa-0 overflow-hidden">
        <span class="logo">
          <BaseLinkedImage
              :src="require(`@/assets/${iugoImg}`)"
              contain
              height="100%"
              position="left center"
          />
        </span>
      </router-link>
    </div>

    <v-list dense nav class="py-0 drawer-wrapper">
      <v-divider class="my-4"/>
      <v-list-item class="primary" v-tooltip="enableTooltip({tooltip:'addToPatient'})">
        <v-list-item-icon>
          <fa-icon :icon="['fal', 'plus']" class="fa-fw"/>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title class="text-subtitle-1">
            {{$t("addToPatient")}}
          </v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-divider class="my-4"/>
      <template v-for="(item, i) in computedItems">
        <BaseItemGroup v-if="item.children" :key="`group-${i}`" :item="item" :use-tooltip="isMini" :class="item.class"/>
        <BaseItem v-else :key="`item-${i}`" :item="item" v-tooltip="enableTooltip(item)" :class="item.class"/>
        <v-divider v-if="item.divider" :key="i" dark class="my-4" :class="item.class"></v-divider>
      </template>
    </v-list>

    <template v-slot:append>
      <v-menu
          top
          offset-y
          origin="bottom left"
          transition="scroll-x-transition"
          min-width="276"
          :value="openMenu"
      >
        <template v-slot:activator="{ attrs, on }">
          <v-list dense nav class="pa-0 naturalGrey fill-height user-menu">
            <v-list-item class="mb-0 pa-4 transparent" @click="openMenu = true" v-bind="attrs" v-on="on">
              <v-list-item-icon class="mr-1">
                <v-list-item-avatar color="primary" size="42">
                  <span class="white--text text-h6">
                    <!-- {{ patientInitials }} -->
                    DS
                  </span>
                </v-list-item-avatar>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title class="text-subtitle-1">
                  <span v-if="patientName">
                    <!-- {{ patientName }} -->
                    David Smith
                  </span>
                  <span v-else class="text-lowercase">
                    <!-- {{ patientEmail }} -->
                    dsmith@doctor.com
                  </span>
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list>
        </template>

        <v-list class="py-0" flat>
          <v-list-item-group>
            <v-list-item inactive>
              <v-list-item-icon class="mr-4">
                <fa-icon :icon="['fad', 'phone']" class="fa-fw"/>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>+1 (555) 527-8736</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            <v-list-item inactive>
              <v-list-item-icon class="mr-4">
                <fa-icon :icon="['fad', 'envelope']" class="fa-fw"/>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>
                  <!-- {{ patientEmail }} -->
                  dsmith@doctor.com
                  </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            <v-divider/>
            <v-list-item @click="userModal = true; selectedOptionValue = 'account'">
              <v-list-item-icon class="mr-4">
                <fa-icon :icon="['fad', 'user-circle']" class="fa-fw"/>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>{{$t("account")}}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            <v-list-item @click="openPreferences()">
              <v-list-item-icon class="mr-4">
                <fa-icon :icon="['fad', 'cog']" class="fa-fw"/>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>{{ $t('preferences') }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            <v-divider/>
            <v-list-item @click="helpModal = true">
              <v-list-item-icon class="mr-4">
                <fa-icon :icon="['fad', 'question-circle']" class="fa-fw"/>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>{{ $t('help') }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            <v-list-item target="_blank" href="https://reliqhealth.freshdesk.com">
              <v-list-item-icon class="mr-4">
                <fa-icon :icon="['fad', 'life-ring']" class="fa-fw"/>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>{{ $t('support') }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            <v-divider/>
            <AboutDialog></AboutDialog>
            <PrivacyDialog></PrivacyDialog>
            <v-divider/>
            <v-list-item @click="logout">
              <v-list-item-icon class="mr-4">
                <fa-icon :icon="['fad', 'sign-out-alt']" class="fa-fw"/>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>{{ $t('logout') }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list-item-group>
        </v-list>
      </v-menu>
    </template>
    <UserSettings v-model="userModal" :selectedOption="selectedOptionValue"></UserSettings>
    <UserPreferences v-model="prefModal"></UserPreferences>
    <UserHelp v-model="helpModal"></UserHelp>
  </v-navigation-drawer>
</template>

<script>
// Utilities
import { mapState } from 'vuex';
// Components
import AboutDialog from './components/AboutDialog.vue';
import PrivacyDialog from './components/PrivacyDialog.vue';
import UserPreferences from './components/UserPreferences.vue';
import UserHelp from './components/UserHelp.vue';
import UserSettings from './components/UserSettings.vue';

export default {
  name: 'IugoCareDrawer',
  components: {
    UserPreferences,
    UserHelp,
    AboutDialog,
    PrivacyDialog,
    UserSettings,
  },
  props: {},
  data: () => ({
    iugoImg: 'iUGO_Care_W.svg',
    miniVariant: false,
    prefModal: false,
    helpModal: false,
    openMenu: false,
    userModal: false,
    selectedOptionValue: '',
    items: [
      {
        icon: 'analytics',
        title: 'dashboard',
        tooltip: 'dashboard',
      },
      {
        icon: 'address-card',
        title: 'patients',
        tooltip: 'patients',
        divider: true,
      },
      {
        group: '/programs',
        icon: 'laptop-medical',
        title: 'programs',
        tooltip: 'programs',
        divider: true,
        children: [
          {
            title: 'rpm',
            tooltip: 'rpm',
          },
          {
            title: 'ccm',
            tooltip: 'ccm',
          },
          {
            title: 'pcm',
            tooltip: 'pcm',
          },
          {
            title: 'bhi',
            tooltip: 'bhi',
          },
        ],
      },
      {
        title: 'users',
        icon: 'user',
        tooltip: 'users',
      },
      {
        title: 'organizations',
        icon: 'hospital',
        tooltip: 'organizations',
      },
      {
        title: 'providers',
        icon: 'user-md',
        tooltip: 'providers',
        divider: true,
      },
      {
        title: 'documents',
        icon: 'files-medical',
        tooltip: 'documents',
        divider: true,
      },
      {
        title: 'components',
        icon: 'code',
        tooltip: 'components',
        class: 'dev-hidden',
        to: '/iugo-care/components',
        divider: true,
      },
      {
        group: '/iugo-care/workspace',
        icon: 'laptop-code',
        title: 'workpsaces',
        tooltip: 'workpsaces',
        class: 'dev-hidden',
        divider: true,
        children: [
          {
            title: 'Fall Workspace',
            to: 'fall',
          },
          {
            title: 'Geo Workspace',
            to: 'geo',
          },
          {
            title: 'SOS Workspace',
            to: 'sos',
          },
        ],
      },
    ],
  }),

  computed: {
    ...mapState('patients', ['currentPatient']),
    patientName() {
      return this.currentPatient ?
          `${this.currentPatient.givenName || ''} ${this.currentPatient.familyName || ''}` : '';
    },
    patientEmail() {
      return this.currentPatient ? this.currentPatient.emailAddress : '';
    },
    patientInitials() {
      return this.currentPatient ?
          `${this.currentPatient.givenName?.charAt(0).toUpperCase() || ''}${this.currentPatient.familyName?.charAt(0).toUpperCase() || ''}` : '';
    },
    miniWidth() {
      return 42 + (16 * 2);
    },
    isMobile() {
      return this.$vuetify.breakpoint.mdAndDown;
    },
    isVisible: {
      get() {
        return this.isMobile ? this.$store.state.settings.drawer : true;
      },
      set(val) {
        this.$store.commit('settings/SET_DRAWER', val);
      },
    },
    isMini() {
      return this.isMobile ? false : !this.$store.state.settings.drawer;
    },
    computedItems() {
      return this.items.map(this.mapItem);
    },
  },

  mounted() {
    this.$root.$on('closeMenu', this.closeMenu);
  },

  methods: {
    closeMenu() {
      this.openMenu = false;
    },
    mapItem(item) {
      return {
        ...item,
        children: item.children ? item.children.map(this.mapItem) : undefined,
        title: this.$t(item.title),
      };
    },
    openPreferences() {
      this.$root.$emit('openPreferences');
      this.prefModal = true;
    },
    logout() {
      this.$auth.logout({
        returnTo: window.location.origin,
      });
    },
    enableTooltip(item) {
      // v-tooltip has not a prop to disable, have to be manually
      if (this.isMini && item.tooltip) {
        return {
          content: this.$t(item.tooltip),
          placement: 'right',
          boundariesElement: 'body',
          classes: ['add-tooltop', this.$vuetify.theme.dark ? 'theme--dark' : 'theme--light'],
          offset: 17,
        };
      }
      return null;
    },
  },
  async beforeCreate() {
    if (this.$auth.isAuthenticated) {
      this.$store.dispatch('patients/fetchCurrentPatient')
          .then(() => {
            localStorage.setItem('language', this.currentPatient ?
                this.currentPatient.patientLanguage ? this.currentPatient.patientLanguage.toLowerCase() :
                    this.$i18n.locale : this.$i18n.locale);
            this.$i18n.locale = localStorage.getItem('language');
          });
      const language = localStorage.getItem('language');
      this.$i18n.locale = (language && language !== '' && language !== 'null' && language !== 'undefined') ?
          language : 'en';

    }
  },
};
</script>
