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
              <span class="align-center text-no-wrap">{{ $t('nA') }}</span>
            </td>
            <td class="border-right">
              <span class="align-center text-no-wrap">{{ $t('nA') }}</span>
            </td>
            <td class="border-right">
              <span class="align-center text-no-wrap">
                {{ monitoringParameter.atRiskHigh }}
                {{ monitoringParameter.unitOfMeasure }}
              </span>
            </td>
            <td>
              <span class="align-center text-no-wrap">
                {{ monitoringParameter.criticalHigh }}
                {{ monitoringParameter.unitOfMeasure }}
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

export default Vue.extend({
  name: 'TemperatureTable',
  props: {
    monitoringParameter: {
      type: Object as () => MonitoringParameter,
      default(): MonitoringParameter {
        return {
          observationDataType: '',
          criticalLow: 'N/A',
          atRiskLow: 'N/A',
          atRiskHigh: 'N/A',
          criticalHigh: 'N/A',
          unitOfMeasure: '',
          id: '',
          patientId: '',
        };
      },
    },
  },
  data(): any {
    return{
      trackAdherence: false,
    }
  }
});
</script>