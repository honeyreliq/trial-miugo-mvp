<template>
  <v-container id="observations"
               fluid
               tag="section"
               class="overflow-y-auto overflow-x-hidden">
    <p class="text-h4 text-center">
      {{$t('commandTests')}}
    </p>

    <v-row>
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-select :items="commands"
                  :label="$t('selectACommand')"
                  v-model="command" />
      </v-col>
    </v-row>
    <v-row v-if="command == 'AcceptToS'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1">
          <v-col cols="12" sm="6">
            <v-card v-if="patients" class="px-5 py-3">
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="text-h5 mb-1">{{$t('patientAcceptsToS')}}</v-list-item-title>
                </v-list-item-content>
                <v-icon>mdi-email</v-icon>
              </v-list-item>
              <v-form class="mt-5">
                <v-select :items="patients"
                          :label="$t('inactivePatients')"
                          v-model="patient" />
              </v-form>
              <v-card-actions class="pl-0">
                <v-btn color="success"
                       min-width="100"
                       @click="patientAcceptsToS"
                       :disabled="submitted">
                  {{$t('submit')}}
                </v-btn>
              </v-card-actions>
            </v-card>
            <div v-else>
              {{$t('loadingMsg')}}
            </div>
          </v-col>
        </v-row>
      </v-col>
    </v-row> <!-- End AcceptToS -->
    <v-row v-if="command == 'Register Patient'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1">
          <v-col cols="12" sm="6">
            <v-card class="px-5 py-3">
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="text-h5 mb-1">{{$t('registerPatient')}}</v-list-item-title>
                </v-list-item-content>
                <v-icon>mdi-account</v-icon>
              </v-list-item>
              <v-form class="mt-5" ref="registerPatientForm">
                <v-row>
                  <v-col cols="8">
                    <v-text-field :label="$t('clinicPatientId')"
                                  v-model="clinicPatientId"
                                  clearable
                                  required
                                  maxlength="36"
                                  :rules="clinicPatientIdRules"
                                  placeholder="00000000-0000-0000-0000-000000000000">
                    </v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-btn color="success"
                           block
                           @click="generateClinicPatientId">
                      {{$t('randomId')}}
                    </v-btn>
                  </v-col>
                </v-row>
                <v-row>
                  <v-text-field v-model="email"
                                :rules="emailRules"
                                :label="$t('email')"
                                clearable
                                :placeholder="$t('emailPlaceholder')"></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="givenName"
                                :label="$t('givenName')"
                                clearable></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="middleName"
                                :label="$t('middleName')"
                                clearable></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="familyName"
                                :label="$t('familyName')"
                                clearable></v-text-field>
                </v-row>
                <v-row>
                  <v-select :items="languages"
                            :label="$t('language')"
                            v-model="selectedLanguage" />
                </v-row>
                <v-row>
                  <v-text-field v-model="patientPhoneNumber"
                                :label="$t('phone')"
                                clearable
                                :rules="phoneRules"></v-text-field>
                </v-row>
                <v-row>
                  <v-menu ref="menuDate"
                          v-model="dateMenu"
                          :close-on-content-click="false"
                          :nudge-right="40"
                          transition="scale-transition"
                          offset-y
                          max-width="290px"
                          min-width="290px">
                    <template v-slot:activator="{ on, attrs }">
                      <v-text-field v-model="patientBirthDateFormatted"
                                    :label="$t('birthDate')"
                                    prepend-icon="mdi-calendar"
                                    readonly
                                    :rules="birthDateRules"
                                    v-bind="attrs"
                                    v-on="on"></v-text-field>
                    </template>
                    <v-date-picker v-model="patientBirthDate"
                                   color="success"
                                   full-width
                                   :max="nowDate"
                                   required
                                   :locale="$i18n.locale"
                                   @input="dateMenu = false">
                    </v-date-picker>
                  </v-menu>
                </v-row>
                <v-row>
                  <v-select :items="providers"
                            :label="$t('primaryCareProvider')"
                            v-model="primaryCareProviderId"
                            required
                            :loading="loadingProviders"
                            item-text="name"
                            item-value="id" />
                </v-row>
              </v-form>
              <v-card-actions class="pl-0">
                <v-btn color="success"
                       min-width="100"
                       @click="registerPatient"
                       :disabled="submitted">
                  {{$t('submit')}}
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-col>
    </v-row><!-- End Register Patient -->
    <v-row v-if="command == 'SendPatientOnboardingEmail'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1">
          <v-col cols="12" sm="6">
            <v-card v-if="patients" class="px-5 py-3">
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="text-h5 mb-1">{{$t('sendPatientOnboardingEmail')}}</v-list-item-title>
                </v-list-item-content>
                <v-icon>mdi-email</v-icon>
              </v-list-item>
              <v-form class="mt-5">
                <v-select :items="patients"
                          :label="$t('inactivePatients')"
                          v-model="patient" />
              </v-form>
              <v-card-actions class="pl-0">
                <v-btn color="success"
                       min-width="100"
                       @click="sendPatientOnboardingEmail"
                       :disabled="submitted">
                  {{$t('submit')}}
                </v-btn>
              </v-card-actions>
            </v-card>
            <div v-else>
              {{$t('loadingMsg')}}
            </div>
          </v-col>
        </v-row>
      </v-col>
    </v-row><!-- End SendPatientOnboardingEmail -->
    <v-row v-if="command == 'MarketingEmails'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1">
          <v-col cols="12" sm="6">
            <v-card v-if="patients" class="px-5 py-3">
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="text-h5 mb-1">{{$t('enableDisableMarketingEmails')}}</v-list-item-title>
                </v-list-item-content>
                <v-icon>mdi-email</v-icon>
              </v-list-item>
              <v-form class="mt-5">
                <v-select :items="patients"
                          :label="$t('allPatients')"
                          v-model="patient" />
              </v-form>
              <v-card-actions class="pl-0">
                <v-btn color="success"
                       min-width="100"
                       @click="enableMarketingEmails"
                       :disabled="submitted">
                  {{$t('enable')}}
                </v-btn>
                <v-btn color="error"
                       min-width="100"
                       @click="disableMarketingEmails"
                       :disabled="submitted">
                  {{$t('disable')}}
                </v-btn>
              </v-card-actions>
            </v-card>
            <div v-else>
              {{$t('loadingMsg')}}
            </div>
          </v-col>
        </v-row>
      </v-col>
    </v-row><!-- End MarketingEmails -->
    <v-row v-if="command == 'Inactive Patients'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1">
          <v-col cols="12" sm="12">
            <div class="text-h5 mb-4 py-4">{{$t('inactivePatients')}}</div>
            <v-simple-table v-if="inactivePatients" class="px-5 py-3" :fixed-header="fixedHeader">
              <template v-slot:default>
                <thead>
                  <tr>
                    <th class="text-left">{{$t('clinicPatientId')}}</th>
                    <th class="text-left">{{$t('email')}}</th>
                    <th class="text-left">{{$t('activateAccount')}}</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item, index) in inactivePatients" :key="item.clinicPatientId">
                    <td>{{ item.clinicPatientId }}</td>
                    <td>{{ item.emailAddress }}</td>
                    <td>
                      <v-btn class="ma-2"
                             color="success"
                             :loading="loading && indexClicked === index"
                             :disabled="loading || !item.activationCode"
                             @click="loader(index, item)">
                        <span>{{$t('activate')}}</span>
                        <v-icon right>mdi-account-check</v-icon>
                      </v-btn>
                    </td>
                  </tr>
                </tbody>
              </template>
            </v-simple-table>
            <div v-else>
              {{$t('loadingMsg')}}
            </div>
          </v-col>
        </v-row>
      </v-col>
    </v-row><!-- End Inactive Patients -->
    <v-row v-if="command == 'CreateObservation'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1 pb-6">
          <v-col cols="12" sm="6">
            <v-card :title="$t('createObservation')" class="px-5 py-3">
              <v-form class="mt-5"
                      ref="createObservationForm"
                      lazy-validation>
                <v-row align="center" justify="space-between">
                  <v-col cols="8">
                    <v-text-field :ref="'observationId'"
                                  :label="$t('observationId')"
                                  v-model="observationId"
                                  clearable
                                  required
                                  :counter="36"
                                  maxlength="36"
                                  :rules="observationIdRules"
                                  placeholder="00000000-0000-0000-0000-000000000000">
                    </v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-btn color="success"
                           block
                           @click="generateObservationUuid">
                      {{$t('randomId')}}
                    </v-btn>
                  </v-col>
                </v-row>
                <v-select :ref="'patient'"
                          v-if="patients"
                          :items="patients"
                          :label="$t('associatedPatient')"
                          v-model="patient"
                          :rules="observationPatientRules"
                          required />
                <div v-else>
                  <v-select loading
                            :label="$t('loadingPatients')" />
                </div>
                <v-select :ref="'observationCode'"
                          v-model="observationCode"
                          :label="$t('observation')"
                          :items="observationCodes"
                          :rules="observationCodeRules"
                          @change ="clearObservationsFormFields"
                          required />
                <v-row v-if="observationCode === 'body-temperature'">
                  <v-col cols="8">
                    <v-text-field :ref="'body-temperature'"
                                  :label="$t('bodyTemperature')"
                                  v-model="bodyTemperatureDataCodes[0].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="getBodyTemperatureRules"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-select v-model="bodyTemperatureDataCodes[0].unit"
                              :label="$t('units')"
                              :items="bodyTemperatureDataCodes[0].units"
                              :item-text="unit => unit.value"
                              :disabled="disabledFields"
                              @change="setBodyTemperatureDataCodesAfterUnitChange(bodyTemperatureDataCodes)"
                              required />
                  </v-col>
                </v-row>
                <!-- Blood preasure -->
                <v-row v-if="observationCode === 'blood-pressure'">
                  <v-col cols="4">
                    <v-text-field :ref="'systolic'"
                                  :label="$t('systolic')"
                                  v-model="bloodPressureDataCodes[0].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="[...observationDataNumberRules, ...decimalRules]"
                                  :suffix="bloodPressureDataCodes[0].unit"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field :ref="'diastolic'"
                                  :label="$t('diastolic')"
                                  v-model="bloodPressureDataCodes[1].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="[...observationDataNumberRules, ...decimalRules]"
                                  :suffix="bloodPressureDataCodes[1].unit"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field :ref="'pulseBP'"
                                  :label="$t('pulse')"
                                  v-model="bloodPressureDataCodes[2].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="[...observationDataNumberRules, ...decimalRules]"
                                  :suffix="bloodPressureDataCodes[2].unit"></v-text-field>
                  </v-col>
                </v-row>
                <!-- Weight -->
                <v-row v-if="observationCode === 'weight'">
                  <v-col cols="12">
                    <v-text-field :ref="'weight'"
                                  :label="$t('weightLbl')"
                                  v-model="weightDataCodes[0].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="[...observationDataNumberRules, ...decimalRules]"
                                  :suffix="weightDataCodes[0].unit"></v-text-field>
                  </v-col>
                </v-row>
                <!-- Oxygen saturation -->
                <v-row v-if="observationCode === 'oxygen-saturation'">
                  <v-col cols="6">
                    <v-text-field :ref="'oxygen-saturation'"
                                  :label="$t('oxygenSaturation')"
                                  v-model="oxygenDataCodes[0].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="[...oxygenSaturationValueRules, ...decimalRules]"
                                  :suffix="oxygenDataCodes[0].unit"></v-text-field>
                  </v-col>
                  <v-col cols="6">
                    <v-text-field :ref="'pulseOS'"
                                  :label="$t('pulse')"
                                  v-model="oxygenDataCodes[1].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="[...pulseValueRules, ...decimalRules]"
                                  :suffix="oxygenDataCodes[1].unit"></v-text-field>
                  </v-col>
                </v-row>
                <!-- Heart rate -->
                <v-row v-if="observationCode === 'heart-rate'">
                  <v-col cols="12">
                    <v-text-field :ref="'heart-rate'"
                                  :label="$t('heartRate')"
                                  v-model="heartRateDataCodes[0].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="[...heartRateValueRules, ...decimalRules]"
                                  :suffix="heartRateDataCodes[0].unit"></v-text-field>
                  </v-col>
                </v-row>
                <!-- Blood glucose -->
                <v-row v-if="observationCode === 'blood-glucose'">
                  <v-col cols="8">
                    <v-text-field :ref="'blood-glucose'"
                                  :label="$t('bloodGlucose')"
                                  v-model="bloodGlucoseDataCodes[0].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="getBloodGlucoseRules"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-select v-model="bloodGlucoseDataCodes[0].unit"
                              :label="$t('units')"
                              :items="bloodGlucoseDataCodes[0].units"
                              :item-text="unit => unit.value"
                              :disabled="disabledFields"
                              @change="setBloodGlucoseDataCodesAfterUnitChange(bloodGlucoseDataCodes)"
                              required />
                  </v-col>
                  <v-radio-group row v-model="bloodGlucoseDataCodes[1].code">
                    <v-radio :label="$t('fastingPreMeal')" value="meal-code-fasting"></v-radio>
                    <v-radio :label="$t('nonFastingPostMeal')" value="meal-code-non-fasting"></v-radio>
                  </v-radio-group>
                </v-row>
                <!-- Respiratory rate -->
                <v-row v-if="observationCode === 'respiratory-rate'">
                  <v-col cols="12">
                    <v-text-field :ref="'respiratory-rate'"
                                  :label="$t('respiratoryRate')"
                                  v-model="respiratoryRateDataCodes[0].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="[...respiratoryRateValueRules, ...decimalRules]"
                                  :suffix="respiratoryRateDataCodes[0].unit"></v-text-field>
                  </v-col>
                </v-row>
                <!-- Workouts -->
                <v-row v-if="observationCode === 'workouts'">
                  <v-col cols="12">
                    <v-text-field :ref="'fitness-duration'"
                                  :label="$t('duration')"
                                  v-model="workoutDataCodes[0].value"
                                  :rules="durationValueRules"
                                  suffix="hh:mm:ss"></v-text-field>
                  </v-col>

                  <v-col cols="8">
                    <v-text-field :ref="'distance'"
                                  :label="$t('distance')"
                                  v-model="workoutDataCodes[1].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="getDistanceRules"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-select v-model="workoutDataCodes[1].unit"
                              :label="$t('units')"
                              :items="workoutDataCodes[1].units"
                              :item-text="unit => unit.value"
                              @change="setWorkoutDataCodesAfterUnitChange(workoutDataCodes)">
                    </v-select>
                  </v-col>

                  <v-col cols="12">
                    <v-text-field :ref="'steps'"
                                  :label="$t('steps')"
                                  v-model="workoutDataCodes[2].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="workoutStepsRules"
                                  :suffix="$t(workoutDataCodes[2].unit).toLowerCase()"></v-text-field>
                  </v-col>

                  <v-col cols="8">
                    <v-text-field :ref="'elevation'"
                                  :label="$t('elevation')"
                                  v-model="workoutDataCodes[3].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="getElevationRules"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-select v-model="workoutDataCodes[3].unit"
                              :label="$t('units')"
                              :items="workoutDataCodes[3].units"
                              :item-text="unit => unit.value"
                              @change="setWorkoutDataCodesAfterUnitChange(workoutDataCodes)" />
                  </v-col>

                  <v-col cols="12">
                    <v-text-field :ref="'floors'"
                                  :label="$t('floors')"
                                  v-model="workoutDataCodes[4].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="workoutFloorsRules"
                                  :suffix="$t(workoutDataCodes[4].unit).toLowerCase()"></v-text-field>
                  </v-col>

                  <v-col cols="8">
                    <v-text-field :ref="'water-consumed'"
                                  :label="$t('waterConsumed')"
                                  v-model="workoutDataCodes[5].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="getWaterConsumedRules"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-select v-model="workoutDataCodes[5].unit"
                              :label="$t('units')"
                              :items="workoutDataCodes[5].units"
                              :item-text="unit => unit.value"
                              @change="setWorkoutDataCodesAfterUnitChange(workoutDataCodes)" />
                  </v-col>

                  <v-col cols="12">
                    <v-text-field :ref="'calories-burned'"
                                  :label="$t('caloriesBurned')"
                                  v-model="workoutDataCodes[6].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="workoutCaloriesRules"
                                  :suffix="workoutDataCodes[6].unit.toLowerCase()"></v-text-field>
                  </v-col>

                  <v-col cols="12">
                    <v-text-field :ref="'heart-rateW'"
                                  :label="$t('heartRate')"
                                  v-model="workoutDataCodes[7].value"
                                  class="type-number"
                                  :type="'number'"
                                  :rules="workoutHRRules"
                                  :suffix="workoutDataCodes[7].unit.toLowerCase()"></v-text-field>
                  </v-col>

                </v-row>
                <!-- Sleep -->
                <v-row v-if="observationCode === 'sleep'">
                  <v-col cols="12">
                    <v-text-field :ref="'total'"
                                  :label="$t('total')"
                                  v-model="sleepDataCodes[0].value"
                                  :rules="sleepValueRules"
                                  :suffix="sleepDataCodes[0].unit"></v-text-field>
                  </v-col>
                </v-row>
                <v-row v-if="observationCode === 'sleep'">
                  <v-col cols="6">
                    <v-text-field :ref="'rem'"
                                  :label="$t('rem')"
                                  v-model="sleepDataCodes[1].value"
                                  :rules="sleepOptionalFieldValueRules"
                                  :suffix="sleepDataCodes[1].unit"></v-text-field>
                  </v-col>
                  <v-col cols="6">
                    <v-text-field :ref="'deep'"
                                  :label="$t('deep')"
                                  v-model="sleepDataCodes[2].value"
                                  :rules="sleepOptionalFieldValueRules"
                                  :suffix="sleepDataCodes[2].unit"></v-text-field>
                  </v-col>
                </v-row>
                <v-row v-if="observationCode === 'sleep'">
                  <v-col cols="6">
                    <v-text-field :ref ="'light'"
                                  :label="$t('light')"
                                  v-model="sleepDataCodes[3].value"
                                  :rules="sleepOptionalFieldValueRules"
                                  :suffix="sleepDataCodes[3].unit"></v-text-field>
                  </v-col>
                  <v-col cols="6">
                    <v-text-field :ref ="'awake'"
                                  :label="$t('awake')"
                                  v-model="sleepDataCodes[4].value"
                                  :rules="sleepOptionalFieldValueRules"
                                  :suffix="sleepDataCodes[4].unit"></v-text-field>
                  </v-col>
                </v-row>
                <v-menu ref="menuDate"
                        v-model="dateMenu"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        transition="scale-transition"
                        offset-y
                        max-width="290px"
                        min-width="290px">
                  <template v-slot:activator="{ on, attrs }">
                    <v-text-field v-model="observationDatePickerFormatted"
                                  :label="$t('date')"
                                  prepend-icon="mdi-calendar"
                                  readonly
                                  :rules="observationDateRules"
                                  v-bind="attrs"
                                  v-on="on"></v-text-field>
                  </template>
                  <v-date-picker v-model="observationDatePicker"
                                 color="success"
                                 full-width
                                 :max="nowDate"
                                 required
                                 :locale="$i18n.locale"
                                 @input="dateMenu = false">
                  </v-date-picker>
                </v-menu>
                <v-menu ref="menuTime"
                        v-model="timeMenu"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        transition="scale-transition"
                        offset-y
                        max-width="290px"
                        min-width="290px">
                  <template v-slot:activator="{ on, attrs }">
                    <v-text-field v-model="observationTimeFormatted"
                                  :label="$t('time')"
                                  prepend-icon="mdi-clock-time-four-outline"
                                  readonly
                                  :rules="observationTimeRules"
                                  v-bind="attrs"
                                  v-on="on"></v-text-field>
                  </template>
                  <v-time-picker v-if="timeMenu"
                                 v-model="observationTime"
                                 :max="getMaxTime()"
                                 color="success"
                                 full-width
                                 @click:minute="$refs.menuTime.save(observationTime)"></v-time-picker>
                </v-menu>
              </v-form>
              <v-card-actions class="pl-0 pb-12">
                <v-btn color="success"
                       min-width="100"
                       @click="createObservation"
                       :disabled="submitted">
                  {{$t('submit')}}
                </v-btn>
                <v-btn color="warning"
                       min-width="100"
                       @click="resetObservationForm"
                       :disabled="submitted">
                  {{$t('reset')}}
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-col>
    </v-row><!-- End CreateObservation -->
    <v-row v-if="command == 'Update Email'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1">
          <v-col cols="12" sm="6">
            <v-card v-if="patients"
                    class="px-5 py-3">
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="text-h5 mb-1">{{$t('updateEmail')}}</v-list-item-title>
                </v-list-item-content>
                <v-icon>mdi-email</v-icon>
              </v-list-item>
              <v-form class="mt-5" ref="updateemailform">
                <v-select :items="patients"
                          :label="$t('patient')"
                          v-model="patient"
                          :rules="observationPatientRules" />
                <v-text-field v-model="email"
                              :rules="requiredEmailRules"
                              :label="$t('email')"
                              required></v-text-field>
              </v-form>
              <v-card-actions class="pl-0">
                <v-btn color="success"
                       min-width="100"
                       @click="updateEmail"
                       :disabled="submitted">
                  {{$t('submit')}}
                </v-btn>
              </v-card-actions>
            </v-card>
            <div v-else>
              {{$t('loadingMsg')}}
            </div>
          </v-col>
        </v-row>
      </v-col>
    </v-row><!-- End UpdateEmail -->
    <v-row v-if="command === 'UpdateObservation'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1 pb-6">
          <v-col cols="12" sm="6">
            <v-card :title="$t('updateObservations')" class="px-5 py-3">
              <v-form class="mt-5"
                      ref="updateObservationsForm"
                      lazy-validation>

                <v-select v-if="patientObservations"
                          v-model="patientObservation"
                          :hint="patientObservation ? `${patientObservation.observationCode} - ${patientObservation.id}` : ''"
                          :items="patientObservations"
                          item-text="id"
                          :label="$t('patientObservations')"
                          persistent-hint
                          return-object
                          single-line
                          @change="mapPatientObservationData()"></v-select>

                <div v-else>
                  <v-select loading
                            :label="$t('loadingPatients')" />
                </div>

                <!-- Workouts -->
                <v-row v-if="observationCode === 'workouts'">
                  <v-col cols="12">
                    <v-text-field :ref="'fitness-duration-update'"
                                  :label="$t('duration')"
                                  v-model="workoutDataCodes[0].value"
                                  :rules="durationValueRules"
                                  suffix="hh:mm:ss"></v-text-field>
                  </v-col>

                  <v-col cols="8">
                    <v-text-field :ref="'distance-update'"
                                  :label="$t('distance')"
                                  v-model="workoutDataCodes[1].value"
                                  :rules="getDistanceRules"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-select v-model="workoutDataCodes[1].unit"
                              :label="$t('units')"
                              :items="workoutDataCodes[1].units"
                              :item-text="unit => unit.value"
                              @change="setWorkoutDataCodesAfterUnitChange(workoutDataCodes)">
                      </v-select>
                  </v-col>

                  <v-col cols="12">
                    <v-text-field :ref="'steps-update'"
                                  :label="$t('steps')"
                                  v-model="workoutDataCodes[2].value"
                                  :rules="workoutStepsRules"
                                  :suffix="$t(workoutDataCodes[2].unit).toLowerCase()"></v-text-field>
                  </v-col>

                  <v-col cols="8">
                    <v-text-field :ref="'elevation-update'"
                                  :label="$t('elevation')"
                                  v-model="workoutDataCodes[3].value"
                                  :rules="getElevationRules"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-select v-model="workoutDataCodes[3].unit"
                              :label="$t('units')"
                              :items="workoutDataCodes[3].units"
                              :item-text="unit => unit.value"
                              @change="setWorkoutDataCodesAfterUnitChange(workoutDataCodes)" />
                  </v-col>

                  <v-col cols="12">
                    <v-text-field :ref="'floors-update'"
                                  :label="$t('floors')"
                                  v-model="workoutDataCodes[4].value"
                                  :rules="workoutFloorsRules"
                                  :suffix="$t(workoutDataCodes[4].unit).toLowerCase()"></v-text-field>
                  </v-col>

                  <v-col cols="8">
                    <v-text-field :ref="'water-consumed-update'"
                                  :label="$t('waterConsumed')"
                                  v-model="workoutDataCodes[5].value"
                                  :rules="getWaterConsumedRules"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-select v-model="workoutDataCodes[5].unit"
                              :label="$t('units')"
                              :items="workoutDataCodes[5].units"
                              :item-text="unit => unit.value"
                              @change="setWorkoutDataCodesAfterUnitChange(workoutDataCodes)" />
                  </v-col>

                  <v-col cols="12">
                    <v-text-field :ref="'calories-burned-update'"
                                  :label="$t('caloriesBurned')"
                                  v-model="workoutDataCodes[6].value"
                                  :rules="workoutCaloriesRules"
                                  :suffix="workoutDataCodes[6].unit.toLowerCase()"></v-text-field>
                  </v-col>

                  <v-col cols="12">
                    <v-text-field :ref="'heart-rate-update'"
                                  :label="$t('heartRate')"
                                  v-model="workoutDataCodes[7].value"
                                  :rules="workoutHRRules"
                                  :suffix="workoutDataCodes[7].unit.toLowerCase()"></v-text-field>
                  </v-col>
                </v-row>

                <!-- Sleep -->
                <v-row v-if="observationCode === 'sleep'">
                  <v-col cols="12">
                    <v-text-field :label="$t('total')"
                                  v-model="sleepDataCodes[0].value"
                                  :rules="sleepValueRules"
                                  :suffix="sleepDataCodes[0].unit"></v-text-field>
                  </v-col>
                </v-row>
                <v-row v-if="observationCode === 'sleep'">
                  <v-col cols="6">
                    <v-text-field :label="$t('rem')"
                                  v-model="sleepDataCodes[1].value"
                                  :rules="sleepOptionalFieldValueRules"
                                  :suffix="sleepDataCodes[1].unit"></v-text-field>
                  </v-col>
                  <v-col cols="6">
                    <v-text-field :label="$t('deep')"
                                  v-model="sleepDataCodes[2].value"
                                  :rules="sleepOptionalFieldValueRules"
                                  :suffix="sleepDataCodes[2].unit"></v-text-field>
                  </v-col>
                </v-row>
                <v-row v-if="observationCode === 'sleep'">
                  <v-col cols="6">
                    <v-text-field :label="$t('light')"
                                  v-model="sleepDataCodes[3].value"
                                  :rules="sleepOptionalFieldValueRules"
                                  :suffix="sleepDataCodes[3].unit"></v-text-field>
                  </v-col>
                  <v-col cols="6">
                    <v-text-field :label="$t('awake')"
                                  v-model="sleepDataCodes[4].value"
                                  :rules="sleepOptionalFieldValueRules"
                                  :suffix="sleepDataCodes[4].unit"></v-text-field>
                  </v-col>
                </v-row>
                <v-menu ref="menuDate"
                        v-model="dateMenu"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        transition="scale-transition"
                        offset-y
                        max-width="290px"
                        min-width="290px">
                  <template v-slot:activator="{ on, attrs }">
                    <v-text-field v-model="observationDatePickerFormatted"
                                  :label="$t('date')"
                                  prepend-icon="mdi-calendar"
                                  readonly
                                  :rules="observationDateRules"
                                  v-bind="attrs"
                                  v-on="on"></v-text-field>
                  </template>
                  <v-date-picker v-model="observationDatePicker"
                                 color="success"
                                 full-width
                                 :max="nowDate"
                                 required
                                 :locale="$i18n.locale"
                                 @input="dateMenu = false">
                  </v-date-picker>
                </v-menu>
                <v-menu ref="menuTime"
                        v-model="timeMenu"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        transition="scale-transition"
                        offset-y
                        max-width="290px"
                        min-width="290px">
                  <template v-slot:activator="{ on, attrs }">
                    <v-text-field v-model="observationTimeFormatted"
                                  :label="$t('time')"
                                  prepend-icon="mdi-clock-time-four-outline"
                                  readonly
                                  :rules="observationTimeRules"
                                  v-bind="attrs"
                                  v-on="on"></v-text-field>
                  </template>
                  <v-time-picker v-if="timeMenu"
                                 v-model="observationTime"
                                 :max="getMaxTime()"
                                 color="success"
                                 full-width
                                 @click:minute="$refs.menuTime.save(observationTime)"></v-time-picker>
                </v-menu>
              </v-form>
              <v-card-actions class="pl-0 pb-12">
                <v-btn color="success"
                       min-width="100"
                       @click="updateObservation"
                       :disabled="submitted">
                  {{$t('submit')}}
                </v-btn>
                <v-btn color="warning"
                       min-width="100"
                       @click="resetUpdateObservationForm"
                       :disabled="submitted">
                  {{$t('reset')}}
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-col>
    </v-row><!-- End UpdateObservation -->
    <v-row v-if="command == 'substituteHtmlTranslation'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1">
          <v-col cols="12" sm="6">
            <v-card class="px-5 py-3">
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="text-h5 mb-1">{{$t('substituteHtmlTranslation')}}</v-list-item-title>
                </v-list-item-content>
                <v-icon>mdi-file-upload</v-icon>
              </v-list-item>
              <v-form class="mt-5" ref="translationElementForm">
                <v-select :items="getElements"
                          :label="$t('element')"
                          v-model="translationElement"
                          :loading="loadingElements"
                          :rules="elementRules" />
                <v-select :items="languages"
                          :label="$t('language')"
                          v-model="selectedLanguage"
                          :rules="languageRules" />
                <template>
                  <v-file-input show-size
                                counter
                                prepend-icon="mdi-paperclip"
                                accept=".html"
                                chips
                                :label="$t('file')"
                                :rules="htmlFileRules"
                                v-model="selectedFile"></v-file-input>
                </template>
              </v-form>
              <v-card-actions class="pl-0">
                <v-btn color="success"
                       min-width="100"
                       @click="updateHtmlFile"
                       :disabled="submitted">
                  {{$t('submit')}}
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-col>
    </v-row><!-- End substituteHtmlTranslation -->
    <v-row v-if="command == 'organization'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1">
          <v-col cols="12" sm="6">
            <v-card class="px-5 py-3">
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="text-h5 mb-1">{{$t('createOrganization')}}</v-list-item-title>
                </v-list-item-content>
                <v-icon>mdi-playlist-plus</v-icon>
              </v-list-item>
              <v-form class="mt-5" ref="organizationForm">
                <v-row>
                  <v-col cols="8">
                    <v-text-field :label="$t('id')"
                                  v-model="guidId"
                                  clearable
                                  required
                                  :counter="36"
                                  maxlength="36"
                                  :rules="organizationIdRules"
                                  placeholder="00000000-0000-0000-0000-000000000000">
                    </v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-btn color="success"
                           block
                           @click="generateGuidId">
                      {{$t('randomId')}}
                    </v-btn>
                  </v-col>
                </v-row>
                <v-row>
                  <v-text-field v-model="name"
                                :rules="nameRules"
                                :label="$t('name')"
                                :counter="300"
                                maxlength="300"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="phone"
                                :rules="phoneRules"
                                :label="$t('phone')"
                                :counter="20"
                                maxlength="20"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="country"
                                :rules="countryRules"
                                :label="$t('country')"
                                :counter="2"
                                maxlength="2"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="state"
                                :rules="stateRules"
                                :label="$t('state')"
                                :counter="2"
                                maxlength="2"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="city"
                                :rules="cityRules"
                                :label="$t('city')"
                                :counter="200"
                                maxlength="200"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="addressLines"
                                :rules="addressLinesRules"
                                :label="$t('address')"
                                :counter="500"
                                maxlength="500"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="zipCode"
                                :rules="zipCodeRules"
                                :label="$t('zipCode')"
                                :counter="20"
                                maxlength="20"
                                required></v-text-field>
                </v-row>
              </v-form>
              <v-card-actions class="pl-0">
                <v-btn color="success"
                       min-width="100"
                       @click="createOrganization"
                       :disabled="submitted">
                  {{$t('submit')}}
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-col>
    </v-row><!-- End Create Organization -->
    <v-row v-if="command == 'provider'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1">
          <v-col cols="12" sm="6">
            <v-card class="px-5 py-3">
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="text-h5 mb-1">{{$t('createProvider')}}</v-list-item-title>
                </v-list-item-content>
                <v-icon>mdi-playlist-plus</v-icon>
              </v-list-item>
              <v-form class="mt-5" ref="providerForm">
                <v-row>
                  <v-col cols="8">
                    <v-text-field :label="$t('id')"
                                  v-model="guidId"
                                  clearable
                                  required
                                  :counter="36"
                                  maxlength="36"
                                  :rules="providerIdRules"
                                  placeholder="00000000-0000-0000-0000-000000000000">
                    </v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-btn color="success"
                           block
                           @click="generateGuidId">
                      {{$t('randomId')}}
                    </v-btn>
                  </v-col>
                </v-row>
                <v-row>
                  <v-text-field v-model="name"
                                :rules="nameRules"
                                :label="$t('name')"
                                :counter="300"
                                maxlength="300"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="type"
                                :rules="typeRules"
                                :label="$t('type')"
                                :counter="100"
                                maxlength="100"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="phone"
                                :rules="phoneRules"
                                :label="$t('phone')"
                                :counter="20"
                                maxlength="20"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="country"
                                :rules="countryRules"
                                :label="$t('country')"
                                :counter="2"
                                maxlength="2"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="state"
                                :rules="stateRules"
                                :label="$t('state')"
                                :counter="2"
                                maxlength="2"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="city"
                                :rules="cityRules"
                                :label="$t('city')"
                                :counter="200"
                                maxlength="200"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="addressLines"
                                :rules="addressLinesRules"
                                :label="$t('address')"
                                :counter="500"
                                maxlength="500"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-text-field v-model="zipCode"
                                :rules="zipCodeRules"
                                :label="$t('zipCode')"
                                :counter="20"
                                maxlength="20"
                                required></v-text-field>
                </v-row>
                <v-row>
                  <v-select :items="organizations"
                            :label="$t('organization')"
                            v-model="organizationId"
                            :loading="loadingOrganizations"
                            :rules="organizationRules"
                            item-text="name"
                            item-value="id" />
                </v-row>
              </v-form>
              <v-card-actions class="pl-0">
                <v-btn color="success"
                       min-width="100"
                       @click="createProvider"
                       :disabled="submitted">
                  {{$t('submit')}}
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-col>
    </v-row><!-- End Create Provider -->
    <v-row v-if="command == 'assignPatientCareManagementProgram'">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-row align="center" class="ml-1">
          <v-col cols="12" sm="6">
            <v-card class="px-5 py-3">
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="text-h5 mb-1">{{$t('assignPatientCareManagementProgram')}}</v-list-item-title>
                </v-list-item-content>
                <v-icon>mdi-playlist-plus</v-icon>
              </v-list-item>
              <v-form class="mt-5" ref="assignPatientCareManagementProgramForm">
                <v-row>
                  <v-select v-if="patients"
                            :items="patients"
                            :label="$t('associatedPatient')"
                            v-model="clinicPatientId"
                            :rules="observationPatientRules"
                            item-text="text"
                            item-value="clinicPatientId"
                            required />
                  <div v-else>
                    <v-select loading
                              :label="$t('loadingPatients')" />
                  </div>
                </v-row>
                <v-row>
                  <v-select :items="careManagementPrograms"
                            :label="$t('careManagementProgram')"
                            v-model="careManagementProgramShortName"
                            :rules="careManagementProgramRules"
                            required
                            :loading="loadingCareManagementPrograms"
                            item-text="name"
                            item-value="shortName" />
                </v-row>
                <v-row>
                  <v-switch v-model="isEnrolled" label="Set Enrollment" />
                </v-row>
              </v-form>
              <v-card-actions class="pl-0">
                <v-btn color="success"
                       min-width="100"
                       @click="assignPatientCareManagementProgram"
                       :disabled="submitted">
                  {{$t('submit')}}
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-col>
    </v-row><!-- End Patient Care Management Programs -->
    <v-row class="mt-4">
      <v-col cols="12"
             md="10"
             class="mx-auto">
        <v-alert type="success" dismissible v-model="showAlert">
          {{ alertMessage }}
        </v-alert>
        <v-alert type="error" dismissible v-model="showErrorAlert">
          {{ alertMessage }}
        </v-alert>
      </v-col>
    </v-row><!-- End Alerts -->
  </v-container>
</template>

<script lang="ts">
  import Vue from 'vue';
  import axiosClient from '@/auth/axiosInstance';
  import {
    CommandTestsClient, PatientAcceptsToSCommand, PatientQueryFilter, RegisterPatientCommand,
    SendPatientOnboardingEmailCommand, EnableMarketingEmailsCommand, DisableMarketingEmailsCommand,
    CreateObservationCommand, ObservationDataItem, UpdateEmailAddressCommand, PatientDto,
    ObservationsClient, PatientObservationDataDto, UpdateObservationCommand,
    TranslationDto, FileParameter, TranslationsClient, CreateOrganizationCommand,
    CreateProviderCommand, SetPatientCareManagementProgramEnrollmentCommand,
  } from '../IUGOCare-api';
  import { mapState, mapGetters } from 'vuex';
  import i18n from '@/i18n';
  import { format } from 'date-fns';
  import {
    exists, isSelected, isFileSelected, isHtmlFile, isValidFileSize, isValidPhoneNumberFormat,
    isValidZipCodeFormat, isDecimal,
  } from '@/validation';
  import { ICombinedMeasurement, IUnit } from '@/shared/interfaces';

  export default Vue.extend({
    name: 'DashboardCommandTests',

    data() {
      return {
        submitted: false,
        currentLanguage: '',
        command: '',
        commands: [],
        patient: null,
        patients: null,
        patientDtos: [],
        inactivePatients: null,
        registerEmail: '',
        translationElement: '',
        email: '',
        emailRules: [
          (v: string) => !v || /^.+@\w+([.-]?\w+)*(\.\w{2,})+$/.test(v) || this.$t('emailFormatInvalid'),
        ],
        requiredEmailRules: [
          (v: string) => !!v || this.$t('emailRequired'),
          (v: string) => /^.+@\w+([.-]?\w+)*(\.\w{2,})+$/.test(v) || this.$t('emailFormatInvalid'),
        ],
        givenName: null,
        middleName: null,
        familyName: null,
        clinicPatientIdRules: [
          (v: string) => !!v || this.$t('clinicPatientIdRequired'),
          (v: string) => !v || /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(v)
            || this.$t('clinicPatientIdInvalidFormat'),
        ],
        clinicSubdomain: 'someClinicSubdomain',
        showAlert: false,
        showErrorAlert: false,
        alertMessage: '',
        clinicPatientId: '',
        indexClicked: -1,
        loading: false,
        fixedHeader: true,

        // Create Observation Command Properties
        observationIdRules: [
          (v: string) => !!v || this.$t('observationIdRequired'),
          (v: string) => !v || /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(v)
            || this.$t('observationIdInvalidFormat'),
        ],
        observationCodeRules: [
          (v: string) => !!v || this.$t('observationTypeRequired'),
        ],
        observationPatientRules: [
          (v: string) => !!v || this.$t('patientRequired'),
        ],
        observationDataNumberRules: [
          (v: string) => !v || (parseFloat(v) <= 1000.00) || this.$t('valueIsTooHigh'),
        ],
        observationDataFloatRules: [
          (v: string) => !v || parseFloat(v) <= 200.00 || this.$t('valueIsTooHigh'),
        ],
        observationDateRules: [
          (v: string) => !!v || this.$t('observationDateRequired'),
        ],
        observationTimeRules: [
          (v: string) => !!v || this.$t('observationTimeRequired'),
        ],
        observationDatePicker: '',
        timeMenu: false,
        dateMenu: false,
        observationTime: '',
        observationTimeFormatted: '',
        observationDatePickerFormatted: '',
        nowDate: new Date().toISOString().slice(0, 10),
        observationId: '',
        observationCode: '',
        observationCodes: [
          { text: this.$t('bloodGlucose'), value: 'blood-glucose' },
          { text: this.$t('bloodPressure'), value: 'blood-pressure' },
          { text: this.$t('bodyTemperature'), value: 'body-temperature' },
          { text: this.$t('heartRate'), value: 'heart-rate' },
          { text: this.$t('oxygenSaturation'), value: 'oxygen-saturation' },
          { text: this.$t('respiratoryRate'), value: 'respiratory-rate' },
          { text: this.$t('weight'), value: 'weight' },
          { text: this.$t('workouts'), value: 'workouts' },
          { text: this.$t('sleep'), value: 'sleep' },
        ],
        bloodPressureDataCodes: [
          { code: 'systolic', unit: 'mmHg', value: '' },
          { code: 'diastolic', unit: 'mmHg', value: '' },
          { code: 'heart-rate', unit: 'bpm', value: '' },
        ],
        weightDataCodes: [
          { code: 'weight', unit: 'lbs', value: '' },
        ],
        oxygenDataCodes: [
          { code: 'oxygen-saturation', unit: 'SpO2', value: '' },
          { code: 'heart-rate', unit: 'bpm', value: '' },
        ],
        bloodGlucoseDataCodesBase: [
          {
            code: 'blood-glucose',
            unit: 'mmol/L',
            units: [
              { value: 'mg/dL', minValue: 0, maxValue: 750 },
              { value: 'mmol/L', minValue: 0, maxValue: 56 },
            ],
            value: '',
            label: this.$t('bloodGlucose').toString(),
          },
          {
            code: 'meal-code-fasting',
            unit: '',
            value: 0,
          },
        ] as ICombinedMeasurement[],
        bodyTemperatureDataCodesBase: [
          {
            code: 'body-temperature',
            unit: '°C',
            units: [
              { value: '°C', minValue: 30, maxValue: 45 },
              { value: '°F', minValue: 86, maxValue: 113 },
            ],
            value: '',
            label: this.$t('bodyTemperature').toString(),
          },
        ] as ICombinedMeasurement[],
        heartRateDataCodes: [
          { code: 'heart-rate', unit: 'bpm', value: '' },
        ],
        respiratoryRateDataCodes: [
          { code: 'respiratory-rate', unit: 'rpm', value: '' },
        ],
        workoutDataCodesBase: [
          {
            code: 'fitness-duration',
            unit: 'secs',
            value: '',
            label: this.$t('duration').toString(),
          },
          {
            code: 'distance',
            unit: 'km',
            units: [
              { value: 'mi', minValue: 0, maxValue: 9999999999999999 / 1.609 },
              { value: 'km', minValue: 0, maxValue: 9999999999999999 },
            ],
            value: '',
            label: this.$t('distance').toString(),
          },
          {
            code: 'steps',
            unit: 'steps',
            value: '',
            label: this.$t('steps').toString(),
          },
          {
            code: 'elevation',
            unit: 'm',
            units: [
              { value: 'ft', minValue: -9999999999999999 / 0.3048, maxValue: 9999999999999999 / 0.3048 },
              { value: 'm', minValue: -9999999999999999, maxValue: 9999999999999999 },
            ],
            value: '',
            label: this.$t('elevation').toString(),
          },
          {
            code: 'floors',
            unit: 'floors',
            value: '',
            label: this.$t('floors').toString(),
          },
          {
            code: 'water-consumed',
            unit: 'L',
            units: [
              { value: 'oz', minValue: 0, maxValue: 676 },
              { value: 'gal', minValue: 0, maxValue: 5 },
              { value: 'mL', minValue: 0, maxValue: 20000 },
              { value: 'L', minValue: 0, maxValue: 20 },
            ],
            value: '',
            label: this.$t('waterConsumed').toString(),
          },
          {
            code: 'calories-burned',
            unit: 'kcal',
            value: '',
            label: this.$t('caloriesBurned').toString(),
          },
          {
            code: 'heart-rate',
            unit: 'bpm',
            value: '',
            label: this.$t('heartRate').toString(),
          },
        ] as ICombinedMeasurement[],
        sleepDataCodes: [
          { code: 'total', unit: 'hh:mm', value: '' },
          { code: 'rem', unit: 'hh:mm', value: '' },
          { code: 'deep', unit: 'hh:mm', value: '' },
          { code: 'light', unit: 'hh:mm', value: '' },
          { code: 'awake', unit: 'hh:mm', value: '' },
        ],
        selectedLanguage: '',
        languages: [
          { text: this.$t('english'), value: 'EN' },
          { text: this.$t('spanish'), value: 'ES' },
        ],
        languageRules: [isSelected],
        decimalRules: [
          (v: string) => v !== '' && /^([0-9]+(\.[0-9]{0,2})?)$/.test(v) || this.$t('invalidEntryEnterAValue'),
        ],
        heartRateValueRules: [
          (v: string) => !v || (parseFloat(v) >= 35.00 && parseFloat(v) <= 500.00) || this.$t('outOfRange', {
            min: 0,
            max: 500,
          }),
        ],
        glucoseReadingValueRules: [
          (v: string) => !v || (parseFloat(v) <= 1000.00) || this.$t('valueIsTooHigh'),
        ],
        oxygenSaturationValueRules: [
          (v: string) => !v || (parseFloat(v) >= 50.00 && parseFloat(v) <= 100.00) || this.$t('outOfRange', {
            min: 50,
            max: 100,
          }),
        ],
        pulseValueRules: [
          (v: string) => !v || (parseFloat(v) <= 1000.00) || this.$t('valueIsTooHigh'),
        ],
        respiratoryRateValueRules: [
          (v: string) => !v || (parseFloat(v) >= 1.00 && parseFloat(v) <= 60.00) || this.$t('outOfRange', {
            min: 1,
            max: 60,
          }),
        ],
        workoutDistanceRules: [
          (v: string) => !v || (parseFloat(v) >= 0.00 && parseFloat(v) <= 500.00) || this.$t('outOfRange', {
            min: 0,
            max: 500,
          }),
        ],
        workoutStepsRules: [
          (v: string) => !v || (parseFloat(v) >= 0.00 && parseFloat(v) <= 100000.00) || this.$t('outOfRange', {
            min: 0,
            max: 100000,
          }),
        ],
        workoutElevationRules: [
          (v: string) => !v || (parseFloat(v) >= 0.00 && parseFloat(v) <= 10.00) || this.$t('outOfRange', {
            min: 0,
            max: 10,
          }),
        ],
        workoutFloorsRules: [
          (v: string) => !v || (parseFloat(v) >= 0.00 && parseFloat(v) <= 1000.00) || this.$t('outOfRange', {
            min: 0,
            max: 1000,
          }),
        ],
        workoutWaterConsumeRules: [
          (v: string) => !v || (parseFloat(v) >= 0.00 && parseFloat(v) <= 20.00) || this.$t('outOfRange', {
            min: 0,
            max: 20,
          }),
        ],
        workoutCaloriesRules: [
          (v: string) => !v || (parseFloat(v) >= 0.00 && parseFloat(v) <= 15000.00) || this.$t('outOfRange', {
            min: 0,
            max: 15000,
          }),
        ],
        workoutHRRules: [
          (v: string) => !v || (parseFloat(v) >= 35.00 && parseFloat(v) <= 500.00) || this.$t('outOfRange', {
            min: 35,
            max: 500,
          }),
        ],
        durationValueRules: [
          (v: string) => !!v || this.$t('invalidEntryEnterAValue'),
          (v: string) => !v || /^([0-1][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$/.test(v) || this.$t('durationFormatError'),
          (v: string) => !v || v !== '00:00:00' || this.$t('invalidEntryEnterAValue'),
        ],
        sleepValueRules: [
          (v: string) => !!v || this.$t('invalidEntryEnterAValue'),
          (v: string) => !v || /^([0-1][0-9]|2[0-3]):[0-5][0-9]$/.test(v) || this.$t('valueMustBeAValidTime'),
        ],
        sleepOptionalFieldValueRules: [
          (v: string) => !v || /^([0-1][0-9]|2[0-3]):[0-5][0-9]$/.test(v) || this.$t('valueMustBeAValidTime'),
        ],
        organizationIdRules: [
          (v: string) => !!v || this.$t('organizationIdRequired'),
          (v: string) => !v || /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(v)
            || this.$t('organizationIdInvalidFormat'),
        ],
        providerIdRules: [
          (v: string) => !!v || this.$t('providerIdRequired'),
          (v: string) => !v || /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(v)
            || this.$t('providerIdInvalidFormat'),
        ],
        // Update Observations
        patientObservations: null,
        patientObservation: null,
        // Translations
        elementRules: [isSelected],
        selectedFile: null,
        htmlFileRules: [isFileSelected, isHtmlFile, isValidFileSize],
        // Organization-Provider-CareManagementPrograms-PatientCarePrograms
        guidId: '',
        name: '',
        phone: '',
        country: '',
        state: '',
        city: '',
        addressLines: '',
        zipCode: '',
        type: '',
        organizationId: '',
        contactName: '',
        relationship: '',
        careManagementProgramShortName: '',
        isEnrolled: false,
        billingProviderId: '',
        primaryCareProviderId: '',
        guidIdRules: [exists],
        nameRules: [exists],
        phoneRules: [exists, isValidPhoneNumberFormat],
        countryRules: [exists],
        stateRules: [exists],
        cityRules: [exists],
        addressLinesRules: [exists],
        zipCodeRules: [exists, isValidZipCodeFormat],
        typeRules: [exists],
        organizationRules: [isSelected],
        contactNameRules: [exists],
        careManagementProgramRules: [isSelected],
        providerRules: [isSelected],
        patientPhoneNumber: '',
        birthDateRules: [exists],
        patientBirthDate: '',
        patientBirthDateFormatted: '',
      };
    },
    props: {
      disabledFields: Boolean,
    },
    methods: {
      formatDate(): string {
        if (!this.observationDatePicker) {
          return null;
        }
        const [year, month, day] = this.observationDatePicker.split('-');
        return new Date(parseInt(year, 10), parseInt(month, 10) - 1, parseInt(day, 10))
          .toLocaleDateString(this.$i18n.locale, { month: 'short', day: 'numeric', year: 'numeric' });
      },
      formatBirthDate(): string {
        if (!this.patientBirthDate) {
          return null;
        }
        const [year, month, day] = this.patientBirthDate.split('-');
        return new Date(parseInt(year, 10), parseInt(month, 10) - 1, parseInt(day, 10))
          .toLocaleDateString(this.$i18n.locale, { month: 'short', day: 'numeric', year: 'numeric' });
      },
      getMaxTime(): string {
        if (this.observationDatePicker !== this.nowDate && this.observationDatePicker !== '') {
          return '';
        } else {
          return format(new Date(), 'HH:mm');
        }
      },
      async getPatientsByFilter(filter: PatientQueryFilter) {
        this.patient = null;
        this.patients = null;
        const ctc = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);
        const vm = await ctc.getPatients(filter);
        this.patientDtos = vm.patients;
        return vm.patients;
      },
      async getInactivePatients() {
        const patients = await this.getPatientsByFilter(PatientQueryFilter.InactiveOnly);
        this.inactivePatients = patients;
        this.patients = patients.map((p) => ({ text: p.emailAddress || `ID: ${p.clinicPatientId}`, value: p.id }));
      },
      async getInactivePatientsWithEmailAddresses() {
        const patients = await this.getPatientsByFilter(PatientQueryFilter.InactiveOnly);
        this.inactivePatients = patients;
        this.patients = patients.filter((p) => Boolean(p.emailAddress))
          .map((p) => ({ text: p.emailAddress || `ID: ${p.clinicPatientId}`, value: p.id }));
      },
      async getAllPatients() {
        const patients = await this.getPatientsByFilter(PatientQueryFilter.AllPatients);
        this.patients = patients.map((p) => ({
          text: p.emailAddress || `ID: ${p.clinicPatientId}`,
          value: p.id, clinicPatientId: p.clinicPatientId,
        }));
      },
      async patientAcceptsToS() {
        if (this.patient) {
          this.submitted = true;
          const command = new PatientAcceptsToSCommand();
          command.patientId = this.patient;
          // For testing process
          command.password = 'Passw0rd';

          const ctc = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);
          await ctc.patientAcceptsToS(command);
          await this.getInactivePatientsWithEmailAddresses();
          this.submitted = false;
        }
      },
      registerPatient() {
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        if (this.$refs.registerPatientForm.validate()) {
          this.submitted = true;
          const command = new RegisterPatientCommand();
          command.clinicPatientId = this.clinicPatientId;
          command.clinicSubdomain = this.clinicSubdomain;
          command.patientTimeZone = 'Etc/UTC';
          command.patientWindowsTimeZone = 'UTC';
          command.clinicId = 'c9b9d356-94ca-4efa-9488-7fdaf36fd898';
          command.emailAddress = this.email;
          command.givenName = this.givenName;
          command.middleName = this.middleName;
          command.familyName = this.familyName;
          command.patientLanguage = this.selectedLanguage;
          command.phoneNumber = this.patientPhoneNumber;
          const [year, month, day] = this.patientBirthDate.split('-');
          const date = new Date(Number(year), Number(month) - 1, Number(day));
          command.birthDate = date;
          command.primaryCareProviderId = this.primaryCareProviderId;

          this.showAlert = false;
          this.showErrorAlert = false;

          const ctc = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);
          ctc.registerPatient(command).then(
            () => {
              this.alertMessage = this.$t('patientRegisteredSuccessfully').toString();
              this.clinicPatientId = '';
              this.email = '';
              this.givenName = '';
              this.middleName = '';
              this.familyName = '';
              this.patientPhoneNumber = '';
              this.patientBirthDate = '';
              this.showAlert = true;
              setTimeout(() => { this.showAlert = false; }, 3000);
              // @ts-ignore - Support for Vuetify form validation for TS set for v3
              this.$refs.registerPatientForm.reset();
              this.submitted = false;
            })
            .catch((ex) => {
              this.alertMessage = 'Error: ';

              if (ex.response.data.errors) {
                const errors = ex.response.data.errors;
                Object.entries(errors).forEach(
                  ([, value]) => this.alertMessage += value);
              }

              this.showErrorAlert = true;
              this.submitted = false;
            });
        }
      },
      uuid() {
        /* eslint-disable */
        let uuidValue = '';
        let k;
        let randomValue;

        for (k = 0; k < 32; k++) {
          randomValue = Math.random() * 16 | 0;

          if (k === 8 || k === 12 || k === 16 || k === 20) {
            uuidValue += '-';
          }
          uuidValue += (k === 12 ? 4 : (k === 16 ? (randomValue & 3 | 8) : randomValue)).toString(16);
        }
        /* eslint-enable */
        return uuidValue;
      },
      async sendPatientOnboardingEmail() {
        if (this.patient) {
          this.submitted = true;
          const command = new SendPatientOnboardingEmailCommand();
          command.patientId = this.patient;
          command.language = i18n.locale;

          const ctc = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);
          await ctc.sendPatientOnboardingEmail(command);

          await this.getInactivePatientsWithEmailAddresses();
          this.submitted = false;
        }
      },
      async enableMarketingEmails() {
        if (this.patient) {
          this.submitted = true;
          const command = new EnableMarketingEmailsCommand();
          command.patientId = this.patient;

          const ctc = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);
          await ctc.enableMarketingEmails(command).then(
            () => {
              this.alertMessage = this.$t('patientEnabledMarketingEmails').toString();
              this.showAlert = true;
              setTimeout(() => { this.showAlert = false; }, 3000);
              this.submitted = false;

            })
            .catch((ex) => {
              this.alertMessage = 'Error: ' + ex.status + ' ' + ex.message;

              if (ex.response.errors) {
                const errors = ex.response.errors;
                errors.forEach((e: any) => this.alertMessage += e);
              }
              this.showErrorAlert = true;
              this.submitted = false;
            });
        }
      },
      async disableMarketingEmails() {
        if (this.patient) {
          this.submitted = true;
          const command = new DisableMarketingEmailsCommand();
          command.patientId = this.patient;

          const ctc = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);
          await ctc.disableMarketingEmails(command).then(
            () => {
              this.alertMessage = this.$t('patientDisabledMarketingEmails').toString();
              this.showAlert = true;
              setTimeout(() => { this.showAlert = false; }, 3000);
              this.submitted = false;

            })
            .catch((ex) => {
              this.alertMessage = 'Error: ' + ex.status + ' ' + ex.message;

              if (ex.response.errors) {
                const errors = ex.response.errors;
                errors.forEach((e: any) => this.alertMessage += e);
              }
              this.showErrorAlert = true;
              this.submitted = false;
            });
        }
      },
      async createObservation() {
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        if (this.$refs.createObservationForm.validate()) {
          this.submitted = true;
          this.showErrorAlert = false;
          this.showAlert = false;
          const command = new CreateObservationCommand();
          command.observationId = this.observationId;
          const [year, month, day] = this.observationDatePicker.split('-');
          const [hours, minutes] = this.observationTime.split(':');
          const date = new Date(Number(year), Number(month) - 1, Number(day), Number(hours), Number(minutes));
          command.effectiveDate = date;
          const patient: PatientDto = this.patientDtos.find((p) => p.id === this.patient);
          command.clinicPatientId = patient.clinicPatientId;
          command.observationCode = this.observationCode;
          const observationDataItems: ObservationDataItem[] = [];

          if (this.observationCode === 'blood-pressure') {
            this.bloodPressureDataCodes.forEach((dataItem) => {
              if (dataItem.value !== '' && !(dataItem.value === undefined)) {
                const item = new ObservationDataItem();
                item.id = this.uuid();
                item.observationCode = dataItem.code;
                item.unit = dataItem.unit;
                item.value = parseFloat(dataItem.value);
                observationDataItems.push(item);
              }
            });
          } else if (this.observationCode === 'weight') {
            this.weightDataCodes.forEach((dataItem) => {
              if (dataItem.value !== '' && !(dataItem.value === undefined)) {
                const item = new ObservationDataItem();
                item.id = this.uuid();
                item.observationCode = dataItem.code;
                item.unit = dataItem.unit;
                item.value = parseFloat(dataItem.value);
                observationDataItems.push(item);
              }
            });
          } else if (this.observationCode === 'oxygen-saturation') {
            this.oxygenDataCodes.forEach((dataItem) => {
              if (dataItem.value !== '' && !(dataItem.value === undefined)) {
                const item = new ObservationDataItem();
                item.id = this.uuid();
                item.observationCode = dataItem.code;
                item.unit = dataItem.unit;
                item.value = parseFloat(dataItem.value);
                observationDataItems.push(item);
              }
            });
          } else if (this.observationCode === 'body-temperature') {
            this.bodyTemperatureDataCodes.forEach((dataItem) => {
              if (dataItem.value !== '' && !(dataItem.value === undefined)) {
                const item = new ObservationDataItem();
                item.id = this.uuid();
                item.observationCode = dataItem.code;
                item.unit = dataItem.unit;
                item.value = parseFloat(dataItem.value);
                observationDataItems.push(item);
              }
            });
          } else if (this.observationCode === 'heart-rate') {
            this.heartRateDataCodes.forEach((dataItem) => {
              if (dataItem.value !== '' && !(dataItem.value === undefined)) {
                const item = new ObservationDataItem();
                item.id = this.uuid();
                item.observationCode = dataItem.code;
                item.unit = dataItem.unit;
                item.value = parseFloat(dataItem.value);
                observationDataItems.push(item);
              }
            });
          } else if (this.observationCode === 'blood-glucose') {
            this.bloodGlucoseDataCodes.forEach((dataItem) => {
              if (dataItem.value !== '' && !(dataItem.value === undefined)) {
                const item = new ObservationDataItem();
                item.id = this.uuid();
                item.observationCode = dataItem.code;
                item.unit = dataItem.unit;
                item.value = parseFloat(dataItem.value);
                observationDataItems.push(item);
              }
            });
          } else if (this.observationCode === 'respiratory-rate') {
            this.respiratoryRateDataCodes.forEach((dataItem) => {
              if (dataItem.value !== '' && !(dataItem.value === undefined)) {
                const item = new ObservationDataItem();
                item.id = this.uuid();
                item.observationCode = dataItem.code;
                item.unit = dataItem.unit;
                item.value = parseFloat(dataItem.value);
                observationDataItems.push(item);
              }
            });
          } else if (this.observationCode === 'workouts') {
            this.workoutDataCodes.forEach((measurementItem) => {
              if (measurementItem) {
                const item = new ObservationDataItem();
                item.id = this.uuid();
                item.observationCode = measurementItem.code;
                item.unit = measurementItem.unit;
                if (item.observationCode === 'fitness-duration') {
                  const v = measurementItem.value.split(':');
                  item.value = (+v[0]) * 60 * 60 + (+v[1]) * 60 + (+v[2]);
                } else if (parseFloat(measurementItem.value) !== undefined &&
                  parseFloat(measurementItem.value) !== null &&
                  parseFloat(measurementItem.value) > 0) {
                  item.value = parseFloat(measurementItem.value);
                }
                observationDataItems.push(item);
              }
            });
          } else if (this.observationCode === 'sleep') {
            this.sleepDataCodes.forEach((dataItem) => {
              if (dataItem.value !== '' && !(dataItem.value === undefined)) {
                const item = new ObservationDataItem();
                const time = dataItem.value.split(':');
                item.id = this.uuid();
                item.observationCode = dataItem.code;
                item.unit = dataItem.unit;
                item.value = ((+time[0]) * 60 + (+time[1])) * 60;
                observationDataItems.push(item);
              }
            });
          }

          command.observationDataList = observationDataItems;

          const ctc = new CommandTestsClient('', axiosClient);
          await ctc.createObservation(command).then(
            (response) => {
              this.alertMessage = this.$t('observationCreatedWithId').toString() + response;
              this.observationDatePicker = new Date().toISOString().substr(0, 10);
              this.showAlert = true;
              setTimeout(() => { this.showAlert = false; }, 3000);

              // @ts-ignore - Support for Vuetify form validation for TS set for v3
              this.$refs.createObservationForm.reset();
              this.observationDatePicker = '';
              this.observationDatePickerFormatted = '';
              this.observationTimeFormatted = '';
              this.observationTime = null;
              this.submitted = false;
            })
            .catch((ex) => {
              this.alertMessage = 'Error: ';

              if (ex.response.data.errors) {
                const errors = ex.response.data.errors;
                Object.entries(errors).forEach(
                  ([, value]) => this.alertMessage += value);
              } else {
                this.alertMessage += ex;
              }

              this.submitted = false;
              this.showErrorAlert = true;
            });
        }
      },
      generateObservationUuid() {
        this.observationId = this.uuid();
      },
      resetObservationForm() {
        this.observationId = null;
        this.observationCode = null;
        this.observationDatePicker = null;
        this.observationTime = null;
        this.workoutDataCodes = [
          {
            code: 'fitness-duration',
            unit: 'secs',
            value: '',
            label: this.$t('duration').toString(),
          },
          {
            code: 'distance',
            unit: 'km',
            units: [
              { value: 'mi', minValue: 0 / 1.609, maxValue: 9999999999999999 / 1.609 },
              { value: 'km', minValue: 0, maxValue: 9999999999999999 },
            ],
            value: '',
            label: this.$t('distance').toString(),
          },
          {
            code: 'steps',
            unit: 'steps',
            value: '',
            label: this.$t('steps').toString(),
          },
          {
            code: 'elevation',
            unit: 'm',
            units: [
              { value: 'ft', minValue: -9999999999999999 / 0.3048, maxValue: 9999999999999999 / 0.3048 },
              { value: 'm', minValue: -9999999999999999, maxValue: 9999999999999999 },
            ],
            value: '',
            label: this.$t('elevation').toString(),
          },
          {
            code: 'floors',
            unit: 'floors',
            value: '',
            label: this.$t('floors').toString(),
          },
          {
            code: 'water-consumed',
            unit: 'L',
            units: [
              { value: 'oz', minValue: 0, maxValue: 676 },
              { value: 'gal', minValue: 0, maxValue: 5 },
              { value: 'mL', minValue: 0, maxValue: 20000 },
              { value: 'L', minValue: 0, maxValue: 20 },
            ],
            value: '',
            label: this.$t('waterConsumed').toString(),
          },
          {
            code: 'calories-burned',
            unit: 'kcal',
            value: '',
            label: this.$t('caloriesBurned').toString(),
          },
          {
            code: 'heart-rate',
            unit: 'bpm',
            value: '',
            label: this.$t('heartRate').toString(),
          },
        ];
        this.sleepDataCodes = [
          { code: 'total', unit: 'hh:mm', value: '' },
          { code: 'rem', unit: 'hh:mm', value: '' },
          { code: 'deep', unit: 'hh:mm', value: '' },
          { code: 'light', unit: 'hh:mm', value: '' },
          { code: 'awake', unit: 'hh:mm', value: '' },
        ];
        this.resetObservationsForms();
        this.observationDatePicker = this.nowDate;
      },
      loader(index: any, item: any) {
        this.indexClicked = index;
        this.loading = !this.loading;
        setTimeout(() => {
          this.loading = false;
          this.$router.push({ name: 'activate-account', params: { code: item.activationCode } });
        }
          , 500);
      },
      updateEmail() {
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        if (this.$refs.updateemailform.validate()) {
          this.submitted = true;
          const command = new UpdateEmailAddressCommand();
          command.patientId = this.patient;
          command.emailAddress = this.email;

          this.showAlert = false;
          this.showErrorAlert = false;

          const ctc = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);
          ctc.updateEmailAddress(command).then(
            () => {
              this.alertMessage = this.$t('emailAddressUpdatedSuccessfully').toString();
              this.email = '';
              this.showAlert = true;
              setTimeout(() => { this.showAlert = false; }, 3000);
              this.getInactivePatients();
              this.submitted = false;
            })
            .catch((ex) => {
              this.alertMessage = 'Error: ';

              if (ex.response.data.errors) {
                const errors = ex.response.data.errors;
                Object.entries(errors).forEach(
                  ([, value]) => this.alertMessage += ` ${value}`);
              } else {
                this.alertMessage += ex.response.data.title;
              }
              this.showErrorAlert = true;
              this.submitted = false;

            });
        }
      },
      async loadPatientObservations() {
        const endDate = new Date();
        const startDate = new Date();
        startDate.setMonth(startDate.getMonth() - 1);
        this.patientObservations = null;

        const ctc = new ObservationsClient(process.env.VUE_APP_API_URL, axiosClient);
        const vm = await ctc.getPatientObservations(this.currentPatientId, ['workouts', 'sleep'], startDate, endDate);
        this.patientObservations = vm.observations;
      },
      resetUpdateObservationForm() {
        this.observationCode = null;
        this.patientObservation = null;
        this.resetObservationsForms();
        this.observationDatePicker = this.nowDate;
      },
      secondsToHms(seconds: number, includeSeconds: boolean): string {
        const h = Math.floor(seconds / 3600);
        const m = Math.floor(seconds % 3600 / 60);
        const s = Math.floor(seconds % 3600 % 60);
        const hDisplay = h > 0 ? (h < 10 ? `0${h}:` : `${h}:`) : '00:';
        const mDisplay = m > 0 ? (m < 10 ? `0${m}` : `${m}`) : '00';
        const sDisplay = s > 0 ? (s < 10 ? `:0${s}` : `:${s}`) : ':00';
        return `${hDisplay}${mDisplay}${includeSeconds ? sDisplay : ''}`;
      },
      async updateObservation() {
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        if (this.$refs.updateObservationsForm.validate()) {
          this.showErrorAlert = false;
          this.showAlert = false;
          this.submitted = true;

          const command = new UpdateObservationCommand();
          command.observationId = this.patientObservation.id;
          const [year, month, day] = this.observationDatePicker.split('-');
          const [hours, minutes] = this.observationTime.split(':');
          const date = new Date(Number(year), Number(month) - 1, Number(day), Number(hours), Number(minutes));
          command.effectiveDate = date;
          command.observationCode = this.observationCode;
          const observationDataItems: ObservationDataItem[] = [];

          if (this.observationCode === 'workouts') {
            this.workoutDataCodes.forEach((measurementItem) => {
              if (measurementItem) {
                const item = new ObservationDataItem();
                item.id = this.uuid();
                item.observationCode = measurementItem.code;
                item.unit = measurementItem.unit;
                if (item.observationCode === 'fitness-duration') {
                  const v = measurementItem.value.split(':');
                  item.value = (+v[0]) * 60 * 60 + (+v[1]) * 60 + (+v[2]);
                } else if (parseFloat(measurementItem.value) !== undefined &&
                  parseFloat(measurementItem.value) !== null &&
                  parseFloat(measurementItem.value) > 0) {
                  item.value = parseFloat(measurementItem.value.toString());
                }
                observationDataItems.push(item);
              }
            });
          } else if (this.observationCode === 'sleep') {
            this.sleepDataCodes.forEach((dataItem) => {
              if (dataItem.value !== '' && !(dataItem.value === undefined)) {
                const item = new ObservationDataItem();
                const time = dataItem.value.split(':');
                item.id = this.uuid();
                item.observationCode = dataItem.code;
                item.unit = dataItem.unit;
                item.value = ((+time[0]) * 60 + (+time[1])) * 60;
                observationDataItems.push(item);
              }
            });
          }

          command.observationDataList = observationDataItems;

          const ctc = new CommandTestsClient('', axiosClient);
          await ctc.updateObservation(command).then(
            () => {
              this.alertMessage = this.$t('observationUpdated').toString();
              this.observationDatePicker = new Date().toISOString().substr(0, 10);
              this.showAlert = true;
              setTimeout(() => { this.showAlert = false; }, 3000);

              // @ts-ignore - Support for Vuetify form validation for TS set for v3
              this.$refs.updateObservationsForm.reset();
              this.observationDatePicker = '';
              this.observationDatePickerFormatted = '';
              this.observationTimeFormatted = '';
              this.observationTime = null;
              this.loadPatientObservations();
              this.submitted = false;
            })
            .catch((ex: any) => {
              this.alertMessage = 'Error: ';

              if (ex.response.data.errors) {
                const errors = ex.response.data.errors;
                Object.entries(errors).forEach(
                  ([, value]) => this.alertMessage += ` ${value}`);
              } else {
                this.alertMessage += ex.response.data.title;
              }

              this.submitted = false;
              this.showErrorAlert = true;
            });
        }
      },
      updateHtmlFile() {
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        if (this.$refs.translationElementForm.validate()) {
          const fileToUpload: FileParameter = { fileName: this.selectedFile.name, data: this.selectedFile };
          this.showAlert = false;
          this.showErrorAlert = false;
          this.submitted = true;

          const tc = new TranslationsClient(process.env.VUE_APP_API_URL, axiosClient);
          tc.updateTranslationFile(this.translationElement, this.selectedLanguage, fileToUpload).then(
            () => {
              this.alertMessage = this.$t('fileUploadedSuccessfully').toString();
              this.showAlert = true;
              setTimeout(() => { this.showAlert = false; }, 3000);
              // @ts-ignore - Support for Vuetify form validation for TS set for v3
              this.$refs.translationElementForm.reset();
              this.submitted = false;
            })
            .catch((ex: any) => {
              this.alertMessage = 'Error: ';
              if (ex.response.data.errors) {
                const errors = ex.response.data.errors;
                Object.entries(errors).forEach(
                  ([, value]) => this.alertMessage += value);
              }
              this.submitted = false;
              this.showErrorAlert = true;
            });
        }
      },
      async setWorkoutDataCodesAfterUnitChange(dataCodes: ICombinedMeasurement[]) {
        this.workoutDataCodesBase = dataCodes;
      },
      async setBloodGlucoseDataCodesAfterUnitChange(dataCodes: ICombinedMeasurement[]) {
        this.bloodGlucoseDataCodesBase = dataCodes;
      },
      async setBodyTemperatureDataCodesAfterUnitChange(dataCodes: ICombinedMeasurement[]) {
        this.bodyTemperatureDataCodesBase = dataCodes;
      },
      validateBloodGlucoseObservationsFormsAfterLanguageChange() {
        if (this.$refs.createObservationForm && this.currentLanguage !== localStorage.getItem('language')) {
          this.currentLanguage = localStorage.getItem('language');
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['blood-glucose'].validate();
        }
      },
      validateBodyTemperatureObservationsFormsAfterLanguageChange() {
        if (this.$refs.createObservationForm && this.currentLanguage !== localStorage.getItem('language')) {
          this.currentLanguage = localStorage.getItem('language');
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['body-temperature'].validate();
        }
      },
      validateWorkoutObservationsFormsAfterLanguageChange() {
        if (this.$refs.createObservationForm && this.currentLanguage !== localStorage.getItem('language')) {
          this.currentLanguage = localStorage.getItem('language');
          this.workoutDataCodes?.forEach((m: ICombinedMeasurement) =>
            // @ts-ignore - Support for Vuetify form validation for TS set for v3
            this.$refs[m.code].validate());
        }

        if (this.$refs.updateObservationsForm && this.currentLanguage !== localStorage.getItem('language')) {
          this.currentLanguage = localStorage.getItem('language');
          this.workoutDataCodes?.forEach((m: ICombinedMeasurement) =>
            // @ts-ignore - Support for Vuetify form validation for TS set for v3
            this.$refs[m.code].validate());
        }
      },
      validateWorkoutObservationsAfterUpdate() {
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        if (this.$refs.updateObservationsForm && this.command === 'UpdateObservation') {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.updateObservationsForm.validate();
        } else if (this.$refs.createObservationForm && this.command === 'CreateObservation') {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.workoutDataCodes?.forEach((m: ICombinedMeasurement) =>
            // @ts-ignore - Support for Vuetify form validation for TS set for v3
            this.$refs[m.code].validate());
        }
      },
      clearObservationsFormFields() {
        if (this.$refs['blood-glucose']) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['blood-glucose'].resetValidation();
          this.bloodGlucoseDataCodes[0].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['blood-glucose'].clearableCallback();
        }

        if (this.$refs.systolic) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.systolic.resetValidation();
          this.bloodPressureDataCodes[0].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.systolic.clearableCallback();
        }

        if (this.$refs.diastolic) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.diastolic.resetValidation();
          this.bloodPressureDataCodes[1].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.diastolic.clearableCallback();
        }
        if (this.$refs.pulseBP) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.pulseBP.resetValidation();
          this.bloodPressureDataCodes[2].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.pulseBP.clearableCallback();
        }

        if (this.$refs['body-temperature']) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['body-temperature'].resetValidation();
          this.bodyTemperatureDataCodes[0].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['body-temperature'].clearableCallback();
        }
        if (this.$refs['heart-rate']) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['heart-rate'].resetValidation();
          this.heartRateDataCodes[0].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['heart-rate'].clearableCallback();
        }
        if (this.$refs['oxygen-saturation']) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['oxygen-saturation'].resetValidation();
          this.oxygenDataCodes[0].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['oxygen-saturation'].clearableCallback();
        }

        if (this.$refs.pulseOS) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.pulseOS.resetValidation();
          this.oxygenDataCodes[1].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.pulseOS.clearableCallback();
        }

        if (this.$refs['respiratory-rate']) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['respiratory-rate'].resetValidation();
          this.respiratoryRateDataCodes[0].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['respiratory-rate'].clearableCallback();
        }

        if (this.$refs.weight) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.weight.resetValidation();
          this.weightDataCodes[0].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.weight.clearableCallback();
        }

        if (this.$refs.duration) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.duration.resetValidation();
          this.workoutDataCodes[0].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.duration.clearableCallback();
        }

        if (this.$refs.distance) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.distance.resetValidation();
          this.workoutDataCodes[1].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.distance.clearableCallback();
        }

        if (this.$refs.steps) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.steps.resetValidation();
          this.workoutDataCodes[2].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.steps.clearableCallback();
        }

        if (this.$refs.elevation) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.elevation.resetValidation();
          this.workoutDataCodes[3].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.elevation.clearableCallback();
        }

        if (this.$refs.floors) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.floors.resetValidation();
          this.workoutDataCodes[4].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.floors.clearableCallback();
        }

        if (this.$refs['water-consumed']) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['water-consumed'].resetValidation();
          this.workoutDataCodes[5].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['water-consumed'].clearableCallback();
        }

        if (this.$refs['calories-burned']) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['calories-burned'].resetValidation();
          this.workoutDataCodes[6].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['calories-burned'].clearableCallback();
        }

        if (this.$refs['heart-rateW']) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['heart-rateW'].resetValidation();
          this.workoutDataCodes[7].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['heart-rateW'].clearableCallback();
        }

        if (this.$refs.total) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.total.resetValidation();
          this.sleepDataCodes[0].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.total.clearableCallback();
        }

        if (this.$refs.rem) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.rem.resetValidation();
          this.sleepDataCodes[1].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.rem.clearableCallback();
        }
        if (this.$refs.deep) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.deep.resetValidation();
          this.sleepDataCodes[2].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.deep.clearableCallback();
        }

        if (this.$refs.light) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.light.resetValidation();
          this.sleepDataCodes[3].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.light.clearableCallback();
        }

        if (this.$refs.awake) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.awake.resetValidation();
          this.sleepDataCodes[4].value = '';
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.awake.clearableCallback();
        }
      },
      resetObservationsForms() {
        if (this.$refs.createObservationForm) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.createObservationForm.reset();
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.createObservationForm.resetValidation();
        }

        if (this.$refs.updateObservationsForm) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.updateObservationsForm.reset();
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs.updateObservationsForm.resetValidation();
        }
      },
      getCommandsList() {
        return [
          { text: '', value: '' },
          { text: this.$t('patientAcceptsToS'), value: 'AcceptToS' },
          { text: this.$t('registerPatient'), value: 'Register Patient' },
          { text: this.$t('sendPatientOnboardingEmail'), value: 'SendPatientOnboardingEmail' },
          { text: this.$t('enableDisableMarketingEmails'), value: 'MarketingEmails' },
          { text: this.$t('inactivePatients'), value: 'Inactive Patients' },
          { text: this.$t('createObservation'), value: 'CreateObservation' },
          { text: this.$t('updateObservations'), value: 'UpdateObservation' },
          { text: this.$t('updateEmail'), value: 'Update Email' },
          { text: this.$t('substituteHtmlTranslation'), value: 'substituteHtmlTranslation' },
          { text: this.$t('createOrganization'), value: 'organization' },
          { text: this.$t('createProvider'), value: 'provider' },
          { text: this.$t('assignPatientCareManagementProgram'), value: 'assignPatientCareManagementProgram' },
        ];
      },
      generateGuidId(): void {
        this.guidId = this.uuid();
      },
      generateClinicPatientId(): void {
        this.clinicPatientId = this.uuid();
      },
      createOrganization(): void {
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        if (this.$refs.organizationForm.validate()) {
          this.submitted = true;
          this.showAlert = false;
          this.showErrorAlert = false;

          const command = new CreateOrganizationCommand();
          command.id = this.guidId;
          command.name = this.name;
          command.phone = this.phone;
          command.country = this.country;
          command.state = this.state;
          command.city = this.city;
          command.addressLines = this.addressLines;
          command.zipCode = this.zipCode;

          const client = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);
          client.createOrganization(command).then(
            () => {
              this.alertMessage = this.$t('informationSavedSuccessfully').toString();
              this.showAlert = true;
              setTimeout(() => { this.showAlert = false; }, 3000);
              // @ts-ignore - Support for Vuetify form validation for TS set for v3
              this.$refs.organizationForm.reset();
              this.submitted = false;
            })
            .catch((ex: any) => {
              this.alertMessage = 'Error: ';
              if (ex.response.data.errors) {
                const errors = ex.response.data.errors;
                Object.entries(errors).forEach(
                  ([, value]) => this.alertMessage += value);
              }
              this.submitted = false;
              this.showErrorAlert = true;
            });
        }
      },
      createProvider(): void {
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        if (this.$refs.providerForm.validate()) {
          this.submitted = true;
          this.showAlert = false;
          this.showErrorAlert = false;

          const command = new CreateProviderCommand();
          command.id = this.guidId;
          command.name = this.name;
          command.type = this.type;
          command.phone = this.phone;
          command.country = this.country;
          command.state = this.state;
          command.city = this.city;
          command.addressLines = this.addressLines;
          command.zipCode = this.zipCode;
          command.organizationId = this.organizationId;

          const client = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);
          client.createProvider(command).then(
            () => {
              this.alertMessage = this.$t('informationSavedSuccessfully').toString();
              this.showAlert = true;
              setTimeout(() => { this.showAlert = false; }, 3000);
              // @ts-ignore - Support for Vuetify form validation for TS set for v3
              this.$refs.providerForm.reset();
              this.submitted = false;
            })
            .catch((ex: any) => {
              this.alertMessage = 'Error: ';
              if (ex.response.data.errors) {
                const errors = ex.response.data.errors;
                Object.entries(errors).forEach(
                  ([, value]) => this.alertMessage += value);
              }
              this.submitted = false;
              this.showErrorAlert = true;
            });
        }
      },
      assignPatientCareManagementProgram(): void {
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        if (this.$refs.assignPatientCareManagementProgramForm.validate()) {
          this.showAlert = false;
          this.showErrorAlert = false;
          this.submitted = true;

          const command = new SetPatientCareManagementProgramEnrollmentCommand();
          command.clinicPatientId = this.clinicPatientId;
          command.careProgramShortName = this.careManagementProgramShortName;
          command.isEnrolled = this.isEnrolled;

          const client = new CommandTestsClient(process.env.VUE_APP_API_URL, axiosClient);
          client.setPatientCareMangementProgramEnrollment(command).then(
            () => {
              this.alertMessage = this.$t('informationSavedSuccessfully').toString();
              this.showAlert = true;
              setTimeout(() => { this.showAlert = false; }, 3000);
              // @ts-ignore - Support for Vuetify form validation for TS set for v3
              this.$refs.assignPatientCareManagementProgramForm.reset();
              this.submitted = false;
            })
            .catch((ex: any) => {
              this.alertMessage = 'Error: ';
              if (ex.response.data.errors) {
                const errors = ex.response.data.errors;
                Object.entries(errors).forEach(
                  ([, value]) => this.alertMessage += value);
              }
              this.showErrorAlert = true;
              this.submitted = false;
            });
        }
      },
      getSingleMeasurement(measurementCode: string, observationCode: string): ICombinedMeasurement {
        switch (observationCode) {
          case 'workouts':
            return this.workoutDataCodes?.find((m: ICombinedMeasurement) =>
              m.code === measurementCode);
          case 'blood-glucose':
            return this.bloodGlucoseDataCodes?.find((m: ICombinedMeasurement) =>
              m.code === measurementCode);
          case 'body-temperature':
            return this.bodyTemperatureDataCodes?.find((m: ICombinedMeasurement) =>
              m.code === measurementCode);
        }
      },
      getMeasurementUnit(measurementCode: string, observationCode: string): IUnit {
        const measurement = this.getSingleMeasurement(measurementCode, observationCode);
        return measurement?.units?.find((u: IUnit) => u.value === measurement.unit) || null;
      },
      validateRange(value: number, measurementCode: string, observationCode: string): boolean {
        const unit = this.getMeasurementUnit(measurementCode, observationCode);
        if (unit) {
          return value >= unit.minValue && value <= unit.maxValue;
        }
        return true;
      },
      getRangeText(measurementCode: string, observationCode: string): string {
        const unit = this.getMeasurementUnit(measurementCode, observationCode);
        if (unit) {
          return this.$t('outOfRange', { min: unit.minValue, max: unit.maxValue }).toString();
        }
        return '';
      },
      mapPatientObservationData() {
        this.patientObservation.observationsData.forEach((od: PatientObservationDataDto) => {
          const unit = od.unit;
          const value = od.value ? String(od.value) : '';
          switch (od.observationCode) {
            case 'fitness-duration':
              this.workoutDataCodes[0].unit = unit;
              this.workoutDataCodes[0].value = value ? this.secondsToHms(Number(value), true) : '';
              break;
            case 'distance':
              this.workoutDataCodes[1].unit = unit;
              this.workoutDataCodes[1].value = value;
              break;
            case 'steps':
              this.workoutDataCodes[2].unit = unit;
              this.workoutDataCodes[2].value = value;
              break;
            case 'elevation':
              this.workoutDataCodes[3].unit = unit;
              this.workoutDataCodes[3].value = value;
              break;
            case 'floors':
              this.workoutDataCodes[4].unit = unit;
              this.workoutDataCodes[4].value = value;
              break;
            case 'water-consumed':
              this.workoutDataCodes[5].unit = unit;
              this.workoutDataCodes[5].value = value;
              break;
            case 'calories-burned':
              this.workoutDataCodes[6].unit = unit;
              this.workoutDataCodes[6].value = value;
              break;
            case 'heart-rate':
              this.workoutDataCodes[7].unit = unit;
              this.workoutDataCodes[7].value = value;
              break;
            case 'total':
              this.sleepDataCodes[0].unit = unit;
              this.sleepDataCodes[0].value = value ? this.secondsToHms(Number(value), false) : '';
              break;
            case 'rem':
              this.sleepDataCodes[1].unit = unit;
              this.sleepDataCodes[1].value = value ? this.secondsToHms(Number(value), false) : '';
              break;
            case 'deep':
              this.sleepDataCodes[2].unit = unit;
              this.sleepDataCodes[2].value = value ? this.secondsToHms(Number(value), false) : '';
              break;
            case 'light':
              this.sleepDataCodes[3].unit = unit;
              this.sleepDataCodes[3].value = value ? this.secondsToHms(Number(value), false) : '';
              break;
            case 'awake':
              this.sleepDataCodes[4].unit = unit;
              this.sleepDataCodes[4].value = value ? this.secondsToHms(Number(value), false) : '';
              break;
          }
          this.workoutDataCodesBase = this.workoutDataCodes;
        });
      },
    },
    beforeCreate() {
      const language = localStorage.getItem('language');
      localStorage.setItem('language', language !== 'undefined' ? language : 'en');
    },
    created() {
      this.$i18n.locale = localStorage.getItem('language');
      this.currentLanguage = localStorage.getItem('language');
      this.commands = this.getCommandsList();
    },
    mounted() {
      this.$i18n.locale = localStorage.getItem('language');
      this.currentLanguage = localStorage.getItem('language');
      this.commands = this.getCommandsList();
    },
    watch: {
      workoutDataCodes() {
        this.validateWorkoutObservationsAfterUpdate();
      },
      bloodGlucoseDataCodes() {
        if (this.$refs['blood-glucose']) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['blood-glucose'].validate();
        }
      },
      bodyTemperatureDataCodes() {
        if (this.$refs['body-temperature']) {
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          this.$refs['body-temperature'].validate();
        }
      },
      command() {
        this.showAlert = false;
        this.showErrorAlert = false;
        switch (this.command) {
          case 'AcceptToS':
            this.getInactivePatientsWithEmailAddresses();
            break;
          case 'Register Patient':
            setTimeout(() => {
              // @ts-ignore - Support for Vuetify form validation for TS set for v3
              this.$refs.registerPatientForm.reset();
              this.$store.dispatch('providers/fetchProviders');
            }, 50);
            this.registerEmail = '';
            break;
          case 'SendPatientOnboardingEmail':
            this.getInactivePatientsWithEmailAddresses();
            break;
          case 'MarketingEmails':
            this.getAllPatients();
            break;
          case 'Inactive Patients':
            this.getInactivePatients();
            break;
          case 'CreateObservation':
            this.getAllPatients();
            this.resetObservationForm();
            break;
          case 'Update Email':
            this.getInactivePatients();
            break;
          case 'UpdateObservation':
            this.loadPatientObservations();
            this.resetUpdateObservationForm();
            break;
          case 'substituteHtmlTranslation':
            this.$store.dispatch('translations/fetchTranslationElements');
            break;
          case 'organization':
            // @ts-ignore - Support for Vuetify form validation for TS set for v3
            setTimeout(() => { this.$refs.organizationForm.reset(); }, 100);
            break;
          case 'provider':
            setTimeout(() => {
              // @ts-ignore - Support for Vuetify form validation for TS set for v3
              this.$refs.providerForm.reset();
              this.$store.dispatch('organizations/fetchOrganizations');
            }, 100);
            break;
          case 'assignPatientCareManagementProgram':
            this.getAllPatients();
            this.$store.dispatch('providers/fetchProviders');
            this.$store.dispatch('careManagementPrograms/fetchCareManagementPrograms');
            // @ts-ignore - Support for Vuetify form validation for TS set for v3
            setTimeout(() => { this.$refs.assignPatientCareManagementProgramForm.reset(); }, 100);
            break;
        }
      },
      observationDatePicker() {
        this.observationDatePickerFormatted = this.formatDate();
        this.observationTimeFormatted = '';
      },
      observationTime() {
        if (!this.observationTime) {
          this.observationTimeFormatted = null;
          return;
        }
        const [hour, minute] = this.observationTime.split(':');
        const tempDate = new Date();
        tempDate.setHours(Number(hour));
        tempDate.setMinutes(Number(minute));
        tempDate.setSeconds(0);
        this.observationTimeFormatted = format(tempDate, 'hh:mm a');
      },
      patientObservation() {
        this.observationCode = null;
        this.observationDatePicker = null;
        this.observationTime = null;
        this.sleepDataCodes = [
          { code: 'total', unit: 'hh:mm', value: '' },
          { code: 'rem', unit: 'hh:mm', value: '' },
          { code: 'deep', unit: 'hh:mm', value: '' },
          { code: 'light', unit: 'hh:mm', value: '' },
          { code: 'awake', unit: 'hh:mm', value: '' },
        ];

        if (this.patientObservation) {
          this.observationCode = this.patientObservation.observationCode;
          const eDate = this.patientObservation.effectiveDate;
          this.observationDatePicker = `${eDate.getFullYear()}-${eDate.getMonth() + 1}-${eDate.getDate()}`;
          this.observationTime = `${eDate.getHours() < 10 ? '0' : ''}${eDate.getHours()}:${eDate.getMinutes() < 10 ? '0' : ''}${eDate.getMinutes()}`;

          if (this.patientObservation.observationsData && this.patientObservation.observationsData.length > 0) {

            this.mapPatientObservationData();
          }
        }
      },
      patientBirthDate() {
        this.patientBirthDateFormatted = this.formatBirthDate();
      },
    },
    computed: {
      ...mapState('translations', ['translationElements', 'loadingElements']),
      ...mapState('organizations', ['organizations', 'loadingOrganizations']),
      ...mapState('providers', ['providers', 'loadingProviders']),
      ...mapState('careManagementPrograms', ['careManagementPrograms', 'loadingCareManagementPrograms']),
      ...mapGetters('settings', ['currentPatientId']),
      getElements(): any {
        return this.translationElements.map((t: TranslationDto) =>
          ({
            text: this.$t(t.elementName),
            value: t.elementName,
          }));
      },
      getBloodGlucoseRules(): any[] {
        const rangeFn = (v: number) => !v || this.validateRange(v, 'blood-glucose', 'blood-glucose') ||
          this.getRangeText('blood-glucose', 'blood-glucose');
        return [exists, isDecimal(2), rangeFn];
      },
      getBodyTemperatureRules(): any[] {
        const rangeFn = (v: number) => !v || this.validateRange(v, 'body-temperature', 'body-temperature') ||
          this.getRangeText('body-temperature', 'body-temperature');
        return [exists, isDecimal(2), rangeFn];
      },
      getDistanceRules(): any[] {
        const rangeFn = (v: number) => !v || this.validateRange(v, 'distance', 'workouts') ||
          this.getRangeText('distance', 'workouts');
        return [exists, isDecimal(2), rangeFn];
      },
      getElevationRules(): any[] {
        const rangeFn = (v: number) => !v || this.validateRange(v, 'elevation', 'workouts') ||
          this.getRangeText('elevation', 'workouts');
        return [exists, isDecimal(2), rangeFn];
      },
      getWaterConsumedRules(): any[] {
        const rangeFn = (v: number) => !v || this.validateRange(v, 'water-consumed', 'workouts') ||
          this.getRangeText('water-consumed', 'workouts');
        return [exists, isDecimal(2), rangeFn];
      },

      bloodGlucoseDataCodes: {
        get(): ICombinedMeasurement[] {
          this.validateBloodGlucoseObservationsFormsAfterLanguageChange();
          const newBloodGlucoseData = [
            {
              code: 'blood-glucose',
              unit: this.bloodGlucoseDataCodesBase[0].unit,
              units: [
                { value: 'mg/dL', minValue: 0, maxValue: 750 },
                { value: 'mmol/L', minValue: 0, maxValue: 56 },
              ],
              value: this.bloodGlucoseDataCodesBase[0].value,
              label: this.$t('bloodGlucose').toString(),
            },
            {
              code: this.bloodGlucoseDataCodesBase[1].code,
              unit: '',
              value: 0,
            },
          ] as ICombinedMeasurement[];
          return newBloodGlucoseData;
        },
        set(newBloodGlucoseData: ICombinedMeasurement[]) {
          this.validateBloodGlucoseObservationsFormsAfterLanguageChange();
          this.bloodGlucoseDataCodesBase = newBloodGlucoseData;
        },
      },
      bodyTemperatureDataCodes: {
        get(): ICombinedMeasurement[] {
          this.validateBodyTemperatureObservationsFormsAfterLanguageChange();
          const newBodyTemperatureData = [
            {
              code: 'body-temperature',
              unit: this.bodyTemperatureDataCodesBase[0].unit,
              units: [
                { value: '°C', minValue: 30, maxValue: 45 },
                { value: '°F', minValue: 86, maxValue: 113 },
              ],
              value: this.bodyTemperatureDataCodesBase[0].value,
              label: this.$t('bodyTemperature').toString(),
            },
          ] as ICombinedMeasurement[];
          return newBodyTemperatureData;
        },
        set(newBodyTemperatureData: ICombinedMeasurement[]) {
          this.validateBodyTemperatureObservationsFormsAfterLanguageChange();
          this.bodyTemperatureDataCodesBase = newBodyTemperatureData;
        },
      },
      workoutDataCodes: {
        get(): ICombinedMeasurement[] {
          this.validateWorkoutObservationsFormsAfterLanguageChange();
          const newWorkoutData = [
            {
              code: 'fitness-duration',
              unit: 'secs',
              value: this.workoutDataCodesBase[0].value,
              label: this.$t('duration').toString(),
            },
            {
              code: 'distance',
              unit: this.workoutDataCodesBase[1].unit,
              units: [
                { value: 'mi', minValue: 0, maxValue: 9999999999999999 / 1.609 },
                { value: 'km', minValue: 0, maxValue: 9999999999999999 },
              ],
              value: this.workoutDataCodesBase[1].value,
              label: this.$t('distance').toString(),
            },
            {
              code: 'steps',
              unit: 'steps',
              value: this.workoutDataCodesBase[2].value,
              label: this.$t('steps').toString(),
            },
            {
              code: 'elevation',
              unit: this.workoutDataCodesBase[3].unit,
              units: [
                { value: 'ft', minValue: -9999999999999999 / 0.3048, maxValue: 9999999999999999 / 0.3048 },
                { value: 'm', minValue: -9999999999999999, maxValue: 9999999999999999 },
              ],
              value: this.workoutDataCodesBase[3].value,
              label: this.$t('elevation').toString(),
            },
            {
              code: 'floors',
              unit: 'floors',
              value: this.workoutDataCodesBase[4].value,
              label: this.$t('floors').toString(),
            },
            {
              code: 'water-consumed',
              unit: this.workoutDataCodesBase[5].unit,
              units: [
                { value: 'oz', minValue: 0, maxValue: 676 },
                { value: 'gal', minValue: 0, maxValue: 5 },
                { value: 'mL', minValue: 0, maxValue: 20000 },
                { value: 'L', minValue: 0, maxValue: 20 },
              ],
              value: this.workoutDataCodesBase[5].value,
              label: this.$t('waterConsumed').toString(),
            },
            {
              code: 'calories-burned',
              unit: 'kcal',
              value: this.workoutDataCodesBase[6].value,
              label: this.$t('caloriesBurned').toString(),
            },
            {
              code: 'heart-rate',
              unit: 'bpm',
              value: this.workoutDataCodesBase[7].value,
              label: this.$t('heartRate').toString(),
            },
          ] as ICombinedMeasurement[];
          return newWorkoutData;
        },
        set(newWorkoutData: ICombinedMeasurement[]) {
          this.workoutDataCodesBase = newWorkoutData;
        },
      },
    },
  });
</script>
<style lang="scss" scoped>
  .type-number {
    &::-webkit-outer-spin-button, ::-webkit-inner-spin-button

  {
    appearance: none;
  }
  }
</style>
