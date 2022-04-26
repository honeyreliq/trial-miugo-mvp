<template>
    <div class="d-flex fill-width">
        <v-col cols="12" md="6">
        <v-text-field
            v-model="oxygenSaturationValue"
            :label="$t('oxygenSaturation')"
            hide-details="auto"
            class="mb-4"
            suffix="%"
        ></v-text-field>
        </v-col>
        <v-col cols="12" md="6">
        <v-text-field
            v-model="pulseValue"
            :label="$t('pulseBpm')"
            hide-details="auto"
            class="mb-4"
            suffix="bpm"
        ></v-text-field>
        </v-col>
    </div>
</template>
<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'AddOxygenSaturationObservationForm',
  data(): any {
    return {
      oxygenSaturationValue: null as number,
      pulseValue: null as number,
    };
  },
});
</script>