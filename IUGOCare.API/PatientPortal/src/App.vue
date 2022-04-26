<template>

  <v-app id="patientportal">
    <router-view />

    <v-tour :name="observationsTour" :steps="observationSteps" :callbacks="observationCallbacks"></v-tour>
  </v-app>

</template>

<script lang="ts">
  import { tourCallbacks, OBSERVATIONS_TOUR } from './common/vue-tour';
  import { mapState } from 'vuex';

  export default {
    name: 'App',

    computed:{
      ...mapState('patients', ['currentPatient'])
    },
    watch:{
      currentPatient(patient: any): void {
        if(patient) {
          localStorage.setItem('themeMode', patient.patientTheme);
          // @ts-ignore
          this.$vuetify.theme.dark = (patient.patientTheme === 'darkMode');
        }
      }
    },
    data() {
      return {
        observationsTour: OBSERVATIONS_TOUR,
        observationSteps: [
          {
            target: '#observations .text-h5',
            header: {
              title: 'Get Started',
            },
            content: `Discover <strong>Vue Tour</strong>!`,
            params: {
              placement: 'right',
            },
          },
          {
            target: 'button[value="blood-pressure"]',
            header: {
              title: '🩸',
            },
            content: `🅰🅱🆎🅾`,
          },
          {
            target: 'button[value="week"]',
            header: {
              title: 'Filter by time',
            },
            content: `DO IT`,
          },
        ],
        observationCallbacks: {
          onSkip: tourCallbacks.onSkip(OBSERVATIONS_TOUR),
          onFinish: tourCallbacks.onFinish(OBSERVATIONS_TOUR),
        },
      };
    },
    mounted() {
      setTimeout(() => {
        const theme = localStorage.getItem('themeMode');
        // @ts-ignore
        this.$vuetify.theme.dark = (theme === 'darkMode');
      });
    }
  };
</script>
