<template>
  <v-row justify="center">
    <v-dialog
        v-model="open"
        persistent
        max-width="500px"
        dark
    >
      <v-card color="error" flat>
        <v-card-text>Caron, Monique Alicia (Alicia) - 67 F - 04/28/1953 - English</v-card-text>
        <v-card-title class="headline">
          <v-row class="pb-4 mx-auto" align="center">
            <v-spacer></v-spacer>
            <v-col cols="3">
              <fa-icon
                  class="alert-icon mx-4"
                  icon="exclamation-triangle"
              ></fa-icon>
            </v-col>
            <v-col cols="8">
              <v-row>Incident Alert:</v-row>
              <v-row class="font-weight-bold">Geofence Violation</v-row>
              <v-row class="text-subtitle-1">{{geofenceWorkspace.alert.date}} @ {{geofenceWorkspace.alert.time}}</v-row>
            </v-col>
          </v-row>
        </v-card-title>
        <div class="pt-4 alert-card">
          <v-card-actions class="d-flex justify-space-around">
            <BaseButton color="primary" to="/iugo-care/workspace/geo" @click="clickButtonHandler(respondHandler)">
              Respond
            </BaseButton>
            <BaseButton color="primary" @click="clickButtonHandler()">
              Redirect
            </BaseButton>
            <BaseButton color="primary" @click="clickButtonHandler()">
              Escalate
            </BaseButton>
          </v-card-actions>
        </div>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue';
import {mapMutations, mapState} from 'vuex';
import {IAlertInformation} from '@/store/demo';

export default Vue.extend({
  name: 'DemoGeofenceAlert',
  computed: {
    ...mapState('demo', ['geofenceWorkspace']),
  },
  data(): any {
    return {
      open: false,
    };
  },
  methods: {
    ...mapMutations('demo', {
      setGeofenceAlert: 'setGeofenceAlert',
    }),
    setAlertData(): void {
      const time: Date = new Date();
      const timeValue = time.getHours().toString().padStart(2, '0') + ':' +
        time.getMinutes().toString().padStart(2, '0') + ':' +
        time.getSeconds().toString().padStart(2, '0');
      const dateValue = (time.getMonth() + 1 ).toString().padStart(2, '0') + '/' +
        time.getDate().toString().padStart(2, '0') + '/' +
        time.getFullYear().toString();
      const alertModel: IAlertInformation = {
        triggered: true,
        date: dateValue,
        time: timeValue,
      };
      this.setGeofenceAlert(alertModel);
    },
    keyboardHandler(event: KeyboardEvent): void {
      const KEY = 'G';
      // COMMAND: command {ctrl} + option {alt} + G
      // Macintosh
      if (navigator.userAgent.includes('Mac OS X')) {
        if ((event.altKey && event.metaKey) && event.keyCode === KEY.charCodeAt(0)) {
          event.preventDefault();
          this.openModal();
        }
      } else {
        // Windows
        if ((event.altKey && event.ctrlKey) && event.keyCode === KEY.charCodeAt(0)) {
          event.preventDefault();
          this.openModal();
        }
      }

    },
    openModal(): void {
      this.open = true;
      this.setAlertData();
    },
    clickButtonHandler(fn: () => void = null): void {
      this.open = false;
      if (fn) {
        fn();
      }
    },
    respondHandler(): void {
      // TODO: code for redirecting goes here
    },
  },
  mounted() {
    window.removeEventListener('keydown', this.keyboardHandler);
    window.addEventListener('keydown', this.keyboardHandler);
  },
  destroyed() {
    window.removeEventListener('keydown', this.keyboardHandler);
  },
});
</script>
