<template>
  <v-dialog
      v-model="openModal"
      persistent
      width="80%"
      content-class="DemoGeofenceMapDirectionsModal black--text"
  >
    <v-card>
      <BaseToolbar title="Incident Summary - Geofence Violation">
        <v-spacer></v-spacer>
        <BaseButton dark
                    icon
                    @click="openFn(false)"
        >
          <fa-icon :icon="['fal', 'times']" class="fa-fw" />
        </BaseButton>
      </BaseToolbar>
      <v-card-text class="mt-0 patient-details">
        <v-row>
          <v-col cols="12" md="8">
            <div class="text-h6">
              {{ patient.name }} - {{ patient.age }} - {{ patient.dateOfBirth }} - {{ patient.language }}
            </div>
          </v-col>
          <v-col cols="12" md="4">
            <div class="caption text-md-right mt-1">
              {{ eventTime }}
            </div>
          </v-col>
        </v-row>

        <section class="modal-map-container">
          <div class="loader d-flex justify-center align-center" v-if="!showMap">
            <v-progress-circular :size="50"
                                 color="primary"
                                 indeterminate
            ></v-progress-circular>
          </div>
          <v-fade-transition :hide-on-leave="true">
            <BaseGoogleMap v-if="showMap"
                           :home-location="homeLocation"
                           :patient-location="patientLocation"
                           :event-time="eventTime"
                           :use-directions-panel="true"
                           :hide-details="true"
                           :use-switch-location="true"
            ></BaseGoogleMap>
          </v-fade-transition>
        </section>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<style lang="scss">
.DemoGeofenceMapDirectionsModal {
  .map-container, .loader, .modal-map-container {
    min-height: 500px !important;
  }

  .v-card {
    .patient-details {
      padding-top: 8px !important;
      padding-bottom: 8px !important;
    }
  }

}
</style>
<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'DemoGeofenceMapDirectionsModal',
  components: {},
  computed: {},
  watch: {
    openModal(value: boolean) {
      setTimeout(() => {
        this.showMap = value;
      }, 500);
    },
  },
  props: {
    openModal: Boolean,
    openFn: Function,
    homeLocation: Object,
    patientLocation: Object,
    eventTime: String,
  },
  data() {
    return {
      showMap: false,
      patient: {
        name: 'Caron, Monique Alicia (Alicia)',
        age: '67 F',
        dateOfBirth: '04/28/1953',
        language: 'English',
      },
    };
  },
  methods: {},
});
</script>
