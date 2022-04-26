<template>
  <section>
    <v-dialog :value="value" persistent fixed max-width="600px" :eager="true">
      <v-card id="add-observation-dialog">
        <BaseToolbar
          :title="$t('addObservation')"
          class="colorPatientChart"
          dark
        />
        <v-card-text>
          <v-form ref="addObservationForm" v-model="valid">
            <v-row>
              <v-col cols="12">
                <div class="text-subtitle-1 font-weight-bold">
                  {{ patient.familyName }}, {{ patient.givenName }}
                  {{ patient.middleName }}, {{ patient.age }} {{ patient.gender }}
                </div>
              </v-col>
              <v-col cols="12">
                <div class="text-subtitle-1 mb-4">
                  ({{ preferredPhone.type }}) {{ preferredPhone.value }}
                </div>
              </v-col>
              <v-col cols="12">
                <v-select
                  v-model="selectedObservationType"
                  :items="observationTypes"
                  class="mb-4"
                  hide-details="auto"
                >
                </v-select>
              </v-col>
              <AddA1cObservationForm
                v-if="selectedObservationType == 'A1C'"
              />
              <AddBloodGlucoseObservationForm
                v-if="selectedObservationType == 'Blood Glucose'"
              />
              <AddBloodPressureObservationForm
                v-if="selectedObservationType == 'Blood Pressure'"
              />
              <AddWeightObservationForm
                v-if="selectedObservationType == 'Weight'"
              />
              <AddOxygenSaturationObservationForm
                v-if="selectedObservationType == 'Oxygen Saturation'"
              />
              <AddTemperatureObservationForm
                v-if="selectedObservationType == 'Temperature'"
              />

              <!-- TODO: Create a BaseDatePicker component and use it here -->
              <v-col cols="12" md="6">
                <v-menu
                  v-model="showDatePicker"
                  :close-on-content-click="false"
                  transition="scale-transition"
                  offset-y
                  min-width="auto"
                >
                  <template v-slot:activator="{ on, attrs }">
                    <v-text-field
                      v-model="effectiveDateFormatted"
                      :label="$t('effectiveDate')"
                      readonly
                      v-bind="attrs"
                      hide-details="auto"
                      v-on="on"
                      class="mb-4"
                    >
                      <fa-icon
                        :icon="['fal', 'calendar']"
                        class="fa-fw"
                        slot="prepend"
                      />
                    </v-text-field>
                  </template>
                  <v-date-picker
                    v-model="effectiveDate"
                    color="primary"
                    :locale="$i18n.locale"
                    @input="showDatePicker = false"
                    class="mb-4"
                  ></v-date-picker>
                </v-menu>
              </v-col>
              <v-col cols="12" md="6">
                <BaseTimePicker
                  v-model="effectiveTime"
                  hideArrows
                  showList
                  class="mb-4"
                />
              </v-col>
              <v-col cols="12" class="mb-4">
                <label>{{ $t('author') }}: {{ author }}</label>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
        <v-card-actions class="pt-0">
          <v-spacer></v-spacer>
          <BaseButton text @click="cancel" :disabled="isSaving">
            {{ $t('cancel') }}
          </BaseButton>
          <BaseButton color="primary" @click="submitPatient">
            {{ $t('submit') }}
          </BaseButton>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </section>
</template>

<script lang="ts">
import Vue from 'vue';
import BaseTimePicker from '@/components/base/BaseTimePicker.vue';
import AddA1cObservationForm from './components/AddA1cObservationForm.vue';
import AddBloodGlucoseObservationForm from './components/AddBloodGlucoseObservationForm.vue';
import AddBloodPressureObservationForm from './components/AddBloodPressureObservationForm.vue';
import AddWeightObservationForm from './components/AddWeightObservationForm.vue';
import AddOxygenSaturationObservationForm from './components/AddOxygenSaturationObservationForm.vue';
import AddTemperatureObservationForm from './components/AddTemperatureObservationForm.vue';

export default Vue.extend({
  name: 'AddObservationDialog',
  components: {
    BaseTimePicker,
    AddA1cObservationForm,
    AddBloodGlucoseObservationForm,
    AddBloodPressureObservationForm,
    AddWeightObservationForm,
    AddOxygenSaturationObservationForm,
    AddTemperatureObservationForm,
  },
  props: {
    value: {
      type: Boolean,
      default: false,
    },
  },
  data(): any {
    return {
      author: 'Smith, David',
      valid: false,
      effectiveDate: null as string,
      effectiveTime: null as string,
      showMenu: false,
      effectiveDateFormatted: null as string,
      showDatePicker: false,
      isSaving: false,
      savedSuccessfully: false,
      selectedObservationType: 'Blood Glucose',
      observationTypes: [
        'A1C',
        'Blood Glucose',
        'Blood Pressure',
        'Temperature',
        'Weight',
        'Oxygen Saturation',
      ],
      patient: {
        familyName: 'Allain',
        givenName: 'Christine',
        middleName: 'Jolie',
        age: 67,
        gender: 'F',
        phone: [
          {
            type: 'H',
            value: '415-356-0000',
            preferred: true,
          },
          {
            type: 'M',
            value: '415-357-0000',
            preferred: false,
          },
        ],
      },
    };
  },
  computed: {
    preferredPhone(): any {
      return this.patient.phone.find((_: any) => _.preferred == true);
    },
  },
  watch: {
    effectiveDate() {
      this.effectiveDateFormatted = this.formatDateForDatePicker(
        this.effectiveDate
      );
    },
  },
  methods: {
    cancel(): void {
      this.$emit('input');
      this.resetForm();
    },
    submitPatient(): void {
      this.isSaving = true;
      this.resetForm();
      this.savedSuccessfully = true;
      this.$emit('input');
    },
    resetForm(): void {
      //
    },
    setEffectiveTime(value: any) {
      this.effectiveTime = value;
    },
    // TODO: Move this method into the datetime-helper utils when
    // we create a base DatePicker component
    formatDateForDatePicker(date: string): string {
      if (!date) {
        return null as string;
      }
      const [year, month, day] = date.split('-');
      return new Date(
        Number(year),
        Number(month) - 1,
        Number(day)
      ).toLocaleDateString(this.$i18n.locale, {
        month: 'long',
        day: 'numeric',
        year: 'numeric',
      });
    },
  },
});
</script>
