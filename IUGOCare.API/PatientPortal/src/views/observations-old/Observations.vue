<template>
  <v-container id="observations" fluid class="fill-height" v-if="$auth.isAuthenticated">
    <v-row class="fill-height flex-column">

      <v-col class="flex-grow-0">
        <observation-types-button-group
          v-if="observationCategory === 'vital-signs'"
          :observation-types="observationTypes"
          v-model="observationType"
          @input="updateObservationType"
        />
      </v-col>

      <v-col>
        <v-sheet class="fill-height pa-4">
          <v-row class="fill-height flex-column" v-if="currentPatient">
            <v-col class="flex-grow-0">
              <date-range-picker
                v-model="dateRange"
                @input="getPatientObservations"
                @print="print"
              />
            </v-col>

            <v-col class="flex-grow-1">
              <v-row class="fill-height">
                <v-col cols="12" sm="12" md="4" class="d-flex">
                  <observations-table :observation-type="observationType" :observations="patientObservations" />
                </v-col>
                <v-col cols="12" sm="12" md="8" class="d-flex">
                  <ObservationsCharts :observations="patientObservations" :observationType="observationType" @sendChartInformation="extractChartInformation"></ObservationsCharts>
                </v-col>
              </v-row>
            </v-col>
          </v-row>
        </v-sheet>
      </v-col>
    </v-row>

    <v-container id="observations-print-container">
      <header id="pageHeader">
        <v-row>
          <v-col cols="4">
            <v-img :src="require(`@/assets/iUGO_Care.svg`)" class="logo-img" style="max-height: 80pt" />
          </v-col>
          <v-col cols="8"><h1>{{printReportTitle}}</h1></v-col>
        </v-row>
      </header>

      <table style="width: 100%">
        <thead>
          <tr>
            <td>
              <!--place holder for the fixed-position header-->
              <div class="page-header-space"></div>
            </td>
          </tr>
        </thead>

        <tbody>
          <tr>
            <td>
              <div class="page">

                <!-- Patient info -->
                <v-row
                  v-for="(info, $index) in printHeaderInformation"
                  v-bind:key="info[0]"
                  :class="{'gray': $index % 2 === 1}"
                >
                  <v-col><strong>{{info[0]}}</strong></v-col>
                  <v-col>{{info[1]}}</v-col>
                </v-row>

                <!-- Observations -->
                <v-row class="print-heading">
                  <v-col
                    v-for="heading in getPrintObservationTableHeaders()"
                    :key="heading"
                  >
                    <strong>{{heading}}</strong>
                  </v-col>
                </v-row>
                <v-row v-for="(value, $index) in getObservationsForPrint()" :key="$index" :class="{'gray': $index % 2 === 1}">
                  <v-col>{{value.observation}}</v-col>
                  <v-col v-if="observationType === 'blood-glucose'">{{value.mealCode}}</v-col>
                  <v-col>
                    <observation-trends :observation-type="observationType" :change="value.change" />
                  </v-col>
                  <v-col>{{value.date}}</v-col>
                </v-row>

                <!-- Chart -->
                <v-row>
                  <v-col v-for="header in chartInformation.ChartHeaders" :key="header.key">
                    <h2>{{header.valueString}}</h2>
                    {{header.key}}
                  </v-col>
                </v-row>

                <v-row><v-col><img v-if="chartDataUrl" :src="chartDataUrl" style="max-width: 100%" /></v-col></v-row>
                <v-row><v-col><img v-if="bottomChartDataUrl" :src="bottomChartDataUrl" style="max-width: 100%" /></v-col></v-row>

              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </v-container>
  </v-container>
</template>

<script lang="ts">
  import { mapState } from 'vuex';
  import { assoc, cond, equals } from 'ramda';
  import Vue from 'vue';
  import DateRangePicker from '../../components/form-elements/DateRangePicker.vue';
  import ObservationsTable from '../../components/observations/ObservationsTable.vue';
  import ObservationTypesButtonGroup from '../../components/observations/ObservationTypesButtonGroup.vue';
  import ObservationsCharts from '../../components/observations/ObservationsCharts.vue';
  import ObservationTrends from '../../components/observations/ObservationTrends.vue';
  import { addSeconds, format } from 'date-fns';
  import { kebabToCamelCase } from '../../common/text-helpers';
  import { shouldDisplayTour, OBSERVATIONS_TOUR } from '../../common/vue-tour';
  import { IChartInformation } from '@/shared/interfaces';

  interface ObservationType {
    text: any;
    value: string;
    disabled?: boolean;
  }

  export default Vue.extend({
    name: 'DashboardObservations',
    components: {
      DateRangePicker, ObservationsTable, ObservationTypesButtonGroup,
      ObservationsCharts, ObservationTrends },
    data() {
      return {
        isComponentMounted: false,
        dateRange: ['2020-01-01', '2020-06-12'],
        observationType: 'blood-pressure',
        chartInformation: {} as IChartInformation,
        chartDataUrl: null,
        bottomChartDataUrl: null,
        appBarColor: {
          class: 'colorObservations',
          dark: true,
        },
      };
    },
    props: {
      observationCategory: String,
    },
    methods: {
      getPatientObservations() {
        if (
          this.observationType &&
          this.observationType.length > 0 &&
          this.isComponentMounted
        ) {
          const observationTypeCodes = [];
          observationTypeCodes.push(this.observationType);

          if (this.observationType === 'heart-rate') {
            observationTypeCodes.push('blood-pressure');
            observationTypeCodes.push('oxygen-saturation');
          }

          this.$store.dispatch('observations/fetchPatientObservations', {
            observationCodes: observationTypeCodes,
            effectiveDateStart: this.parseDate(this.dateRange[0]),
            effectiveDateEnd: new Date(this.parseDate(this.dateRange[1])),
          }).then(() => {
            if (this.patientObservations.length === 0) {
              this.chartInformation = {
                ChartsImages: [],
                ChartHeaders: [],
              };
            }
          });
        }
      },
      getPatientTargetRanges() {
        this.$store.dispatch('targetRanges/fetchPatientTargetRanges');
      },
      getPrintObservationsInformation(): any[] {
        const observationsInformation = [];
        const [yearStart, monthStart, dayStart] = this.dateRange[0].split('-');
        const [yearEnd, monthEnd, dayEnd] = this.dateRange[1].split('-');
        const startDate = format(new Date(parseInt(yearStart, 10),
                                          parseInt(monthStart, 10) - 1,
                                          parseInt(dayStart, 10)), this.currentPatient?.dateFormat || 'MM/dd/yyyy');
        const endDate = format(new Date(parseInt(yearEnd, 10),
                                        parseInt(monthEnd, 10) - 1,
                                        parseInt(dayEnd, 10)), this.currentPatient?.dateFormat || 'MM/dd/yyyy');

        const targetRangesInformation = this.getTargetRangesByObservationType();

        observationsInformation.push([`${this.$t('dateAccessedLbl')}:`, `${format(new Date(), this.currentPatient?.dateFormat || 'MM/dd/yyyy')}`]);
        observationsInformation.push([`${this.$t('observationsFromLbl')}`, `${startDate} ${this.$t('observationsToLbl')} ${endDate}`]);

        if (targetRangesInformation.length > 0) {
          observationsInformation.push([`${this.$t('referenceRangesLbl')}:`, '']);
        } else {
          observationsInformation.push([`${this.$t('referenceRangesLbl')}:`, ` ${this.$t('referenceRangesEmpty')}`]);
        }

        return observationsInformation;
      },
      getTargetRangesByObservationType(): string[][] {
        const targetRange = [];
        let criticalHigh = '';
        let atRiskHigh = '';
        let target = '';
        let atRiskLow = '';
        let criticalLow = '';

        switch (this.observationType) {
          case 'blood-pressure': {
            const systolicRanges = this.targetRanges.find((t: any) => t.observationCode === 'systolic');
            const diastolicRanges = this.targetRanges.find((t: any) => t.observationCode === 'diastolic');

            if (systolicRanges && diastolicRanges) {
              criticalHigh = `>${systolicRanges.criticalHigh}/${diastolicRanges.criticalHigh}`;
              atRiskHigh = `${systolicRanges.atRiskHigh}/${diastolicRanges.atRiskHigh} - ${systolicRanges.criticalHigh - 1}/${diastolicRanges.criticalHigh - 1}`;
              target = `${systolicRanges.atRiskLow + 1}/${diastolicRanges.atRiskLow + 1} - ${systolicRanges.atRiskHigh - 1}/${diastolicRanges.atRiskHigh - 1}`;
              atRiskLow = `${systolicRanges.criticalLow}/${diastolicRanges.criticalLow} - ${systolicRanges.atRiskLow}/${diastolicRanges.atRiskLow}`;
              criticalLow = `<${systolicRanges.criticalLow}/${diastolicRanges.criticalLow}`;

              targetRange.push([criticalLow, atRiskLow, target, atRiskHigh, criticalHigh]);
            }
            break;
          }
          case 'oxygen-saturation': {
            const oxygenSaturationRanges = this.targetRanges.find((t: any) =>
              t.observationCode === 'oxygen-saturation');

            if (oxygenSaturationRanges) {
              criticalHigh = `>${oxygenSaturationRanges.criticalHigh}`;
              atRiskHigh = `${oxygenSaturationRanges.atRiskHigh} - ${oxygenSaturationRanges.criticalHigh - 1}`;
              target = `${oxygenSaturationRanges.atRiskLow + 1} - ${oxygenSaturationRanges.atRiskHigh - 1}`;
              atRiskLow = `${oxygenSaturationRanges.criticalLow} - ${oxygenSaturationRanges.atRiskLow}`;
              criticalLow = `<${oxygenSaturationRanges.criticalLow}`;

              targetRange.push([criticalLow, atRiskLow, target, atRiskHigh, criticalHigh]);
            }
            break;
          }
          default: {
            const ranges = this.targetRanges.find((t: any) =>
              t.observationCode === this.observationType);

            if (ranges) {
              criticalHigh = `>${ranges.criticalHigh}`;
              atRiskHigh = `${ranges.atRiskHigh} - ${ranges.criticalHigh - 1}`;
              target = `${ranges.atRiskLow + 1} - ${ranges.atRiskHigh - 1}`;
              atRiskLow = `${ranges.criticalLow} - ${ranges.atRiskLow}`;
              criticalLow = `<${ranges.criticalLow}`;

              targetRange.push([criticalLow, atRiskLow, target, atRiskHigh, criticalHigh]);
            }
            break;
          }
        }

        return targetRange;
      },
      parseDate(date: string) {
        if (!date) {
          return null;
        }
        return new Date(date);
      },
      updateObservationTypeByCategory() {
        switch (this.observationCategory) {
          case 'sleep':
            this.observationType = 'sleep';
            break;
          case 'activity':
            this.observationType = 'workouts';
            break;
          default:
            this.observationType = 'blood-pressure';
            break;
        }
      },
      print() {
        const chart = (document.getElementById('myChart') as HTMLCanvasElement);
        this.chartDataUrl = chart?.toDataURL();
        const bottomChart = document.getElementById('myBottomChart') as HTMLCanvasElement;
        this.bottomChartDataUrl = bottomChart?.toDataURL();

        setTimeout(() => window.print(), 100);
      },
      patientInformationValues() {
        const patientInfomationData = [];

        // Patient personal information
        if (! this.currentPatient) {
          return [];
        }

        const currentPatient = this.currentPatient;
        const dateFormat = currentPatient.dateFormat ? currentPatient.dateFormat : 'MM/dd/yyyy';
        const patientName = `${currentPatient.givenName} ${currentPatient.familyName}`;
        const dateOfBirth = currentPatient.birthDate ?  format(currentPatient.birthDate, dateFormat) : '';
        const medicaidNumber = currentPatient.medicaidNumber;
        const medicareNumber = currentPatient.medicareNumber;
        const medicalRecordNumber = currentPatient.medicalRecordNumber;
        const insuranceNumber = currentPatient.insuranceNumber;

        if (patientName) {
          patientInfomationData.push([this.$t('name').toString(), patientName]);
        }

        if (dateOfBirth) {
          patientInfomationData.push([this.$t('dateOfBirth').toString(), dateOfBirth]);
        }

        if (medicalRecordNumber) {
          patientInfomationData.push([this.$t('medicalRecordNumber').toString(), medicalRecordNumber]);
        }

        if (medicaidNumber) {
          patientInfomationData.push([this.$t('medicaidNumber').toString(), medicaidNumber]);
        }

        if (medicareNumber) {
          patientInfomationData.push([this.$t('medicareNumber').toString(), medicareNumber]);
        }

        if (insuranceNumber) {
          patientInfomationData.push([this.$t('insuranceNumber').toString(), insuranceNumber]);
        }

        return patientInfomationData;
      },
      getObservationsForPrint() {
        switch (this.observationCategory) {
          case 'sleep':
            return this.addSleepsObservationTable();
          case 'activity':
            return this.addWorkoutsObservationTable();
          default:
            return this.addVitalSignsObservationTable();
        }
      },
      getPrintObservationTableHeaders() {
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
          [() => true,                  () => 'observation'],
        ])(this.observationType);

        let changeHeader = this.$t('changeFromPrevious');

        if (this.observationType === 'blood-pressure') {
          changeHeader = `${changeHeader} ${this.$t('systolic')}/${this.$t('diastolic')}`;
        }

        if (this.observationType === 'blood-glucose') {
          return [
            this.$t(i18n),
            this.$t('fastingQuestion'),
            changeHeader,
            this.$t('date'),
          ];
        } else {
          return [
            this.$t(i18n),
            changeHeader,
            this.$t('date'),
          ];
        }
      },
      addVitalSignsObservationTable() {
        const observationValues: any[] = [];

        this.patientObservations.forEach((x: any) => {
          let observationValue = '';

          if (this.observationType === 'blood-pressure') {
            const systolic = x.observationsData.find((d: any) => d.observationCode === 'systolic')?.value || 0;
            const diastolic = x.observationsData.find((d: any) => d.observationCode === 'diastolic')?.value || 0;
            const heartRate = x.observationsData.find((d: any) => d.observationCode === 'heart-rate')?.value || 0;
            observationValue = `${systolic}/${diastolic} ${this.$t('heartRateAbr')}: ${heartRate}`;
          } else if (this.observationType === 'oxygen-saturation') {
            const oxygenSaturation = x.observationsData.find(
              (d: any) => d.observationCode === 'oxygen-saturation')?.value || 0;
            const heartRate = x.observationsData.find((d: any) => d.observationCode === 'heart-rate')?.value || 0;
            observationValue = `${oxygenSaturation}% ${this.$t('heartRateAbr')}: ${heartRate}`;
          } else if (this.observationType === 'heart-rate') {
            const heartRate = x.observationsData.find((d: any) => d.observationCode === 'heart-rate')?.value || 0;
            observationValue = `${heartRate} bpm`;
          } else if (x.observationCode === 'respiratory-rate') {
            const respiratoryRate = x.observationsData.
                              find((d: any) => d.observationCode === 'respiratory-rate')?.value || 0;
            observationValue = `${respiratoryRate}`;
          } else  {
            if  (x.observationsData.length > 0) {
              const datum = x.observationsData[0];
              observationValue = `${datum.value} ${datum.unit}`;
            }
          }

          if (this.observationType === 'blood-glucose') {
            const bloodGlucose = x.observationsData.
                              find((d: any) => d.observationCode === 'blood-glucose')?.value || 0;
            observationValue = `${bloodGlucose} mg/dL`;
            const mealCode = x.observationsData.find((d: any) => d.observationCode.indexOf('meal-code-') === 0);

            observationValues.push(
            {
              observation: observationValue,
              mealCode: mealCode ? this.$t(kebabToCamelCase(mealCode.observationCode)) : '',
              change: x.change,
                date: x.effectiveDate ? format(x.effectiveDate, this.currentPatient?.dateFormat || 'MM/dd/yyyy') : '',
            });
          } else {
            observationValues.push(
            {
              observation: observationValue,
              change: x.change,
                date: x.effectiveDate ? format(x.effectiveDate, this.currentPatient?.dateFormat || 'MM/dd/yyyy') : '',
            });
          }
        });

        return observationValues;
      },
      addSleepsObservationTable() {
        const observationValues: any[] = [];

        this.patientObservations.forEach((x: any) => {
          const totalSleepValue = x.observationsData.find((d: any) => d.observationCode === 'total')?.value || 0;
          const totalSleepLabel = this.$t('total');
          let observationValue = `${totalSleepLabel}: ${format(addSeconds(new Date(2020, 1, 1), totalSleepValue), 'HH:mm:ss')}`;

          x.observationsData.filter((ob: any ) => ob.observationCode !== 'total').forEach((data: any) => {
            let detailLabel = '';
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

            if (detailLabel) {
              observationValue = `${observationValue}/${detailLabel}: ${format(addSeconds(new Date(2020, 1, 1), data.value), 'HH:mm:ss')}`;
            }
          });

          observationValues.push(
            {
              observation: observationValue,
              change: x.change,
              date: x.effectiveDate ? format(x.effectiveDate, this.currentPatient?.dateFormat || 'MM/dd/yyyy') : '',
            });
        });

        return observationValues;
      },
      addWorkoutsObservationTable() {
        const observationValues: any[] = [];

        this.patientObservations.forEach((x: any) => {
          const durationValue = x.observationsData.
                                    find((d: any) => d.observationCode === 'fitness-duration')?.value || 0;
          const durationLabel = this.$t('duration');
          let observationValue = `${durationLabel}: ${format(addSeconds(new Date(2020, 1, 1), durationValue), 'HH:mm:ss')}`;

          x.observationsData.filter((ob: any ) => ob.observationCode !== 'fitness-duration').forEach((data: any) => {
            let detailLabel = '';
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
                detailLabel = `${this.$t('heartRate')}: ${data.value} ${data.unit}`;
                break;
            }

            if (detailLabel) {
              observationValue = `${observationValue}/${detailLabel}`;
            }
          });

          observationValues.push(
            {
              observation: observationValue,
              change: x.change,
              date: x.effectiveDate ? format(x.effectiveDate, this.currentPatient?.dateFormat || 'MM/dd/yyyy') : '',
            });
        });

        return observationValues;
      },
      extractChartInformation(charts: IChartInformation): void {
        this.chartInformation = charts;
      },
      updateObservationType(newValue: string): void {
        this.observationType = newValue;
      },
    },
    computed: {
      ...mapState('patients', ['currentPatient']),
      ...mapState('observations', ['patientObservations']),
      ...mapState('targetRanges', ['targetRanges']),
      observationTypes(): ObservationType[] {
        // TODO -> Filter these by the selected Observation Category (ie, Activity, Sleep, Vital Signs)

        const types: ObservationType[] = [
          { text: 'bloodPressure', value: 'blood-pressure' },
          { text: 'bloodGlucose', value: 'blood-glucose' },
          { text: 'heartRate', value: 'heart-rate'},
          { text: 'oxygenSaturation', value: 'oxygen-saturation' },
          { text: 'respiratoryRate', value: 'respiratory-rate' },
          { text: 'bodyTemperature', value: 'body-temperature' },
          { text: 'weight', value: 'weight' },
          { text: 'a1c', value: 'a1c' },
        ];

        return types.map((c) => assoc('text', this.$t(c.text), c));
      },
      printReportTitle(): string {
        let title = '';

        switch (this.observationCategory) {
          case 'sleep':
            title = this.$t('sleep').toString();
            break;
          case 'activity':
            title = this.$t('workouts').toString();
            break;
          default: {
            const type = this.observationTypes.find((x) => x.value === this.observationType);
            title = type ? this.$t(type.text).toString() : '';
            break;
          }
        }

        return title;
      },
      printHeaderInformation(): any[] {
        const printHeaderInformation = this.patientInformationValues();

        // Observations Information
        const observationsInformation = this.getPrintObservationsInformation();
        observationsInformation.forEach((info) => {
          printHeaderInformation.push(info);
        });

        return printHeaderInformation;
      },
    },
    watch: {
      dateRange() {
        this.getPatientObservations();
      },
      observationType() {
        this.getPatientObservations();
      },
      observationCategory() {
        this.updateObservationTypeByCategory();
      },
    },
    async created() {
      this.$store.dispatch('patients/fetchCurrentPatient')
        .then(() => this.updateObservationTypeByCategory());
      this.getPatientTargetRanges();
    },
    async mounted() {
      this.$root.$emit('setAppBarColor', this.appBarColor);
      this.isComponentMounted = true;
      this.$root.$on('observationCreated', this.getPatientObservations);
      const shouldStartTour: boolean = await shouldDisplayTour(OBSERVATIONS_TOUR);
      if (shouldStartTour) {
        this.$tours.observationsTour.start();
      }
    },
    destroyed(): void {
      this.appBarColor.class = '';
      this.appBarColor.dark = false;
      this.$root.$emit('setAppBarColor', this.appBarColor);
    },
  });
</script>

<style lang="scss">
@media print {
  @page {
    size: letter portrait;
    width: 8.5in;
    height: 11in;
    margin: 2cm;
    padding-top: 10cm;

    @top-left {
      content: element(pageHeader);
    }
  }

  body *, #patientportal {
    visibility: hidden;
  }
  #observations-print-container, #observations-print-container * {
    -webkit-print-color-adjust: exact !important;
    visibility: visible;
  }
  #observations-print-container {
    display: block !important;
    position: absolute;
    top: 15pt;
  }

  #pageHeader {
    position: fixed;
    top: 0;
  }

  #pageHeader , .page-header-space {
    height: 80pt;
  }

  .page {
    page-break-after: always;
  }

  thead {display: table-header-group;}

  .v-main {
    padding: 20pt!important;
  }
}

#pageHeader {
  width: 100%;
  height: 80pt;
  h1 {
    font-size: 3rem;
    font-weight: 300;
    letter-spacing: -0.015625em;
    line-height: 80pt;
  }
}

#observations-print-container {
  display: none;
  color: #111;

  .row {
    padding: 0.5rem;
    page-break-inside: avoid;
  }
}

.gray {
  background-color: #e8e8e8;
}

.print-heading {
  color: white;
  background-color: #007EC7;
  margin-top: 2rem;
}
</style>
