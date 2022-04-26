<template>
<div>
  <v-row>
    <v-col cols="12">
      <v-text-field :label="measurement.label"
                    v-model="measurement.value"
                    :rules="getRules"
                    hide-details="auto"
                    type="number"
                    class="inputMeasurement"
                    @keypress="onKeyPress($event)"
                    :disabled="disabledFields"
                    :suffix="units"></v-text-field>
    </v-col>
  </v-row>

  <v-row>
    <v-col cols="12">
      <v-select
          :items="['°F', '°C']"
          :label="$t('units')"
          v-model="units"
        ></v-select>
    </v-col>
  </v-row>
</div>
</template>

<script lang="ts">
  import Vue from 'vue';
  import axiosClient from '@/auth/axiosInstance';
  import { CreateManualObservationCommand, ManualObservationDataItem, ObservationsClient } from '@/IUGOCare-api';
  import { IObservationResponse, IObservationRequest, IBasicMeasurement } from '@/shared/interfaces';
  import { exists, isBetween, isDecimal, ValidationFn } from '../../../validation';

  export default Vue.extend({
    name: 'BloodPressureObservation',
    data: () => ({
      measurement: null,
      units: '°F',
    }),
    props: {
      disabledFields: Boolean,
    },
    methods: {
      getMeasurement(): IBasicMeasurement {
        return {
          code: 'body-temperature',
          value: null as number,
          unit: '',
          label: this.$t('bodyTemperature').toString(),
          rules: [],
        };
      },
      onKeyPress(evt: KeyboardEvent): void {
        const event = (evt) ? evt : window.event;
        const charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode === 43 || charCode === 45) {
          event.preventDefault();
        }
      },
      notifyObservationStatus(saved: boolean, errorMessage: string): void {
        const response: IObservationResponse = {
          isSaved: saved,
          message: errorMessage,
        };
        setTimeout(() => {
          this.$root.$emit('submitObservationEnd', response);
        }
          , 500);
      },
      saveObservation(request: IObservationRequest): void {
        const command = new CreateManualObservationCommand();
        command.effectiveDate = request.effectiveDate;
        command.observationCode = request.observationCode;
        const observationDataItems = [];
        const measurement = this.measurement;

        if (measurement &&
          measurement.value !== undefined &&
          measurement.value !== null &&
          measurement.value > 0) {

          const item = new ManualObservationDataItem();
          item.observationCode = measurement.code;
          item.unit = this.units;
          item.value = parseFloat(measurement.value.toString());
          observationDataItems.push(item);
        }
        command.observationDataList = observationDataItems;

        const obClient = new ObservationsClient(process.env.VUE_APP_API_URL, axiosClient);
        obClient.createManualObservation(command).then(
          () => {
            this.measurement.value = null;
            this.notifyObservationStatus(true, '');
          })
          .catch((ex: any) => {
            let errorMessage = 'Error: ';
            if (ex.response.data.errors) {
              const errors = ex.response.data.errors;
              Object.entries(errors).forEach(
                ([, value]) => errorMessage += value);
            }
            this.notifyObservationStatus(false, errorMessage);
          });
      },
      clearFields(): void {
        this.measurement = this.getMeasurement();
      },
    },
    computed: {
      getRules(): ValidationFn[] {
        const rangeFn: ValidationFn = this.units === '°F' ? isBetween(86, 113) : isBetween(30, 46);

        return [ exists, isDecimal(2), rangeFn ];
      },
    },
    created() {
      this.measurement = this.getMeasurement();
    },
    mounted() {
      this.$root.$on('submitObservationStart', this.saveObservation);
      this.$root.$on('clearObservationFields', this.clearFields);
    },
    beforeDestroy() {
      this.$root.$off('submitObservationStart', this.saveObservation);
      this.$root.$off('clearObservationFields', this.clearFields);
    },
  });
</script>

<style>
.inputMeasurement input[type='number'] {
  -moz-appearance: textfield;
}

.inputMeasurement input::-webkit-outer-spin-button,
.inputMeasurement input::-webkit-inner-spin-button {
  -webkit-appearance: none;
}
</style>
