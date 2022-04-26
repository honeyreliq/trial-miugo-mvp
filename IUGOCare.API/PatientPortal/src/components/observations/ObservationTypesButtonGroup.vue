<template>
  <v-row>
    <v-col cols="12" class="hidden-sm-and-down">
      <BaseButtonGroup
        mandatory
        class="mb-4"
        v-model="currentValue"
      >
        <BaseButton
          v-for="type in observationTypes"
          :key="type.value"
          :value="type.value"
          :disabled="type.disabled === true"
          :class="$vuetify.theme.dark ? 'darkGrey' : 'white'"
          @click="emitValue(type.value)"
        >
            {{type.text}}
        </BaseButton>
      </BaseButtonGroup>
    </v-col>
    <v-col cols="12" class="hidden-md-and-up">
      <v-select
        :items="observationTypes"
        :label="$t('observation')"
        v-model="currentValue"
        @change="setObservationType"
        class="my-4"
      />
    </v-col>
  </v-row>
</template>

<script lang="ts">
import Vue, { PropType } from 'vue';

interface ObservationType {
  text: string;
  value: string;
  disabled?: boolean;
}

export default Vue.extend({
  name: 'ObservationTypesButtonGroup',
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
