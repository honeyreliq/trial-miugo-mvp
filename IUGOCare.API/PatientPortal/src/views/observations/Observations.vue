<template>
  <v-container id="observations" fluid class="fill-heightX" v-if="$auth.isAuthenticated">
    <v-row class="fill-height flex-column">

      <v-col class="flex-grow-0">
        <ObservationTypesButtonGroup
        v-if="observationCategory === 'vital-signs'"
          :observation-types="observationTypes"
          v-model="observationType"
          @input="updateObservationType"
        />
      </v-col>
           <v-col>
        <v-sheet class="fill-height pa-4">
 <v-row class="mb-4" id="date-pick-and-print-row">
        <v-col cols="auto">
          <DateRangePickAndPrint
            v-model="dateRange"
            @input="getPatientObservations"
          />
        </v-col>
        <v-spacer />
        <v-col cols="auto">
          <BaseButton color="primary">
            <fa-icon :icon="['fal', 'print']" class="fa-fw white--text" />
          </BaseButton>
        </v-col>
      </v-row>
      <v-row class="fill-height">
        <v-col cols="12" sm="12" md="5" class="d-flex" id="data-table-column">
          <ObservationDataTable
            :observationType="observationType"
            :observations="patientObservations"
          />
        </v-col>
        <v-col cols="12" sm="12" md="7" class="d-flex" id="chart-column">
          <ObservationChart :observationType="observationType" :observations="patientObservations" />
        </v-col>
      </v-row>
        </v-sheet>
      </v-col>
      </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import { mapMutations, mapState } from 'vuex';
import ObservationTypesButtonGroup from '../../components/observations/ObservationTypesButtonGroup.vue';
import DateRangePickAndPrint from "./components/DateRangePickAndPrint.vue";
//import ObservationTypesSelectList from "./components/ObservationTypesSelectList.vue";
import ObservationChart from "./components/ObservationChart.vue";
import ObservationDataTable from "./components/ObservationDataTable.vue";
//import ObservationExpandableDataTable from './components/ObservationExpandableDataTable.vue';
// @ts-ignore
import { assoc } from "ramda";
import { ObservationTypes } from "@/common/data/observation-constants";
// import AddObservationDialog from "./dialogs/AddObservationDialog.vue";

// fake data collection
import fakeObservationA1cData from "./data/fakeObservationA1cData.json";
import fakeObservationBloodGlucoseData from "./data/fakeObservationBloodGlucoseData.json";
import fakeObservationBloodPressureData from "./data/fakeObservationBloodPressureData.json";
import fakeObservationBodyTemperatureData from "./data/fakeObservationBodyTemperatureData.json";
import fakeObservationHeartRateData from "./data/fakeObservationHeartRateData.json";
import fakeObservationOxygenSaturationData from "./data/fakeObservationOxygenSaturationData.json";
import fakeObservationRespiratoryRateData from "./data/fakeObservationRespiratoryRateData.json";
import fakeObservationWeightData from "./data/fakeObservationWeightData.json";
import fakeObservationSleepData from "./data/fakeObservationSleepData.json";
import fakeObservationActivityData from "./data/fakeObservationActivityData.json";
import { PATIENT_CHART } from "@/store/mutation-types";

interface ObservationType {
  text: any;
  value: string;
  disabled?: boolean;
}

export default Vue.extend({
  name: "Observations",
  components: {
    DateRangePickAndPrint,
    // ObservationTypesSelectList,
    ObservationChart,
    ObservationDataTable,
    // AddObservationDialog,
    ObservationTypesButtonGroup,
  },
  data(): any {
    return {
      openAddObservation: false,
      patientObservations: fakeObservationBloodPressureData.observations,
      observationType: "blood-pressure",
      dateRange: ["2020-01-01", "2020-06-12"],
      appBarColor: {
        class: 'colorObservations',
        dark: true,
      },
    };
  },
  props: {
    observationCategory: String,
  },
  computed: {
    observationTypes(): ObservationType[] {
      const types: ObservationType[] = [
        { text: "bloodPressure", value: "blood-pressure" },
        { text: "bloodGlucose", value: "blood-glucose" },
        { text: "heartRate", value: "heart-rate" },
        { text: "oxygenSaturation", value: "oxygen-saturation" },
        { text: "respiratoryRate", value: "respiratory-rate" },
        { text: "bodyTemperature", value: "body-temperature" },
        { text: "weight", value: "weight" },
        { text: "a1c", value: "a1c" },
        // { text: "activity", value: "activity" },
        // { text: "sleep", value: "sleep" },
      ];

      return types.map((c) => assoc("text", this.$t(c.text), c));
    },
    ...mapState('demo', ['miugo'])
  },
  watch: {
    dateRange() {
      this.getPatientObservations();
    },
    observationType() {
      this.getPatientObservations();
    },
  },
  // beforeMount() {
  //   this.$root.$emit('setAppBarColor', {
  //     class: 'colorObservations',
  //     dark: true,
  //     light: false,
  //   });
  // },
  methods: {
    ...mapMutations("patientChart", {
      setSearchEnabled: PATIENT_CHART.SET_SEARCH_ENABLED,
    }),
    getPatientObservations(): void {
      switch (this.observationType) {
        case ObservationTypes.A1C:
          this.patientObservations = fakeObservationA1cData.observations;
          break;
        case ObservationTypes.BLOOD_GLUCOSE:
          this.patientObservations =
            fakeObservationBloodGlucoseData.observations;
          break;
        case ObservationTypes.BLOOD_PRESSURE:
          this.patientObservations =
            fakeObservationBloodPressureData.observations;
          break;
        case ObservationTypes.BODY_TEMPERATURE:
          this.patientObservations =
            fakeObservationBodyTemperatureData.observations;
          break;
        case ObservationTypes.HEART_RATE:
          this.patientObservations = fakeObservationHeartRateData.observations;
          break;
        case ObservationTypes.OXYGEN_SATURATION:
          this.patientObservations =
            fakeObservationOxygenSaturationData.observations;
          break;
        case ObservationTypes.RESPIRATORY_RATE:
          this.patientObservations =
            fakeObservationRespiratoryRateData.observations;
          break;
        case ObservationTypes.WEIGHT:
          this.patientObservations = fakeObservationWeightData.observations;
          break;
        case ObservationTypes.ACTIVITY:
          this.patientObservations = fakeObservationActivityData.observations;
          break;
        case ObservationTypes.SLEEP:
          this.patientObservations = fakeObservationSleepData.observations;
          break;
      }
    },
    updateObservationType(newValue: string): void {
      this.observationType = newValue;
    },
    ...mapMutations('demo', {
      setAppStarted: 'setAppStarted'
    })
  },
  mounted() {
      setTimeout(() => {
        this.setAppStarted(true);
        this.$root.$emit('setAppBarColor', this.appBarColor);
      }, (this.miugo.appStarted) ? 0 : 200);
    },
    destroyed(): void {
      this.appBarColor.class = '';
      this.appBarColor.light = false;
      this.$root.$emit('setAppBarColor', this.appBarColor);
    },
});
</script>
