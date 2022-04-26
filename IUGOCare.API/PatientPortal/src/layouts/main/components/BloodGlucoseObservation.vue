<template>
  <div>
    <v-row v-for="measurement in bloodGlucoseMeasurements" :key="measurement.code">
      <v-col cols="8">
        <v-text-field :label="measurement.label"
                      v-model="measurement.value"
                      :rules="measurement.rules"
                      :placeholder="measurement.placeholder"
                      type="number"
                      class="inputMeasurement"
                      @keypress="onKeyPress($event)"
                      :disabled="disabledFields"></v-text-field>
      </v-col>
      <v-col cols="4">
        <v-select v-model="measurement.unit"
                  :label="$t('units')"
                  item-text="value"
                  item-value="value"
                  :items="measurement.units"
                  :disabled="disabledFields"
                  :rules="getUnitRules"
                  required />
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-radio-group v-model="mealCode" row>
          <v-radio :label="$t('fastingPreMeal')" value="meal-code-fasting"></v-radio>
          <v-radio :label="$t('nonFastingPostMeal')" value="meal-code-non-fasting"></v-radio>
        </v-radio-group>
      </v-col>
    </v-row>
  </div>
</template>
<script lang="ts">
  import Vue from 'vue';
  import axiosClient from '@/auth/axiosInstance';
  import { CreateManualObservationCommand, ManualObservationDataItem, ObservationsClient } from '@/IUGOCare-api';
  import { IObservationResponse, IObservationRequest, IComplexMeasurement, IUnit } from '@/shared/interfaces';
  import { exists, isDecimal, isSelected } from '../../../validation';

  export default Vue.extend({
    name: 'BloodGlucoseObservation',
    data: () => ({
      bloodGlucoseMeasurements: [],
      mealCode: 'meal-code-fasting',
    }),
    props: {
      disabledFields: Boolean,
    },
    methods: {
      getBloodGlucoseMeasurements(): IComplexMeasurement[] {
        return [
          {
            code: 'blood-glucose',
            units: this.getAvailableUnits,
            value: null as number,
            label: this.$t('bloodGlucose').toString(),
            rules: this.getBloodGlucoseRules,
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
        const observationDataItems = [];

        this.bloodGlucoseMeasurements.forEach((measurementItem) => {
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

        if (this.mealCode) {
          const mealCode = new ManualObservationDataItem();
          mealCode.observationCode = this.mealCode;
          observationDataItems.push(mealCode);
        }

        command.observationDataList = observationDataItems;

        const obClient = new ObservationsClient(process.env.VUE_APP_API_URL, axiosClient);
        obClient.createManualObservation(command).then(
          () => {
            this.getBloodGlucoseMeasurements().forEach((measurementItem) => measurementItem.value = null);
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
        this.bloodGlucoseMeasurements[0].value = null;
      },
      getMeasurement(measurementCode: string): IComplexMeasurement {
        return this.bloodGlucoseMeasurements?.find((m: IComplexMeasurement) =>
          m.code === measurementCode);
      },
      getUnit(measurementCode: string): IUnit {
        const measurement = this.getMeasurement(measurementCode);
        return measurement?.units?.find((u: IUnit) => u.value === measurement.unit) || null;
      },
      validateRange(value: number, measurementCode: string): boolean {
        const unit = this.getUnit(measurementCode);
        if (unit) {
          return value >= unit.minValue && value <= unit.maxValue;
        }
        return true;
      },
      getRangeText(measurementCode: string): string {
        const unit = this.getUnit(measurementCode);
        if (unit) {
          return this.$t('outOfRange', { min: unit.minValue, max: unit.maxValue }).toString();
        }
        return '';
      },
    },
    computed: {
      getBloodGlucoseRules(): any[] {
        const rangeFn = (v: number) => !v || this.validateRange(v, 'blood-glucose') ||
          this.getRangeText('blood-glucose');
        return [exists, isDecimal(2), rangeFn];
      },
      getUnitRules(): any[] {
        return [ isSelected ];
      },
      getAvailableUnits(): IUnit[] {
        return [
          { value: 'mg/dL', minValue: 0, maxValue: 750 },
          { value: 'mmol/L', minValue: 0, maxValue: 56 },
        ];
      },
    },
    created() {
      this.bloodGlucoseMeasurements = this.getBloodGlucoseMeasurements();
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
