<template>
    <v-col cols="12">
      <v-text-field
        v-model="systolicValue"
        :label="$t('systolic')"
        hide-details="auto"
        class="mb-4"
        suffix="mmHg"
      ></v-text-field>
      <v-text-field
        v-model="diastolicValue"
        :label="$t('diastolic')"
        hide-details="auto"
        class="mb-4"
        suffix="mmHg"
      ></v-text-field>
      <v-text-field
        v-model="pulseValue"
         :label="$t('pulse')"
        hide-details="auto"
        class="mb-4"
        suffix="bpm"
      ></v-text-field>
    </v-col>
</template>
<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'AddBloodPressureObservationForm',
  data(): any {
    return {
      systolicValue: null as number,
      diastolicValue: null as number,
      pulseValue: null as number,
    };
  },
});
</script>