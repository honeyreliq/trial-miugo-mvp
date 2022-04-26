<template>
  <div>
    <v-dialog :value="value" persistent fixed max-width="600px" :eager="true">
      <v-card>
        <BaseToolbar :title="$t('addObservation')" class="colorObservations">
        </BaseToolbar>
        <v-card-text>
          <v-container>
            <v-form ref="observationForm" :disabled="disabled">
              <v-row>
                <v-col cols="12">
                  <v-select :items="observationTypes"
                            :label="$t('observationType')"
                            v-model="observationType"
                            :disabled="disabled"
                            :loading="loadingObservationTypes"
                            hide-details="auto"
                            :rules="observationTypeRules()"></v-select>
                </v-col>
              </v-row>
              <blood-pressure-observation v-if="observationType === 'blood-pressure'" :disabledFields="disabled"></blood-pressure-observation>
              <oxygen-saturation-observation v-else-if="observationType === 'oxygen-saturation'" :disabledFields="disabled"></oxygen-saturation-observation>
              <respiratory-rate-observation v-else-if="observationType === 'respiratory-rate'" :disabledFields="disabled"></respiratory-rate-observation>
              <body-temperature-observation v-else-if="observationType === 'body-temperature'" :disabledFields="disabled"></body-temperature-observation>
              <blood-glucose-observation v-else-if="observationType === 'blood-glucose'" :disabledFields="disabled"></blood-glucose-observation>
              <heart-rate-observation v-else-if="observationType === 'heart-rate'" :disabledFields="disabled"></heart-rate-observation>
              <weight-observation v-else-if="observationType === 'weight'" :disabledFields="disabled"></weight-observation>
              <a1c-observation v-else-if="observationType === 'a1c'" :disabledFields="disabled"></a1c-observation>
              <v-row>
                <v-col cols="12">
                  <v-menu v-model="effectiveDateMenu"
                          :close-on-content-click="false"
                          transition="scale-transition"
                          offset-y
                          :nudge-right="40"
                          max-width="290px"
                          min-width="290px">
                    <template v-slot:activator="{ on, attrs }">
                      <v-text-field v-model="effectiveDateFormatted"
                                    :label="$t('date')"
                                    readonly

                                    v-bind="attrs"
                                    :rules="observationDateRules()"
                                    hide-details="auto"
                                    :disabled="disabled"
                                    v-on="on">
                      <fa-icon :icon="['fal', 'calendar']" class="fa-fw" slot="prepend" />
                      </v-text-field>
                    </template>
                    <v-date-picker v-model="effectiveDate" @input="effectiveDateMenu = false"
                                   :locale="$i18n.locale"
                                   :max="getMaxDate"></v-date-picker>
                  </v-menu>
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="12">
                  <v-menu ref="timeMenu"
                          v-model="timeMenu"
                          :close-on-content-click="false"
                          :nudge-right="40"
                          :return-value.sync="effectiveTime"
                          transition="scale-transition"
                          offset-y
                          max-width="290px"
                          min-width="290px">
                    <template v-slot:activator="{ on, attrs }">
                      <v-text-field v-model="effectiveTimeFormatted"
                                    :label="$t('time')"
                                    readonly
                                    v-bind="attrs"
                                    :rules="observationTimeRules()"
                                    hide-details="auto"
                                    :disabled="disabled"
                                    v-on="on">
                    <fa-icon :icon="['fal', 'clock']" class="fa-fw" slot="prepend" />
                    </v-text-field>
                    </template>
                    <v-time-picker v-if="timeMenu"
                                   v-model="effectiveTime"
                                   format="ampm"
                                   :max="maxTime"
                                   full-width
                                   @click:minute="$refs.timeMenu.save(effectiveTime)"></v-time-picker>
                  </v-menu>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-alert dark type="success" v-if="showSuccessAlert">
                    <div>
                      {{ $t('observationAdded') }}
                    </div>
                    <div class="my-2">
                      <BaseButton text @click="onClose">{{$t('close')}}</BaseButton>
                      <BaseButton color="primary" depressed class="mx-2" @click="onSubmitAnotherObservation">{{$t('submitAnotherObservation')}}</BaseButton>
                    </div>
                  </v-alert>
                  <v-alert dark type="error" dismissible v-if="showErrorAlert">
                    {{ errorMessages }}
                  </v-alert>
                </v-col>
              </v-row>
            </v-form>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <BaseButton text @click="onClose" :disabled="disabled">{{$t('cancel')}}</BaseButton>
          <BaseButton color="primary" depressed @click="onSubmitStart" :loading="loading" :disabled="disabled">{{$t('submit')}}</BaseButton>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>
<script lang="ts">
  import Vue from 'vue';
  import { mapState } from 'vuex';
  import { assoc } from 'ramda';
  import { format } from 'date-fns';
  import BodyTemperatureObservation from './BodyTemperatureObservation.vue';
  import BloodPressureObservation from './BloodPressureObservation.vue';
  import OxygenSaturationObservation from './OxygenSaturationObservation.vue';
  import RespiratoryRateObservation from './RespiratoryRateObservation.vue';
  import BloodGlucoseObservation from './BloodGlucoseObservation.vue';
  import WeightObservation from './WeightObservation.vue';
  import HeartRateObservation from './HeartRateObservation.vue';
  import A1cObservation from './A1cObservation.vue';
  import { IObservationResponse, IObservationRequest, IObservationType } from '@/shared/interfaces';
  import { ObservationTypeDto } from '@/IUGOCare-api';
  import { ValidationFn, exists, isSelected } from '../../../validation';

  export default Vue.extend({
    name: 'ObservationsDialog',
    components: { BloodPressureObservation, OxygenSaturationObservation, RespiratoryRateObservation,
                  BodyTemperatureObservation, BloodGlucoseObservation, WeightObservation, HeartRateObservation,
                  A1cObservation },
    props: ['value'],
    data: () => ({
      showDialog: false,
      effectiveDateMenu: false,
      timeMenu: false,
      effectiveDate: null as string,
      effectiveDateFormatted: null as string,
      effectiveTimeFormatted: null as string,
      effectiveTime: null as string,
      selectedObservationType: null as string,
      observationType: null as string,
      loading: false,
      disabled: false,
      showSuccessAlert: false,
      showErrorAlert: false,
      errorMessages: null as string,
      maxTime: null as string,
      loadingObservationTypes: false,
      observationTypes: [],
    }),
    computed: {
      ...mapState('observations', ['recentPatientObservationTypes']),
      getObservationCategory(): string {
        return this.$route.params.observationCategory || '';
      },
      getMaxDate(): string {
        return new Date().toISOString().slice(0, 10);
      },
      getEffectiveDateAndTime(): Date {
        const [year, month, day] = this.effectiveDate.split('-');
        const [hour, minute] = this.effectiveTime.split(':');
        return new Date(Number(year), Number(month) - 1, Number(day), Number(hour), Number(minute));
      },
    },
    methods: {
      observationTypeRules(): ValidationFn[] {
        return [ isSelected ];
      },
      observationDateRules(): ValidationFn[] {
        return [ exists ];
      },
      observationTimeRules(): ValidationFn[] {
        return [
          exists,
          (v: string) => !v || this.isValidTime() || this.$t('invalidTime'),
        ];
      },
      getMaxTime(): string {
        if (!this.effectiveDate || this.effectiveDate === format(new Date(), 'yyyy-MM-dd')) {
          return format(new Date(), 'HH:mm');
        }
        return '';
      },
      isValidTime(): boolean {
        const maxTime = this.getMaxTime();
        const [hour, minute] = this.effectiveTime.split(':');
        const [maxHour, maxMinute] = maxTime.split(':');
        return (!maxTime || maxTime === '') ? true :
          (Number(hour) > Number(maxHour)) ? false :
            (Number(hour) === Number(maxHour) && Number(minute) > Number(maxMinute)) ? false : true;
      },
      formatDate(): string {
        if (!this.effectiveDate) {
          return null;
        }
        const [year, month, day] = this.effectiveDate.split('-');
        return new Date(Number(year), Number(month) - 1, Number(day))
          .toLocaleDateString(this.$i18n.locale, { month: 'long', day: 'numeric', year: 'numeric' });
      },
      closeDialog(): void {
        this.loading = false;
        this.showSuccessAlert = false;
        this.effectiveDate = null;
        this.effectiveDateFormatted = null;
        this.effectiveTime = null;
        this.effectiveTimeFormatted = null;
        this.$emit('input');
      },
      onSubmitStart(): void {
        // @ts-ignore
        if (this.$refs.observationForm.validate()) {
          this.showSuccessAlert = false;
          this.showErrorAlert = false;
          this.loading = !this.loading;
          this.disabled = !this.disabled;

          const observationDate = this.getEffectiveDateAndTime;
          const code = this.observationType;

          const request: IObservationRequest = {
            effectiveDate: observationDate,
            observationCode: code,
          };
          this.$root.$emit('submitObservationStart', request);
        }
      },
      onSubmitEnd(response: IObservationResponse): void {
        this.loading = false;
        if (response.isSaved) {
          this.showSuccessAlert = true;
          this.$root.$emit('observationCreated');
        } else {
          this.showErrorAlert = true;
          this.errorMessages = response.message;
        }
      },
      onObservationTypeExternalChange(newObservationType: string): void {
        const observationCategory = this.getObservationCategory;
        this.selectedObservationType = newObservationType;
        this.observationType = (observationCategory && observationCategory !== '') ? newObservationType : '';
      },
      onClose(): void {
        this.closeDialog();
      },
      onSubmitAnotherObservation() {
        this.showSuccessAlert = false;
        this.disabled = false;
        this.$root.$emit('clearObservationFields');
        // @ts-ignore
        this.$refs.observationForm.resetValidation();
      },
      async getRecentPatientObservationTypes() {
        await this.$store.dispatch('observations/fetchRecentPatientObservationTypes');
      },
      getObservationTypes(): IObservationType[] {
        const types: IObservationType[] = [
          { text: 'bloodPressure', value: 'blood-pressure', order: 7 },
          { text: 'bloodGlucose', value: 'blood-glucose', order: 7 },
          { text: 'oxygenSaturation', value: 'oxygen-saturation', order: 7 },
          { text: 'respiratoryRate', value: 'respiratory-rate', order: 7 },
          { text: 'bodyTemperature', value: 'body-temperature', order: 7 },
          { text: 'heartRate', value: 'heart-rate', order: 7 },
          { text: 'weight', value: 'weight', order: 7 },
          { text: 'a1c', value: 'a1c', order: 7 },
        ];

        if (this.recentPatientObservationTypes &&
          this.recentPatientObservationTypes.length > 0) {
          types.forEach((t: IObservationType) =>
            t.order = this.recentPatientObservationTypes.find((r: ObservationTypeDto) =>
              r.observationType === t.value)?.observationOrder + 1 || t.order,
          );
        }
        return types.map((c) => assoc('text', this.$t(c.text), c))
          .sort((a, b) => (a.order > b.order) ? 1 : (a.order === b.order) ? ((a.text > b.text) ? 1 : -1) : -1);
      },
    },
    watch: {
      effectiveDate() {
        this.effectiveDateFormatted = this.formatDate();
      },
      effectiveTime() {
        if (!this.effectiveTime) {
          this.effectiveTimeFormatted = null;
          return;
        }
        const [hour, minute] = this.effectiveTime.split(':');
        const tempDate = new Date();
        tempDate.setHours(Number(hour));
        tempDate.setMinutes(Number(minute));
        tempDate.setSeconds(0);
        this.effectiveTimeFormatted = format(tempDate, 'hh:mm a');
      },
      async value() {
        // @ts-ignore
        this.$refs.observationForm.resetValidation();

        if (this.value) {
          this.disabled = true;
          this.loadingObservationTypes = true;
          this.observationType = this.selectedObservationType || 'blood-pressure';

          await this.getRecentPatientObservationTypes();
          this.observationTypes = this.getObservationTypes();
          this.loadingObservationTypes = false;
          this.effectiveDate = format(new Date(), 'yyyy-MM-dd');
          this.effectiveTime = format(new Date(), 'HH:mm');
          this.maxTime = this.getMaxTime();
          const observationCategory = this.getObservationCategory;

          if (observationCategory && observationCategory !== '') {
            this.$root.$emit('clearObservationFields');
          }

          this.loading = false;
          this.disabled = false;

          // @ts-ignore
          const observationForm: any = this.$refs.observationForm;
          if (observationForm) {
            // @ts-ignore
            observationForm.resetValidation();
          }
        }
      },
      timeMenu() {
        this.maxTime = this.getMaxTime();
      },
    },
    mounted() {
      this.$root.$on('submitObservationEnd', this.onSubmitEnd);
      this.$root.$on('observationTypeChange', this.onObservationTypeExternalChange);
    },
  });
</script>
