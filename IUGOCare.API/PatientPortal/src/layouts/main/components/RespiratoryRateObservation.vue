<template>
  <div>
    <v-row v-for="measurement in respiratoryRateMeasurements" :key="measurement.code">
      <v-col cols="12">
        <v-text-field :label="measurement.label"
                      v-model="measurement.value"
                      :rules="measurement.rules"
                      :placeholder="measurement.placeholder"
                      type="number"
                      class="inputMeasurement"
                      @keypress="onKeyPress($event)"
                      :disabled="disabledFields"
                      :suffix="measurement.unit"></v-text-field>
      </v-col>
    </v-row>
  </div>
</template>
<script lang="ts">
  import Vue from 'vue';
  import axiosClient from '@/auth/axiosInstance';
  import { CreateManualObservationCommand, ManualObservationDataItem, ObservationsClient } from '@/IUGOCare-api';
  import { IObservationResponse, IObservationRequest, IBasicMeasurement } from '@/shared/interfaces';
  import { exists, isBetween, isDecimal } from '../../../validation';

  export default Vue.extend({
    name: 'RespiratoryRateObservation',
    data: () => ({
      respiratoryRateMeasurements: [],
    }),
    props: {
      disabledFields: Boolean,
    },
    methods: {
      getRespiratoryRateMeasurements(): IBasicMeasurement[] {
        return [
          {
            code: 'respiratory-rate',
            unit: 'rpm',
            value: null as number,
            label: this.$t('respiratoryRate').toString(),
            rules: this.getRespiratoryRateRules,
          },
        ];
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
        const observationDataItems: ManualObservationDataItem[] = [];

        this.respiratoryRateMeasurements.forEach((measurementItem) => {
          if (measurementItem &&
            measurementItem.value !== undefined &&
            measurementItem.value !== null &&
            measurementItem.value > 0) {
            const item = new ManualObservationDataItem();
            item.observationCode = measurementItem.code;
            item.unit = measurementItem.unit;
            item.value = parseFloat(measurementItem.value.toString());
            observationDataItems.push(item);
          }
        });
        command.observationDataList = observationDataItems;

        const obClient = new ObservationsClient(process.env.VUE_APP_API_URL, axiosClient);
        obClient.createManualObservation(command).then(
          () => {
            this.respiratoryRateMeasurements.forEach((measurementItem) => measurementItem.value = null);
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
        this.respiratoryRateMeasurements = this.getRespiratoryRateMeasurements();
      },
    },
    computed: {
      getRespiratoryRateRules(): any[] {
        return [exists, isDecimal(2), isBetween(1, 60)];
      },
    },
    created() {
      this.respiratoryRateMeasurements = this.getRespiratoryRateMeasurements();
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
