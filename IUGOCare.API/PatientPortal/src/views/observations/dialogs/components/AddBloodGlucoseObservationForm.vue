<template>
  <v-col cols="12">
    <v-text-field
      v-model="readingValue"
      :label="$t('reading')"
      class="mb-4"
      hide-details="auto"
      suffix="mg/dL"
    ></v-text-field>
    <v-radio-group dense column hide-details="auto" class="mt-0 mb-4">
      <v-radio :label="$t('mealCodeFasting')" value="fasting"></v-radio>
      <v-radio :label="$t('mealCodeNonFasting')" value="nonfasting"></v-radio>
    </v-radio-group>
  </v-col>
</template>
<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'AddBloodGlucoseObservationForm',
  data(): any {
    return {
      readingValue: null as number,
    };
  },
});
</script>