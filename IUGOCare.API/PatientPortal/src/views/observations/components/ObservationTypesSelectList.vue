<template>
  <v-select
    :items="observationTypes"
    :label="$t('observation')"
    v-model="currentValue"
    @change="emitValue(currentValue)"
    hide-details="true"
    class="mt-0"
  />
</template>

<script lang="ts">
import Vue, { PropType } from 'vue';

interface ObservationType {
  text: string;
  value: string;
  disabled?: boolean;
}

export default Vue.extend({
  name: 'ObservationTypesSelectList',
  data(): any {
    return {
      currentValue: this.value,
    };
  },
  props: {
    value: String,
    observationTypes: {
      type: Array as PropType<ObservationType[]>,
      default: [],
    },
  },
  methods: {
    emitValue(value: string) {
      this.$emit('input', value);
      this.emitObservationTypeChange(value);
    },
    setObservationType() {
      this.emitValue(this.value);
    },
    emitObservationTypeChange(newValue: string): void {
      this.$root.$emit('observationTypeChange', newValue);
    },
  },
  mounted() {
    this.emitObservationTypeChange(this.value);
  },
});
</script>