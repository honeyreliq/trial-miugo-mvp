<template>
  <BaseCard id="iugo-care-app-bar" tile flat dark color="naturalGrey" margin="mb-0">
    <v-row class="fill-height">
      <v-col cols="auto" class="flex-grow-1 align-self-center d-md-none">
        <span class="text-subtitle-1">Patient Information</span>
      </v-col>
      <v-col
        cols="auto"
        class="flex-grow-1 align-self-center hidden-sm-and-down"
      >
        <span class="text-subtitle-1 font-weight-bold">{{$t("dob")}}: </span>
        <span class="text-subtitle-1 mr-8">{{ patientInformation.dob }}</span>

        <span class="text-subtitle-1 font-weight-bold">{{$t("phone")}}: </span>
        <span class="text-subtitle-1 mr-8">{{ patientInformation.phone }}</span>

        <span class="text-subtitle-1 font-weight-bold">{{$t("language")}}: </span>
        <span class="text-subtitle-1 mr-8">{{ patientInformation.language }}</span>

        <span class="text-subtitle-1 font-weight-bold">{{$t("allergies")}}: </span>
        <span class="text-subtitle-1 mr-8">{{ patientInformation.allergies }}</span>

      </v-col>
      <!-- <v-col cols="auto" class="d-flex justify-end">
        <BaseButton>
          <fa-icon
            @click.stop="patientModal = true"
            :icon="['fal', 'expand-alt']"
            class="fa-fw"
          />
        </BaseButton>
      </v-col> -->
    </v-row>
    <DemoPatientInformationModal
      v-model="patientModal"
      :selectedOption="selectedOptionValue"
    ></DemoPatientInformationModal>
  </BaseCard>
</template>

<script>
// Components
import DemoPatientInformationModal from '@/views/iugo-care/components/DemoPatientInformationModal.vue';

// Utilities
import { mapState, mapMutations } from 'vuex';
import i18n from '@/i18n';

export default {
  name: 'IugoCareAppBar',

  components: {
    DemoPatientInformationModal,
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
    currentLocale: i18n.locale,
    patientModal: false,
    selectedOptionValue: 'personal',
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
