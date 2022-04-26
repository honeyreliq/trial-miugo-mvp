<template>
  <v-sheet class="transparent">
    <v-row>
      <v-col cols="auto" class="flex-grow-1 pa-0 ma-0">
        <v-row class="mb-1">
          <v-col>
            <span class="text-h6">{{patientInformation.firstName}}, {{patientInformation.lastName}} {{patientInformation.middleName}},
                        {{patientInformation.age}} {{patientInformation.gender}}</span>
          </v-col>
        </v-row>
        <v-row class="hidden-sm-and-down">
          <v-col cols="4" md="3">
            <span class="text-subtitle-1">DOB: {{patientInformation.dob}}</span>
          </v-col>
          <v-col cols="4" md="3">
            <span class="text-subtitle-1">Phone: {{patientInformation.phone}}</span>
          </v-col>
          <v-col cols="4" md="3">
            <span class="text-subtitle-1">Language: {{patientInformation.language}}</span>
          </v-col>
          <v-col cols="4" md="3">
            <span class="text-subtitle-1">Allergies: {{patientInformation.allergies}}</span>
          </v-col>
        </v-row>
      </v-col>
      <v-row align="center" justify="end">
        <BaseButton class="mr-4">
        <fa-icon @click.stop="patientModal = true" :icon="['fal', 'expand-alt']" class="fa-fw"/>
        </BaseButton>
      </v-row>
    </v-row>
    <DemoPatientInformationModal v-model="patientModal"
                                 :selectedOption="selectedOptionValue"></DemoPatientInformationModal>
  </v-sheet>
</template>
<script lang="ts">
  import Vue from 'vue';
  import DemoPatientInformationModal from '@/views/iugo-care/components/DemoPatientInformationModal.vue';

  export default Vue.extend({
    name: 'DemoPatientInfoTopBar',
    components: {DemoPatientInformationModal},
    data: () => ({
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
  });
</script>
