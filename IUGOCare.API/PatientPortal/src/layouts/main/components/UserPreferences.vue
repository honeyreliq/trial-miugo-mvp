<template>
  <v-dialog :value="value" @input="$emit('input')" width="600px" @keydown.esc="prefModal = false">
    <v-card color="var(--v-background-base)">

      <v-toolbar dark flat fixed color="primary">
        <v-toolbar-title>{{ $t('preferences') }}</v-toolbar-title>
        <v-spacer></v-spacer>
        <BaseButton icon dark @click.native="$emit('input')">
          <fa-icon :icon="['fal', 'times']" class="fa-fw" />
        </BaseButton>
      </v-toolbar>

      <v-row no-gutters>
        <v-col cols="12">

          <v-card-subtitle class="pb-0">
            <span class="text-h6">{{ $t('general') }}</span>
          </v-card-subtitle>
          <v-card-text>
            <BaseCard>
              <v-row no-gutters class="mb-4">
                <v-col class="text-h6" cols="auto" sm="12" md="4">{{ $t('language') }}</v-col>
                <v-spacer />
                <v-col cols="auto" sm="12" md="8">
                  <v-select v-model="currentLocale"
                      :items="locales"
                      item-text="title"
                      return-object
                      single-line
                      @change="updateLanguage()"
                      menu-props="auto" filled dense hide-details
                  ></v-select>
                </v-col>
              </v-row>
              <v-row no-gutters class="mb-4">
                <v-col cols="12">
                  <v-divider />
                </v-col>
              </v-row>
              <v-row no-gutters>
                <v-col class="text-h6" cols="auto">{{ $t('tooltips') }}</v-col>
                <v-spacer />
                <v-col cols="auto">
                  <v-switch v-model="tooltips" class="ma-0 pa-0"
                    @change="savePrefereces()" hide-details />
                </v-col>
                <v-col
                  class="text-subtitle-2"
                  cols="12"
                >{{ $t('tooltipsDescription') }}</v-col>
              </v-row>
            </BaseCard>
          </v-card-text>

          <v-card-subtitle class="pb-0">
            <span class="text-h6">{{ $t('theme') }}</span>
          </v-card-subtitle>
          <v-card-text>
            <BaseCard>
              <v-row no-gutters>
                <v-radio-group v-model="theme" dense hide-details class="ma-0 pa-0"
                  @change="changeTheme()">
                  <v-radio :label="$t('lightMode')" value="lightMode"></v-radio>
                  <v-radio :label="$t('darkMode')" value="darkMode"></v-radio>
                </v-radio-group>
              </v-row>
            </BaseCard>
          </v-card-text>

          <v-card-subtitle class="pb-0">
            <span class="text-h6">{{ $t('dateAndTimePreferences') }}</span>
          </v-card-subtitle>
          <v-card-text>
            <BaseCard>
              <v-row no-gutters class="mb-4">
                <v-col class="text-h6" cols="auto" sm="12" md="4">{{ $t('timeZone') }}</v-col>
                <v-spacer />
                <v-col cols="auto" sm="12" md="8">
                  <v-select v-model="patientTimeZone"
                    :items="timeZones"
                    item-text="timeZoneName"
                    return-object
                    single-line
                    @change="savePrefereces()"
                    menu-props="auto" filled dense hide-details
                  ></v-select>
                </v-col>
              </v-row>
              <v-row no-gutters class="mb-4">
                <v-col class="text-h6" cols="auto" sm="12" md="4">{{ $t('dateFormat') }}</v-col>
                <v-spacer />
                <v-col cols="auto" sm="12" md="8">
                  <v-select v-model="dateFormat"
                    :items="dateFormats"
                    item-text="name"
                    return-object
                    single-line
                    @change="savePrefereces()"
                    menu-props="auto" filled dense hide-details
                  ></v-select>
                </v-col>
              </v-row>
              <v-row no-gutters>
                  <v-col class="text-h6" cols="9">{{ $t('timeFormat') }}</v-col>
                  <v-col >
                    12 hrs
                  </v-col>
                  <v-col >
                    <v-switch v-model="timeFormat" class="ma-0 pa-0" hide-details
                    @change="savePrefereces()"/>
                  </v-col>
                  <v-col >
                    24 hrs
                  </v-col>
              </v-row>
            </BaseCard>
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>
            <BaseButton color="primary" @click.native="$emit('input')">{{$t('close')}}</BaseButton>
          </v-card-actions>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import Vue from 'vue';
// Mixins
import i18n from '@/i18n';
import { mapMutations } from 'vuex';
import axiosClient from '@/auth/axiosInstance';
import { PatientsClient, UpdatePatientPreferencesCommand } from '../../../IUGOCare-api';
import { availableLocales, availableTimeZones, availableDateFormats } from '../../../common/SettingArraysOptions';

export default Vue.extend({
  name: 'UserPreferences',
  props: ['value' ],

  data: () => ({
    currentPatient: null,
    locales: availableLocales,
    timeZones: availableTimeZones,
    dateFormats: availableDateFormats,
    currentLocale: {
      name: 'en',
      flag: 'usFlag.png',
      label: 'English',
      title: 'English',
    },
    theme: 'lightMode',
    tooltips: true,
    patientTimeZone:
    {
      timeZone: 'Etc/UTC',
      windowsTimeZone: 'UTC',
      timeZoneName: '(UTC) Coordinated Universal Time',
    },
    dateFormat: {
      name: 'MM/DD/YYYY',
      value: 'MM/dd/yyyy',
    },
    timeFormat: false,
  }),
  mounted() {
    this.$root.$on('openPreferences', this.updateForm);
  },
  methods: {
    ...mapMutations('settings', {
      setDrawer: 'SET_DRAWER',
    }),
    ...mapMutations('patients', {
      setCurrentPatient: 'setCurrentPatient',
    }),
    updateForm(): void {
      this.$store.dispatch('patients/fetchCurrentPatient')
      .then((currentPatient) => {
        this.currentPatient = currentPatient;
        this.theme = currentPatient.patientTheme ? currentPatient.patientTheme : 'lightMode';
        this.tooltips = currentPatient.tooltips;
        this.patientTimeZone = currentPatient.timeZone ?
          this.timeZones.find((x) => x.timeZone === currentPatient.timeZone) :
        {
          timeZone: 'Etc/UTC',
          windowsTimeZone: 'UTC',
          timeZoneName: '(UTC) Coordinated Universal Time',
        };
        this.currentLocale = currentPatient.patientLanguage ?
            this.locales.find((x) => x.name === currentPatient.patientLanguage.toLowerCase()) :
            this.locales.find((x) => x.name === i18n.locale);

        this.dateFormat = currentPatient.dateFormat
        ? this.dateFormats.find((x) => x.value === currentPatient.dateFormat )
        : this.dateFormats.find((x) => x.value === 'MM/dd/yyyy');
        this.timeFormat = currentPatient.timeFormat === '24 H';
      });
    },
    updateLanguage(): void {
      this.$i18n.locale = this.currentLocale.name;
      localStorage.setItem('language', this.$i18n.locale);
      this.savePrefereces();
    },
    changeTheme(): void {
      localStorage.setItem('themeMode', this.theme);
      this.$vuetify.theme.dark = this.theme === 'darkMode';
      this.savePrefereces();
    },
    savePrefereces(): void {
      localStorage.setItem('language', this.$i18n.locale);
      const command = new UpdatePatientPreferencesCommand();
      command.patientId = this.currentPatient.id;
      command.patientLanguage = this.currentLocale.name;
      command.tooltips = this.tooltips;
      command.patientTheme = this.theme;
      command.timeZone = this.patientTimeZone.timeZone;
      command.windowsTimeZone = this.patientTimeZone.windowsTimeZone;
      command.dateFormat = this.dateFormat.value;
      command.timeFormat = this.timeFormat ? '24 H' : '12 H';

      const ctc = new PatientsClient(process.env.VUE_APP_API_URL, axiosClient);
      ctc.updatePatientPreferences(command).then(() => {
        this.currentPatient.patientLanguage = this.currentLocale.name;
        this.currentPatient.tooltips = this.tooltips;
        this.currentPatient.patientTheme = this.theme;
        this.currentPatient.timeZone = this.patientTimeZone.timeZone;
        this.currentPatient.windowsTimeZone = this.patientTimeZone.windowsTimeZone;
        this.currentPatient.dateFormat = this.dateFormat.value;
        this.currentPatient.timeFormat = this.timeFormat ? '24 H' : '12 H';
        this.setCurrentPatient(this.currentPatient);
      });
    },
  },
});
</script>

