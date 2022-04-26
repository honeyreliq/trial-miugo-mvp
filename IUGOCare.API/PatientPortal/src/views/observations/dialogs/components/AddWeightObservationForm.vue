<template>
    <v-col cols="12">
      <v-text-field
        v-model="weight"
        :label="$t('reading')"
        hide-details="auto"
        class="mb-4"
        suffix="lbs"
      ></v-text-field>
    </v-col>
</template>
<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'AddWeightObservationForm',
  data(): any {
    return {
      weight: null as number,
    };
  },
});
</script>