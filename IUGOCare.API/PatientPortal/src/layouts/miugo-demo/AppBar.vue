<template>
  <v-app-bar id="patient-portal-app-bar"
    absolute
    app
    flat
    :class="appBarColor.class"
    :dark="appBarColor.dark"
    :light="appBarColor.light"
    >
    <BaseButton
      class="mr-4"
      @click="setDrawer(!drawer)"
    >
      <fa-icon v-if="value" :icon="['fal', 'bars']" class="fa-fw" />
      <fa-icon v-else :icon="['fal', 'bars']" class="fa-fw" />
    </BaseButton>
    <!-- <router-link to="/miugo/" class="d-flex overflow-hidden">
      <span class="avatar hidden-sm-and-up mr-4">
        <BaseLinkedImage
          :src="require(`@/assets/${mobileImg}`)"
          contain
          height="100%"
          position="left center"
        />
      </span>
    </router-link> -->
    <v-toolbar-title v-text="getPageCategory()" />
    <v-spacer />
    <v-menu
      bottom
      left
      offset-y
      origin="top right"
      transition="scroll-y-transition"
      max-width="300"
      min-width="300"
      content-class="background pa-3">
      <template v-slot:activator="{ attrs, on }">
        <BaseButton class="mr-4"
                    v-bind="attrs"
                    @click="notifications = 0"
                    v-on="on">
          <v-badge
            :content="notifications"
            :value="notifications"
            color="error"
            overlap
            bordered
            light
            offset-x="9"
            offset-y="10">
            <fa-icon :icon="['fad', 'bell']" class="fa-fw" />
          </v-badge>
        </BaseButton>
      </template>
      <DemoNotificationsMenu></DemoNotificationsMenu>
    </v-menu>
    <v-menu
      bottom
      left
      offset-y
      origin="top right"
      transition="scroll-y-transition"
      min-width="200"
    >
      <template v-slot:activator="{ attrs, on }">
        <!-- CHRIS RYAN: WIP >> -->
        <v-btn
          rounded
          depressed
          v-bind="attrs"
          v-on="on"
          class="pl-0 user-button"
          :class="{'px-0': $vuetify.breakpoint.xs}"
        >
          <v-list-item-avatar
            size="42"
            class="my-0 darkGrey"
            :class="{'mr-0': $vuetify.breakpoint.xs}"
          >
            <!-- <span class="white--text text-h6">{{ patientInitials }}</span> -->
            <!-- Fake file submit for Demo -->
            <span class="white--text text-h6">MC</span>
          </v-list-item-avatar>
          <v-list-item-title class="hidden-xs-only">
            <!-- <span v-if="patientName" class="hidden-xs-only">{{ patientName }}</span>
            <span v-else class="hidden-xs-only text-lowercase">{{ patientEmail }}</span> -->
            <!-- Fake file submit for Demo -->
            <span v-if="patientName" class="hidden-xs-only">Monique Alicia Caron</span>
          </v-list-item-title>
          <fa-icon right dark :icon="['fas', 'caret-down']" class="fa-fw fa-button mx-2 hidden-xs-only" />
        </v-btn>
        <!-- << CHRIS RYAN: WIP -->
      </template>

      <v-list class="py-0" flat>
        <v-list-item-group>
          <v-list-item @click="userModal = true; selectedOptionValue = 'account'">
            <v-list-item-icon class="mr-4">
              <fa-icon :icon="['fad', 'user-circle']" class="fa-fw" />
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>{{ $t('account')}}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-list-item @click="userModal = true; selectedOptionValue = 'profile'">
            <v-list-item-icon class="mr-4">
              <fa-icon :icon="['fad', 'address-card']" class="fa-fw" />
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>{{ $t('profile')}}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-list-item @click="openPreferences()">
            <v-list-item-icon class="mr-4">
              <fa-icon :icon="['fad', 'cog']" class="fa-fw" />
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>{{ $t('preferences')}}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-divider />
          <v-list-item @click="helpModal = true">
            <v-list-item-icon class="mr-4">
              <fa-icon :icon="['fad', 'question-circle']" class="fa-fw" />
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>{{ $t('help')}}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <!-- URL should be pulled from the DB for when we have an admin section. -->
          <v-list-item target="_blank" href="https://reliqhealth.freshdesk.com">
            <v-list-item-icon class="mr-4">
              <fa-icon :icon="['fad', 'life-ring']" class="fa-fw" />
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>{{ $t('support')}}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-list-item @click="logout">
            <v-list-item-icon class="mr-4">
              <fa-icon :icon="['fad', 'sign-out-alt']" class="fa-fw" />
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>{{$t('logout')}}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </v-list-item-group>
      </v-list>
    </v-menu>
    <UserSettings v-model="userModal" :selectedOption="selectedOptionValue"></UserSettings>
    <UserPreferences v-model="prefModal" ></UserPreferences>
    <UserHelp v-model="helpModal"></UserHelp>
  </v-app-bar>
</template>

<script>
  // Components
  import UserSettings from './components/UserSettings.vue';
  import UserPreferences from './components/UserPreferences.vue';
  import UserHelp from './components/UserHelp.vue';
  import DemoNotificationsMenu from '../../views/miugo-demo/components/DemoNotificationsMenu';
  // Utilities
  import { mapState, mapMutations } from 'vuex';
  import i18n from '@/i18n';
  export default {
    name: 'PatientPortalAppBar',
    components: {
      UserSettings, UserPreferences, UserHelp, DemoNotificationsMenu,
    },
    computed: {
      ...mapState('settings', ['drawer']),
      ...mapState('patients', ['currentPatient']),
      patientName() { return this.currentPatient ? `${this.currentPatient.givenName || ''} ${this.currentPatient.familyName || ''}` : ''; },
      patientEmail() { return this.currentPatient ? this.currentPatient.emailAddress : ''; },
      patientInitials() {
        return this.currentPatient ? `${this.currentPatient.givenName?.charAt(0).toUpperCase() || ''}${this.currentPatient.familyName?.charAt(0).toUpperCase() || ''}` : '';
      },
    },
    props: {
      value: {
        type: Boolean,
        default: false,
      },
    },
    data: () => ({
      mobileImg: 'iUGO_Mobile_avatar_W.svg',
      userModal: false,
      prefModal: false,
      helpModal: false,
      notifications: 2,
      selectedOptionValue: '',
      currentLocale: i18n.locale,
      appBarColor: {
        class: '',
        dark: false,
        light: false,
      },
    }),
    methods: {
      ...mapMutations('settings', {
        setDrawer: 'SET_DRAWER',
      }),
      logout() {
        this.$auth.logout({
          returnTo: window.location.origin,
        });
      },
      openPreferences() {
        this.$root.$emit('openPreferences');
        this.prefModal = true;
      },
      updateSelectedMenuOption(newValue) {
        this.selectedOptionValue = newValue;
      },
      setAppBarColor(appBarColor) {
        this.appBarColor.class = appBarColor.class;
        this.appBarColor.dark = appBarColor.dark;
        this.appBarColor.light = appBarColor.light;
      },
      getPageCategory() {
        const param = (this.$route.params && this.$route.params.observationCategory) ?
          this.$route.params.observationCategory : '';
        if (param) {
          return `${this.$t(this.$route.name)} - ${this.$t(this.normalizeParam(param))}`;
        }
        return `${this.$t(this.$route.name)}`;
      },
      normalizeParam(input) {
        return input.toLowerCase().replace(/-(.)/g, (match, group1) => {
          return group1.toUpperCase();
        });
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
    beforeMount() {
      this.$root.$on('updateSelectedMenuOption', this.updateSelectedMenuOption);
      this.$root.$on('setAppBarColor', this.setAppBarColor);
    },
  };
</script>
