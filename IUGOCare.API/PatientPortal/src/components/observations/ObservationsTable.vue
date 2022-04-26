<template>
  <v-data-table :headers="headers"
                :items="observations"
                item-key="id"
                :show-expand="showExpand"
                class="striped overflow-y-auto"
                :loading="loading"
                :loading-text="$t('loadingMsg')"
                fixed-header
                height="calc(100vh - 360px)"
                :footerProps="footerProps">
    <!--
    TO DO: header is not being used- do you have a plan for this @Matt ?
    <template
      v-slot:[`header.data-table-expand`]="{ header }"v-if="showExpand">
      <a @click="toggleAll" class="text-decoration-none">{{$t('toggleAll')}}</a>
    </template> -->
    <template v-slot:[`item.data-table-expand`]="{ item, expand, isExpanded }" v-if="showExpand">
      <v-btn @click="expand(!isExpanded)" :ref="item.id" small icon v-if="item.observationsData.length > 1" :isExpanded="isExpanded">
        <fa-icon v-if="isExpanded" :icon="['far', 'chevron-up']" class="fa-fw fa-button" />
        <fa-icon v-else :icon="['far', 'chevron-down']" class="fa-fw fa-button" />
      </v-btn>
    </template>
    <template v-slot:[`item.observation`]="{ item }">
      <template align="center" v-if="item.observationStatus === 'Stable'">
        <fa-icon :style="{ color: 'var(--v-success-base)' }" :icon="['fas', 'check-circle']" class="fa-fw fa-button" /> {{displayObservationData(item.observationsData)}}
      </template>
      <template align="center" v-else-if="item.observationStatus === 'AtRisk'">
        <fa-icon :style="{ color: 'var(--v-warning-base)' }" :icon="['fas', 'exclamation-circle']" class="fa-fw fa-button" /> {{displayObservationData(item.observationsData)}}
      </template>
      <template align="center" v-else-if="item.observationStatus === 'Critical'">
        <fa-icon :style="{ color: 'var(--v-error-base)' }" :icon="['fas', 'exclamation-triangle']" class="fa-fw fa-button" /> {{displayObservationData(item.observationsData)}}
      </template>
      <template align="center" v-else>
        {{displayObservationData(item.observationsData)}}
      </template>
    </template>
    <template v-slot:[`item.effectiveDate`]="{ item }">
      <span>{{ formatDate(item.effectiveDate) }}</span>
    </template>
    <template v-slot:[`item.change`]="{ item }">
      <observation-trends :observation-type="observationType" :change="item.change" />
    </template>
    <!--
    TO DO: item is not being used- do you have a plan for this @Matt ?
    <template v-slot:[`item.details`]="{ item }" v-if="observationType === 'sleep'">
      <span>TBD</span>
    </template> -->
    <template v-slot:[`item.mealCode`]="{ item }" v-if="observationType === 'blood-glucose'">
      {{$t(displayMealCode(item.observationsData))}}
    </template>
    <template v-slot:expanded-item="{ headers, item }">
      <td :colspan="headers.length" class="elevation-0 pa-0">
        <v-simple-table dense class="rounded-0">
          <template v-slot:default>
              <tbody>
                <tr v-for="itemDetail in getFilteredObservationsData(item)" :key="itemDetail.observationCode">
                  <td>{{ displayObservationDataDetail(itemDetail) }}</td>
                </tr>
              </tbody>
          </template>
        </v-simple-table>
      </td>
    </template>
  </v-data-table>
</template>

<script lang="ts">
import Vue, { PropType } from 'vue';
import { format, addSeconds } from 'date-fns';
import { cond, equals } from 'ramda';
import { mapState } from 'vuex';
import { PatientObservationDto, PatientObservationDataDto } from '../../IUGOCare-api';
import { kebabToCamelCase } from '../../common/text-helpers';
import ObservationTrends from './ObservationTrends.vue';

export default Vue.extend({
  name: 'ObservationsTable',
  components: { ObservationTrends },

  props: {
    observationType: String,
    observations: {
      type: Array as PropType<PatientObservationDto[]>,
    },
  },
  data() {
    return {
      isToggleAll: false as boolean,
      footerProps: {
        'items-per-page-text': '',
      },
      workoutSubitems: ['distance', 'steps', 'calories-burned', 'heart-rate'],
    };
  },
  computed: {
    ...mapState('observations', [ 'loading' ]),
    ...mapState('patients', ['currentPatient']),
    headers(): any[] {
      const i18n: string = cond<string, string>([
        [equals('body-temperature'),  () => 'temperatureLbl'],
        [equals('blood-glucose'),     () => 'bloodGlucoseLbl'],
        [equals('blood-pressure'),    () => 'bloodPressureLbl'],
        [equals('weight'),            () => 'weightLbl'],
        [equals('oxygen-saturation'), () => 'oxygenSaturationLbl'],
        [equals('respiratory-rate'),  () => 'respiratoryRateLbl'],
        [equals('heart-rate'),        () => 'heartRateLbl'],
        [equals('sleep'),             () => 'sleepDurationHeaderLabel'],
        [equals('workouts'),          () => 'workoutLbl'],
        [equals('a1c'),               () => 'A1c\xa0(%)'],
        [() => true,                  () => 'observation'],
      ])(this.observationType);

      const headerItems = [
        { text: this.$t(i18n), value: 'observation', sortable: false, align: 'start' },
        { text: this.$t('changeFromPrevious'), value: 'change', sortable: false },
        { text: this.$t('date'), value: 'effectiveDate', sortable: false },
      ];

      if (this.observationType === 'sleep') {
        headerItems.splice(0, 0, { text: '', value: 'data-table-expand', sortable: false, align: 'start' });
        headerItems.push({ text: this.$t('details'), value: 'details', sortable: false });
      }

      if (this.observationType === 'workouts') {
        headerItems.splice(0, 0, { text: '', value: 'data-table-expand', sortable: false, align: 'start' });
      }

      if (this.observationType === 'blood-pressure') {
        headerItems.splice(1, 1, { text: `${this.$t('changeFromPrevious')} ${this.$t('systolic')}/${this.$t('diastolic')}`, value: 'change', sortable: false });
      }

      if (this.observationType === 'blood-glucose') {
        headerItems.splice(1, 0, { text: `${this.$t('fastingQuestion')}`, value: 'mealCode', sortable: false });
      }

      return headerItems;
    },
    showExpand() {
      switch (this.observationType) {
        case 'sleep':
          return true;
        case 'workouts':
          return true;
        default:
          return false;
      }
    },
  },

  methods: {
    displayMealCode(data: PatientObservationDataDto[]): string {
      const mealCode = data.find((d) => d.observationCode.indexOf('meal-code') === 0);
      if (! mealCode) {
        return '';
      }
      return kebabToCamelCase(mealCode.observationCode);
    },
    displayObservationData(data: PatientObservationDataDto[]): string {
      if (this.observationType === 'blood-pressure') {
        const systolic = data.find((d) => d.observationCode === 'systolic')?.value || 0;
        const diastolic = data.find((d) => d.observationCode === 'diastolic')?.value || 0;
        const heartRate = data.find((d) => d.observationCode === 'heart-rate')?.value || 0;
        return `${systolic}/${diastolic} ${this.$t('heartRateAbr')}: ${heartRate}`;
      } else if (this.observationType === 'oxygen-saturation') {
        const oxygenSaturation = data.find((d) => d.observationCode === 'oxygen-saturation')?.value || 0;
        const heartRate = data.find((d) => d.observationCode === 'heart-rate')?.value || 0;
        return `${oxygenSaturation}% ${this.$t('heartRateAbr')}: ${heartRate}`;
      } else if (this.observationType === 'heart-rate') {
          const heartRate = data.find((d) => d.observationCode === 'heart-rate')?.value || 0;
          return `${heartRate} bpm`;
      } else if (this.observationType === 'sleep') {
        const totalSleepValue = data.find((d) => d.observationCode === 'total')?.value || 0;
        const totalSleepLabel = this.$t('total');
        return `${totalSleepLabel}: ${this.formatSecondsToHms(totalSleepValue) }`;
      } else if (this.observationType === 'workouts') {
        const duration = data.find((d) => d.observationCode === 'fitness-duration') ?.value || 0;
        return `${this.$t('duration')}: ${this.formatSecondsToHms(duration)}`;
      } else if (this.observationType === 'respiratory-rate') {
        const respiratoryRate = data.find((d: any) => d.observationCode === 'respiratory-rate')?.value || 0;
        return `${respiratoryRate} rpm`;
      } else if (this.observationType === 'blood-glucose') {
        const observationData = data.find((d: any) => d.observationCode === 'blood-glucose');
        const bloodGlucose = observationData?.value;
        const bloodGlucoseUnit = observationData?.unit;
        return `${bloodGlucose} ${bloodGlucoseUnit}`;
      } else  {
        if  (data.length > 0) {
          const datum = data[0];
          return `${datum.value} ${datum.unit}`;
        } else {
          return '';
        }
      }
    },
    formatDate(date: Date): string {
      const dateFormat = this.currentPatient?.dateFormat || 'MM/dd/yyyy';
      const timeFormat = (this.currentPatient?.timeFormat || '24 H') === '24 H' ? ' HH:mm' : ' hh:mm a';
      return date ? format(date, dateFormat + timeFormat) : '';
    },
    getFilteredObservationsData(item: PatientObservationDto): PatientObservationDataDto[] {
      let observationData: PatientObservationDto[];

      if (this.observationType === 'sleep') {
        observationData = item.observationsData.filter((o) => o.observationCode !== 'total');
      } else if (this.observationType === 'workouts') {
        observationData = item.observationsData.filter((o) => o.observationCode !== 'fitness-duration');
      } else {
        observationData = item.observationsData;
      }
      return observationData.filter((dto) => this.verifySubItem(dto));
    },
    displayObservationDataDetail(data: PatientObservationDataDto): string {
      let detailLabel = '';
      if (this.observationType === 'sleep') {
        switch (data.observationCode) {
          case 'rem':
            detailLabel = this.$t('rem').toString();
            break;
          case 'deep':
            detailLabel = this.$t('deep').toString();
            break;
          case 'light':
            detailLabel = this.$t('light').toString();
            break;
          case 'awake':
            detailLabel = this.$t('awake').toString();
            break;
        }
        return `${detailLabel}: ${this.formatSecondsToHms(data.value)}`;
      } else if (this.observationType === 'workouts') {
        switch (data.observationCode) {
          case 'distance':
            detailLabel = `${this.$t('distance')}: ${data.value} ${data.unit}`;
            break;
          case 'steps':
            detailLabel = `${this.$t('steps')}: ${data.value} ${this.$t(data.unit.toLowerCase())}`;
            break;
          case 'calories-burned':
            detailLabel = `${this.$t('caloriesBurned')}: ${data.value} ${data.unit}`;
            break;
          case 'heart-rate':
            detailLabel = `${this.$t('average')} ${this.$t('heartRate')}: ${data.value} ${data.unit}`;
            break;
        }
        return detailLabel;
      }
    },
    formatSecondsToHms(observationValue: number): string {
      return format(addSeconds(new Date(2020, 1, 1), observationValue), 'HH:mm:ss');
    },
    toggleAll(): void {
      this.isToggleAll = !this.isToggleAll;
      Object.keys(this.$refs).forEach((k) => {
        if (this.$refs[k]) {
          // @ts-ignore
          const element = this.$refs[k].$el;

          if ((this.isToggleAll && !element.attributes.isexpanded) ||
            (!this.isToggleAll && element.attributes.isexpanded)) {
            element.click();
          }
        }
      });
    },
    verifySubItem(data: PatientObservationDataDto): boolean {
      switch (this.observationType) {
        case 'sleep':
          return true;
        case 'workouts':
          return this.workoutSubitems.includes(data.observationCode);
        default:
          return true;
      }
    },
  },
});
</script>

<style lang="scss" scoped>
.v-data-table {
  width: 100%;
}
</style>
