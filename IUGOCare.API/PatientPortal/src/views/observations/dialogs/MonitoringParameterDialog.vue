<template>
    <v-dialog v-model="openMonitoringParameterDialog" persistent fixed max-width="900px">
      <template v-slot:activator="{ on, attrs }">
      <BaseButton
        color="primary"
        @click="openMonitoringParameterDialog = true"
        v-bind="attrs"
        v-on="on"
      >
        {{ $t('monitoringParameters') }}
      </BaseButton>
      </template>
      <v-card id="monitoring-parameter-dialog">
        <BaseToolbar :title="$t('monitoringParameters')" class="colorUsers" dark>
          <v-spacer></v-spacer>
          <BaseButton icon dark @click="openMonitoringParameterDialog = false">
            <fa-icon :icon="['fal', 'times']" class="fa-fw" />
          </BaseButton>
        </BaseToolbar>
        <v-card-text>
        <v-row class="fill-height">
          <!-- <v-col cols="12" md="4">
          <v-card class="flex elevation-0">
            <BaseDataTable
              :headers="headers"
              :items="rows"
              :sort-desc="true"
              activeRow
              disable-pagination
              @handle-row-click="handleRowClick"
            > -->
            <!-- Columns -->
            <!-- <template v-slot:[`item.name`]="{ item }">
              <span class="d-flex my-2">{{ $t(item.name) }}</span>
            </template>
            <template v-slot:[`item.enable`]="{ item }">
              <v-switch
                dense
                class="ma-0 mb-n4"
                v-model="item.enable"
                @click.stop="toggleMonitoringConfiguration(item.id)"
              />
            </template>
            </BaseDataTable>
          </v-card>
          </v-col> -->
          <v-col cols="12" >
          <v-card class="flex overflow-y-auto elevation-0">
            <span class="d-flex justify-end align-center pa-0 mb-4">
              <v-switch
                v-model="focus"
                inset
                small
                :label="$t('focusSwitch')"
                class="mt-0 mr-4"
                hide-details="auto"
                :disabled="this.selection === null"
              ></v-switch>
              <BaseButton
                color="primary"
                dark
                small
                @click="
                expanded = !expanded;
                expand();
                "
              >
                {{ expanded ? $t('collapse') : $t('expand') }}
              </BaseButton>
              </span>
              <v-expansion-panels v-model="panel" multiple flat tile class="mb-4">
              <v-expansion-panel
                v-for="item in resultItems"
                v-show="(filter === item.id && focus) || !focus"
                :key="item.id"
                :class="[
                'mb-4 mt-0 rounded',
                filter === item.id
                    ? $vuetify.theme.dark
                    ? 'darkGrey lighten-1'
                    : 'lighterGrey darken-1'
                    : $vuetify.theme.dark
                    ? 'darkGrey'
                    : 'lightGrey',
                !focus || 'no-animation',
                ]"
                :expanded="expanded"
              >
                  <v-expansion-panel-header class="text-h6 font-weight-bold">
                  <template v-slot:actions>
                      <v-switch
                        v-model="enableSwitch"
                        class="ma-0 ml-2 align-self-center"
                        hide-details
                        dense
                      />
                      <v-btn small icon class="pointer">
                      <fa-icon
                          :icon="['far', 'chevron-down']"
                          class="fa-fw fa-list"
                      />
                      </v-btn> </template
                  >{{ item.title }}</v-expansion-panel-header
                  >
                  <v-expansion-panel-content>
                  <BloodPressureTable
                      v-if="!isLoading && item.observationType === 'blood-pressure'"
                      :monitoringParameters="bloodPressureParameters"
                  />
                  <BloodGlucoseTable
                      v-if="!isLoading && item.observationType === 'blood-glucose'"
                      :monitoringParameters="bloodGlucoseParameters"
                  />
                  <TemperatureTable
                      v-if="!isLoading && item.observationType === 'body-temperature'"
                      :monitoringParameter="temperatureParameters"
                  />
                  </v-expansion-panel-content>
              </v-expansion-panel>
              </v-expansion-panels>
          </v-card>
          </v-col>
        </v-row>
        </v-card-text>
        <v-card-actions class="pt-0">
          <v-spacer></v-spacer>
          <BaseButton text @click="openMonitoringParameterDialog = false">
            {{ $t('cancel') }}
          </BaseButton>
        </v-card-actions>
      </v-card>
    </v-dialog>
</template>

<script lang="ts">
import {
  MonitoringParameter,
  PatientMonitoringConfiguration,
} from '@/store/patient-chart/interfaces';
import Vue from 'vue';
import { mapActions, mapState, mapMutations } from 'vuex';
import { kebabToCamelCase } from '@/common/utils/text-helpers';
import BloodPressureTable from './components/BloodPressureTable.vue';
import BloodGlucoseTable from './components/BloodGlucoseTable.vue';
import TemperatureTable from './components/TemperatureTable.vue';
import { ObservationTypes } from '@/common/data/observation-constants';
import { PATIENT_CHART } from '@/store/mutation-types';

export default Vue.extend({
  name: 'MonitoringParameterDialog',
  components: {
    BloodPressureTable,
    BloodGlucoseTable,
    TemperatureTable,
  },
  data(): any {
    return {
      enableSwitch: true,
      openMonitoringParameterDialog: false,
      focus: false,
      expanded: true,
      filter: null,
      selection: null,
      panel: [0, 1, 2],
      isLoading: false,
    };
  },
  computed: {
    ...mapState('patientChart', [
      'monitoringConfigurations',
      'monitoringParameters',
    ]),
    rows(): Array<PatientMonitoringConfiguration> {
      return this.monitoringConfigurations.map(
        (configuration: PatientMonitoringConfiguration) => {
          return {
            ...configuration,
            observationType: this.$t(
              kebabToCamelCase(configuration.observationType)
            ),
          };
        }
      );
    },
    resultItems(): any[] {
      return this.monitoringConfigurations
        .filter((mp: PatientMonitoringConfiguration) => mp.enable)
        .map((config: PatientMonitoringConfiguration) => {
          return {
            id: config.id,
            title: this.$t(kebabToCamelCase(config.observationType)),
            observationType: config.observationType,
          };
        });
    },
    headers(): any {
      return [
        {
          text: `${this.$t('observationType')}`,
          value: 'observationType',
          class: 'text-subtitle-2',
          sortable: false,
          width: '90%',
        },
        {
          text: `${this.$t('enable')}`,
          value: 'enable',
          class: 'text-subtitle-2',
          sortable: false,
          width: '10%',
        },
      ];
    },
    bloodPressureParameters(): Array<MonitoringParameter> {
      return this.monitoringParameters.filter((mp: MonitoringParameter) =>
        mp.observationDataType.includes(ObservationTypes.BLOOD_PRESSURE)
      );
    },
    bloodGlucoseParameters(): Array<MonitoringParameter> {
      return this.monitoringParameters.filter((mp: MonitoringParameter) =>
        mp.observationDataType.includes(ObservationTypes.BLOOD_GLUCOSE)
      );
    },
    temperatureParameters(): MonitoringParameter {
      return this.monitoringParameters.find(
        (mp: MonitoringParameter) =>
          mp.observationDataType === ObservationTypes.BODY_TEMPERATURE
      );
    },
  },
  watch: {
    selection: function () {
      if (typeof this.selection === 'undefined') {
        this.resetSelection();
        this.expand();
      }
    },
    panel: function () {
      if (this.panel.length === 0) {
        this.expanded = false;
        this.resetSelection();
      } else {
        this.expanded = true;
      }
    },
  },
  async created(): Promise<void> {
    this.isLoading = true;
    await this.fetchMonitoringConfigurations();
    await this.fetchMonitoringParameters();
    this.isLoading = false;
  },
  mounted(): void {
    this.setSearchEnabled(false);
  },
  methods: {
    ...mapMutations('patientChart', {
      setSearchEnabled: PATIENT_CHART.SET_SEARCH_ENABLED,
    }),
    ...mapActions('patientChart', {
      fetchMonitoringConfigurations: 'fetchMonitoringConfigurations',
      toggleMonitoringConfiguration: 'toggleMonitoringConfiguration',
      fetchMonitoringParameters: 'fetchMonitoringParameters',
    }),
    handleRowClick(event: any) {
      // TODO: this was an early attemp to address this bug: https://reliqhealth.atlassian.net/browse/ICV5-197
      // which will likely be removed as part of the fix for that bug.
      const bloodPressureConfig = this.monitoringConfigurations.find(
        (mc: PatientMonitoringConfiguration) =>
          mc.observationType == ObservationTypes.BLOOD_PRESSURE
      );
      const bloodGlucoseConfig = this.monitoringConfigurations.find(
        (mc: PatientMonitoringConfiguration) =>
          mc.observationType == ObservationTypes.BLOOD_GLUCOSE
      );
      const temperatureConfig = this.monitoringConfigurations.find(
        (mc: PatientMonitoringConfiguration) =>
          mc.observationType == ObservationTypes.BODY_TEMPERATURE
      );

      switch (parseInt(event.id)) {
        case parseInt(bloodPressureConfig.id):
          this.panel = [0];
          break;
        case parseInt(bloodGlucoseConfig.id):
          this.panel = [1];
          break;
        case parseInt(temperatureConfig.id):
          this.panel = [2];
          break;
      }

      this.filter = event.id;
      this.selection = event.id;
    },
    resetSelection() {
      this.filter = null;
      this.selection = null;
      this.focus = false;
    },
    expand() {
      if (this.expanded) {
        this.panel = [...Array(this.resultItems.length).keys()].map(
          (k, i) => i
        );
        this.focus = false;
      } else {
        this.panel = [];
        this.focus = false;
        this.resetSelection();
      }
    },
  },
});
</script>
<style lang="scss" scoped>
.no-animation .v-expansion-panel-content {
  transition: 0ms !important;
}
</style>
