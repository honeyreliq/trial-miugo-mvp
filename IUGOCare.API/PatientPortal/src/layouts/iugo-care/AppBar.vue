<template>
  <v-app-bar
    id="iugo-care-app-bar"
    fixed
    app
    flat
    dark
    color="primary"
    :value="true"
  >
    <v-row class="fill-height" align="center" justify="center">
      <v-col cols="auto" class="flex-grow-0 ml-4">
        <BaseButton text @click="setDrawer(!drawer)">
          <fa-icon v-if="value" :icon="['fal', 'bars']" class="fa-fw" />
          <fa-icon v-else :icon="['fal', 'bars']" class="fa-fw" />
        </BaseButton>
      </v-col>
      <v-col cols="auto" class="flex-grow-0 align-self-center">
        <!-- <v-toolbar-title v-text="$t($route.name)" /> -->
        <v-toolbar-title
          >{{ patientInformation.firstName }},
          {{ patientInformation.lastName }} {{ patientInformation.middleName }} -
          {{ patientInformation.age }}
          {{ patientInformation.gender }}
        </v-toolbar-title>
      </v-col>
      <v-col cols="auto" class="flex-grow-1 align-self-center">
        <BaseButton>
          <fa-icon
            @click.stop="patientModal = true"
            :icon="['fal', 'expand-alt']"
            class="fa-fw fa-button"
          />
        </BaseButton>
      </v-col>
      <v-col cols="auto" class="flex-grow-0 align-self-center fill-height hidden-xs-only" align="center" justify="center">

        <DemoTimer></DemoTimer>
      </v-col>
    </v-row>
    <DemoPatientInformationModal
      v-model="patientModal"
      :selectedOption="selectedOptionValue"
    ></DemoPatientInformationModal>
  </v-app-bar>
</template>

<script>
// Components
import DemoTimer from '@/views/iugo-care/components/DemoTimer.vue';
import DemoPatientInformationModal from '@/views/iugo-care/components/DemoPatientInformationModal.vue';

// Utilities
import { mapState, mapMutations } from 'vuex';
import i18n from '@/i18n';

export default {
  name: 'IugoCareAppBar',

  components: {
    DemoTimer, DemoPatientInformationModal,
  },
  computed: {
    ...mapState('settings', ['drawer']),
  },
  props: {
    value: {
      type: Boolean,
      default: false,
    },
  },
  data: () => ({
    mobileImg: 'iUGO_Mobile_avatar_W.svg',
    patientModal: false,
    selectedOptionValue: '',
    currentLocale: i18n.locale,
    patientInformation: {
      lastName: 'Monique',
      firstName: 'Caron',
      middleName: 'Alicia (Alicia)',
      age: '67',
      gender: 'F',
      dob: '04/28/1953',
      phone: '+1 (345) 888-7728',
      language: 'English',
      allergies: 'Penicillin, Peanuts',
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

    updateSelectedMenuOption(newValue) {
      this.selectedOptionValue = newValue;
    },
  },

  mounted() {
    this.$root.$on('updateSelectedMenuOption', this.updateSelectedMenuOption);
  },
};
</script>
<style lang="scss" scoped>
::v-deep .v-toolbar__content {
  padding: 0px !important;
}
</style>
