<template>
  <v-skeleton-loader :loading="loading" type="table" class="flex">
    <BaseDataTable
      :headers="headers"
      :items="observations"
      :server-items-length="observations.length"
      item-key="id"
      item-type="observation"
      fixed-header
      show-select
      activeRow
      :actions="actions"
      :show-expand="showExpandedCells"
      :footerProps="{ itemsPerPageText: '' }"
      class="striped overflow-y-auto"
      @action-applied="alertUser"
    >
      <!-- The core observation data -->
      <template v-slot:[`item.observation`]="{ item }">
        <!-- Show a different icon depending on severity of reading -->
        <template v-if="isTarget(item.observationStatus)">
          <fa-icon
            :style="{ color: 'var(--v-success-base)' }"
            :icon="['fas', 'check-circle']"
            class="fa-fw fa-button mr-1"
          />
          {{ displayObservationData(item.observationsData) }}
        </template>
        <template v-else-if="isAtRisk(item.observationStatus)">
          <fa-icon
            :style="{ color: 'var(--v-warning-base)' }"
            :icon="['fas', 'exclamation-circle']"
            class="fa-fw fa-button mr-1"
          />
          {{ displayObservationData(item.observationsData) }}
        </template>
        <template v-else-if="isCritical(item.observationStatus)">
          <fa-icon
            :style="{ color: 'var(--v-error-base)' }"
            :icon="['fas', 'exclamation-triangle']"
            class="fa-fw fa-button mr-1"
          />
          {{ displayObservationData(item.observationsData) }}
        </template>
        <!-- Show no icon -->
        <template v-else>
          {{ displayObservationData(item.observationsData) }}
        </template>
      </template>
      <!-- Blood glucose fasting column -->
      <template v-slot:[`item.mealCode`]="{ item }" v-if="isBloodGlucose">
        {{ $t(displayMealCode(item.observationsData)) }}
      </template>
      <!-- Date of reading -->
      <template v-slot:[`item.effectiveDate`]="{ item }">
        <span>{{ formatDate(item.effectiveDate) }}</span>
      </template>
      <!-- <template v-slot:[`item.notes`]>
        <v-btn icon @click.stop="handleButtonClick">
          <fa-icon :icon="['fal', 'file-alt']" class="fa-fw fa-button" />
        </v-btn>
      </template> -->
      <!-- Expanded cell data -->
      <template v-slot:expanded-item="{ headers, item }" v-if="showExpandedCells">
        <td :colspan="headers.length" class="elevation-0 pa-0">
          <v-simple-table class="rounded-0">
            <template v-slot:default>
              <tbody>
                <tr
                  v-for="itemDetail in getFilteredObservationData(item)"
                  :key="itemDetail.observationCode"
                >
                  <td>{{ displayObservationDataDetail(itemDetail) }}</td>
                </tr>
              </tbody>
            </template>
          </v-simple-table>
        </td>
      </template>
      <!-- Conditionally render the expansion button -->
      <template
        v-slot:[`item.data-table-expand`]="{ item, expand, isExpanded }"
        v-if="showExpandedCells"
      >
        <v-btn
          @click.stop="expand(!isExpanded)"
          :ref="item.id"
          small
          icon
          v-if="item.observationsData.length > 1"
          :isExpanded="isExpanded"
        >
          <fa-icon
            v-if="isExpanded"
            :icon="['far', 'chevron-up']"
            class="fa-fw fa-button"
          />
          <fa-icon
            v-else
            :icon="['far', 'chevron-down']"
            class="fa-fw fa-button"
          />
        </v-btn>
      </template>
    </BaseDataTable>
    <ViewNoteDialog v-model="openNoteDialog" />
  </v-skeleton-loader>
</template>

<script lang="ts">
import Vue from 'vue';
// @ts-ignore
import { cond, equals } from 'ramda';

import {
  formatDate,
  formatSecondsToHms,
} from '@/common/utils/datetime-helpers';
import { kebabToCamelCase } from '@/common/utils/text-helpers';
import {
  ObservationTypes,
  ObservationCodes,
  ObservationStatus,
} from '@/common/data/observation-constants';
import BaseDataTable from '@/components/base/BaseDataTable.vue';
import ViewNoteDialog from '../dialogs/ViewNoteDialog.vue';

export default Vue.extend({
  name: 'ObservationDataTable',
  components: {
    BaseDataTable,
    ViewNoteDialog,
  },
  props: {
    observationType: String,
    observations: {
      type: Array,
    },
  },
  data(): any {
    return {
      loading: true,
      loaded: false,
      openNoteDialog: false,
      activitySubitems: [
        ObservationCodes.DISTANCE,
        ObservationCodes.STEPS,
        ObservationCodes.CALORIES_BURNED,
        ObservationCodes.HEART_RATE,
      ],
      actions: [
        {
          name: `${this.$t('print')}`,
          icon: 'print',
          isQuickAction: true,
        },
        {
          name: `${this.$t('download')}`,
          icon: 'download',
          isQuickAction: true,
        },
        {
          name: `${this.$t('edit')}`,
          icon: '',
          isQuickAction: false,
        },
        {
          name: `${this.$t('share')}`,
          icon: '',
          isQuickAction: false,
        },
        {
          name: `${this.$t('bundle')}`,
          icon: '',
          isQuickAction: false,
        },
        {
          name: `${this.$t('delete')}`,
          icon: '',
          isQuickAction: false,
        },
      ],
    };
  },
  computed: {
    headers(): any[] {
      const i18n: string = cond<string, string>([
        [equals(ObservationTypes.BODY_TEMPERATURE), () => 'temperatureLbl'],
        [equals(ObservationTypes.BLOOD_GLUCOSE), () => 'bloodGlucoseLbl'],
        [equals(ObservationTypes.BLOOD_PRESSURE), () => 'bloodPressureLbl'],
        [equals(ObservationTypes.WEIGHT), () => 'weightLbl'],
        [
          equals(ObservationTypes.OXYGEN_SATURATION),
          () => 'oxygenSaturationLbl',
        ],
        [equals(ObservationTypes.RESPIRATORY_RATE), () => 'respiratoryRateLbl'],
        [equals(ObservationTypes.HEART_RATE), () => 'heartRateLbl'],
        [equals(ObservationTypes.A1C), () => 'A1c\xa0(%)'],
        [() => true, () => 'observation'],
      ])(this.observationType);

      const headerItems = [
        {
          text: this.$t(i18n),
          value: 'observation',
          sortable: false,
          align: 'start',
          class: 'text-subtitle-2',
        },
        {
          text: this.$t('date'),
          value: 'effectiveDate',
          sortable: false,
          class: 'text-subtitle-2',
        },
        // {
        //   text: `${this.$t('notes')}`,
        //   value: 'notes',
        //   align: 'center',
        //   sortable: false,
        //   class: 'text-subtitle-2',
        // },
      ];

      if (this.observationType === ObservationTypes.BLOOD_GLUCOSE) {
        headerItems.splice(1, 0, {
          text: `${this.$t('fastingQuestion')}`,
          value: 'mealCode',
          sortable: false,
          class: 'text-subtitle-2',
        });
      }

      if (this.showExpandedCells)
        headerItems.push({
          text: '',
          value: 'data-table-expand',
          sortable: false,
          align: 'start',
          class: ''
        });

      return headerItems;
    },
    isBloodGlucose(): boolean {
      return this.observationType === ObservationTypes.BLOOD_GLUCOSE;
    },
    showExpandedCells(): boolean {
      return this.observationType === ObservationTypes.ACTIVITY ||
        this.observationType === ObservationTypes.SLEEP;
    }
  },
  methods: {
    // imported methods
    formatDate,
    formatSecondsToHms,
    // local methods
    displayMealCode(data: any[]): string {
      const mealCode = data.find(
        (d) => d.observationCode.indexOf('meal-code') === 0
      );
      if (!mealCode) {
        return '';
      }
      return kebabToCamelCase(mealCode.observationCode);
    },
    displayObservationData(data: any[]): string {
      if (this.observationType === ObservationTypes.BLOOD_PRESSURE) {
        const systolic =
          data.find((d) => d.observationCode === ObservationCodes.SYSTOLIC)
            ?.value || 0;
        const diastolic =
          data.find((d) => d.observationCode === ObservationCodes.DIASTOLIC)
            ?.value || 0;
        const heartRate =
          data.find((d) => d.observationCode === ObservationCodes.HEART_RATE)
            ?.value || 0;
        return `${systolic}/${diastolic} ${this.$t(
          'heartRateAbr'
        )}: ${heartRate}`;
      } else if (this.observationType === ObservationTypes.OXYGEN_SATURATION) {
        const oxygenSaturation =
          data.find((d) => d.observationCode === 'oxygen-saturation')?.value ||
          0;
        const heartRate =
          data.find((d) => d.observationCode === ObservationCodes.HEART_RATE)
            ?.value || 0;
        return `${oxygenSaturation}% ${this.$t('heartRateAbr')}: ${heartRate}`;
      } else if (this.observationType === ObservationTypes.HEART_RATE) {
        const heartRate =
          data.find((d) => d.observationCode === ObservationCodes.HEART_RATE)
            ?.value || 0;
        return `${heartRate} bpm`;
      } else if (this.observationType === ObservationTypes.RESPIRATORY_RATE) {
        const respiratoryRate =
          data.find(
            (d: any) => d.observationCode === ObservationCodes.RESPIRATORY_RATE
          )?.value || 0;
        return `${respiratoryRate} rpm`;
      } else if (this.observationType === ObservationTypes.BLOOD_GLUCOSE) {
        const observationData = data.find(
          (d: any) => d.observationCode === ObservationCodes.BLOOD_GLUCOSE
        );
        const bloodGlucose = observationData?.value;
        const bloodGlucoseUnit = observationData?.unit;
        return `${bloodGlucose} ${bloodGlucoseUnit}`;
      } else {
        if (data.length > 0) {
          const datum = data[0];
          return `${datum.value} ${datum.unit}`;
        } else {
          return '';
        }
      }
    },
    isAtRisk(status: string): boolean {
      return status === ObservationStatus.AT_RISK;
    },
    isCritical(status: string): boolean {
      return status === ObservationStatus.CRITICAL;
    },
    isTarget(status: string): boolean {
      return status === ObservationStatus.TARGET;
    },
    getFilteredObservationData(item: any): any[] {
      let observationData: any[] = [];
      // return all the observation data without the element we've already displayed once
      if (this.observationType === ObservationTypes.SLEEP) {
        observationData = item.observationsData.filter(
          (o: any) => o.observationCode !== ObservationCodes.TOTAL
        );
      } else if (this.observationType === ObservationTypes.ACTIVITY) {
        observationData = item.observationsData.filter(
          (o: any) => o.observationCode !== ObservationCodes.FITNESS_DURATION
        );
      }
      return observationData.filter((dto) => this.verifySubItem(dto));
    },
    handleButtonClick() {
      this.openNoteDialog = true;
    },
    verifySubItem(data: any): boolean {
      if (this.observationType == ObservationTypes.ACTIVITY) {
        // ensure there's at least 1 element from our expected list of possible data items
        return this.activitySubitems.includes(data.observationCode);
      }
      return true;
    },
    displayObservationDataDetail(data: any): string {
      let detailLabel = '';
      // format the expanded rows of sleep observations
      if (this.observationType === ObservationTypes.SLEEP) {
        // tried reducing this to
        // return `${this.$t(data.ObservationCode).toString()}: ${this.formatSecondsToHms(data.value)}`;
        // and it doesn't work. why?
        switch (data.observationCode) {
          case ObservationCodes.REM:
            detailLabel = this.$t(ObservationCodes.REM).toString();
            break;
          case ObservationCodes.DEEP:
            detailLabel = this.$t(ObservationCodes.DEEP).toString();
            break;
          case ObservationCodes.LIGHT:
            detailLabel = this.$t(ObservationCodes.LIGHT).toString();
            break;
          case ObservationCodes.AWAKE:
            detailLabel = this.$t(ObservationCodes.AWAKE).toString();
            break;
        }
        return `${detailLabel}: ${this.formatSecondsToHms(data.value)}`;
      }
      // format the expanded rows of activity observations
      else if (this.observationType === ObservationTypes.ACTIVITY) {
        switch (data.observationCode) {
          case ObservationCodes.DISTANCE:
            detailLabel = `${this.$t('distance')}: ${data.value} ${data.unit}`;
            break;
          case ObservationCodes.STEPS:
            detailLabel = `${this.$t('steps')}: ${data.value} ${this.$t(
              data.unit.toLowerCase()
            )}`;
            break;
          case ObservationCodes.CALORIES_BURNED:
            detailLabel = `${this.$t('caloriesBurned')}: ${data.value} ${
              data.unit
            }`;
            break;
          case ObservationCodes.HEART_RATE:
            detailLabel = `${this.$t('average')} ${this.$t('heartRate')}: ${
              data.value
            } ${data.unit}`;
            break;
        }
        return detailLabel;
      }
    },
    alertUser(payload: any): void {
      alert(
        `${payload.name} action triggered against ${payload.selectedRows.length} items.`
      );
    },
  },
  async created() {
    const readyHandler = () => {
      if (document.readyState == 'complete') {
        this.loading = false;
        this.loaded = true;
        document.removeEventListener('readystatechange', readyHandler);
      }
    };
    document.addEventListener('readystatechange', readyHandler);
    readyHandler();
  },
});
</script>
