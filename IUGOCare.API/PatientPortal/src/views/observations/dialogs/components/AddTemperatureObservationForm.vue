<template>
  <v-col cols="12">
    <v-text-field
      v-model="temperatureValue"
      :label="$t('reading')"
      class="mb-4"
      hide-details="auto"
      suffix="°F"
    ></v-text-field>
  </v-col>
</template>
<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'AddTemperatureObservationForm',
  data(): any {
    return {
      temperatureValue: null as number,
    };
  },
});
</script>