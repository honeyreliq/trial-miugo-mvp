<template>
    <v-col cols="12">
      <v-text-field
        v-model="a1c"
        :label="$t('reading')"
        hide-details="auto"
        class="mb-4"
        suffix="%"
      ></v-text-field>
    </v-col>
</template>
<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'AddA1cObservationForm',
  data(): any {
    return {
      a1c: null as number,
    };
  },
});
</script>