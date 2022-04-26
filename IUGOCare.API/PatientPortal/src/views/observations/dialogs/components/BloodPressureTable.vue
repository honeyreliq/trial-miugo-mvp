<template>
  <v-container fluid class="pa-0">
    <v-form>
      <v-simple-table class="mb-4">
        <tbody>
          <tr class="disable-events text-center border-bottom">
            <td class="border-bottom border-right">
              <span class="d-flex align-center justify-center my-2">
                <fa-icon
                  :icon="['fas', 'exclamation-triangle']"
                  class="fa-fw mr-2"
                  style="color: var(--v-error-base)"
              />
                <span>{{ $t('criticalLow') }}</span>
              </span>
            </td>
            <td class="border-bottom border-right">
              <span class="d-flex align-center justify-center my-2">
                <fa-icon
                  :icon="['fas', 'exclamation-circle']"
                  class="fa-fw mr-2"
                  style="color: var(--v-warning-base)"
              />
                <span>{{ $t('atRiskLow') }}</span>
              </span>
            </td>
            <td rowspan="10" scope="col" class="success dark white--text border-right">
              <span class="d-flex align-center justify-center my-2">
                <fa-icon
                  :icon="['fas', 'check-circle']"
                  class="fa-fw mr-2"
                  style="color: var(--v-white-base)"
              />
                <span>{{ $t('target') }}</span>
              </span>
            </td>
            <td class="border-bottom border-right">
              <span class="d-flex align-center justify-center my-2">
                <fa-icon
                  :icon="['fas', 'exclamation-circle']"
                  class="fa-fw mr-2"
                  style="color: var(--v-warning-base)"
                />
                <span>{{ $t('atRiskHigh') }}</span>
              </span>
            </td>
            <td class="border-bottom">
              <span class="d-flex align-center justify-center my-2">
                <fa-icon
                  :icon="['fas', 'exclamation-triangle']"
                  class="fa-fw mr-2"
                  style="color: var(--v-error-base)"
              />
                <span>{{ $t('criticalHigh') }}</span>
              </span>
            </td>
          </tr>
          <tr class="text-center white">
            <td class="border-right">
              <span class="text-no-wrap">
                {{ systolic.criticalLow }} / {{ diastolic.criticalLow }}
                {{ systolic.unitOfMeasure }}
              </span>
            </td>
            <td class="border-right">
              <span class="text-no-wrap">
                {{ systolic.atRiskLow }} / {{ diastolic.atRiskLow }}
                {{ systolic.unitOfMeasure }}
              </span>
            </td>
            <td class="border-right">
              <span class="text-no-wrap">
                {{ systolic.atRiskHigh }} / {{ diastolic.atRiskHigh }}
                {{ systolic.unitOfMeasure }}
              </span>
            </td>
            <td>
              <span class="text-no-wrap">
                {{ systolic.criticalHigh }} / {{ diastolic.criticalHigh }}
                {{ systolic.unitOfMeasure }}
              </span>
            </td>
          </tr>
        </tbody>
      </v-simple-table>
    </v-form>
    <span class="d-flex align-center">
      <v-switch
        v-model="trackAdherence"
        inset
        small
        :label="$t('trackAdherence')"
        hide-details
        dense
        class="mt-0"
      />
      <v-spacer></v-spacer>
      <div class="float-sm-right">
        <BaseButton 
          class="mr-4"
          small
        >
          {{ $t('useDefaults') }}
        </BaseButton>
        <BaseButton 
          color="primary"
          small
        >
          {{ $t('edit') }}
        </BaseButton>
      </div>
    </span>
  </v-container>
</template>
<script lang="ts">
import { MonitoringParameter } from '@/store/patient-chart/interfaces';
import Vue from 'vue';
import { ObservationDataTypes } from '@/common/data/observation-constants';

export default Vue.extend({
  name: 'BloodPressureTable',
  props: {
    monitoringParameters: {
      type: Array as () => Array<MonitoringParameter>,
      default(): Array<MonitoringParameter> {
        const defaultParameters = [
          {
            observationDataType: ObservationDataTypes.BLOOD_PRESSURE_SYSTOLIC,
            criticalLow: 'N/A',
            atRiskLow: 'N/A',
            atRiskHigh: 'N/A',
            criticalHigh: 'N/A',
            unitOfMeasure: '',
            id: '',
            patientId: '',
          },
          {
            observationDataType: ObservationDataTypes.BLOOD_PRESSURE_DIASTOLIC,
            criticalLow: 'N/A',
            atRiskLow: 'N/A',
            atRiskHigh: 'N/A',
            criticalHigh: 'N/A',
            id: '',
            patientId: '',
          },
        ] as Array<MonitoringParameter>;
        return defaultParameters;
      },
    },
  },
  data(): any {
    return {
      trackAdherence: false,
      defaultParameter: {
        criticalLow: 'N/A',
        atRiskLow: 'N/A',
        atRiskHigh: 'N/A',
        criticalHigh: 'N/A',
        unitOfMeasure: '',
        id: '',
        patientId: '',
        observationDataType: '',
      } as MonitoringParameter,
    };
  },
  computed: {
    systolic(): MonitoringParameter {
      return this.monitoringParameters.find(
        (mp: MonitoringParameter) =>
          mp.observationDataType ===
          ObservationDataTypes.BLOOD_PRESSURE_SYSTOLIC
      );
    },
    diastolic(): MonitoringParameter {
      return this.monitoringParameters.find(
        (mp: MonitoringParameter) =>
          mp.observationDataType ===
          ObservationDataTypes.BLOOD_PRESSURE_DIASTOLIC
      );
    },
  },
});
</script>