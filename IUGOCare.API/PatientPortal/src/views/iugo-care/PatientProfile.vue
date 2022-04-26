<template>
  <v-container id="patientprofile" fluid>
    <v-row>
      <v-snackbar
        app
        class="pb-4"
        timeout="2000"
        v-model="showSuccessMessage"
        color="primary"
        center
        bottom
        tag="body"
        dark>
      <div justify="center" align="center" class="text-subtitle-1">{{successMessage}}</div>
    </v-snackbar>

      <v-col cols="12" class="hidden-sm-and-down">
        <BaseButtonGroup mandatory class="mb-4" >
          <BaseButton>
            {{ $t("surveys") }}
          </BaseButton>
          <BaseButton class="disable-events">
            {{ $t("notes") }}
          </BaseButton>
          <BaseButton class="disable-events">
           {{ $t("observations") }}
          </BaseButton>
          <BaseButton class="disable-events">
            {{ $t("medications") }}
          </BaseButton>
          <BaseButton class="disable-events">
           {{ $t("documents") }}
          </BaseButton>
          <BaseButton class="disable-events">
            {{ $t("programs") }}
            <fa-icon right dark :icon="['fas', 'caret-down']" class="fa-fw fa-button pt-1" />
          </BaseButton>
        </BaseButtonGroup>
      </v-col>
      <v-col cols="12" class="hidden-md-and-up">
        <v-select class="my-4" />
      </v-col>
      <v-col cols="7" class="d-fle flex-column">
        <DemoActiveConditions v-if="currentPatient" class="flex-grow-1"></DemoActiveConditions>
      </v-col>
      <v-col cols="5" class="d-flex flex-column">
        <DemoCareTeamCard v-if="currentPatient" class="flex-grow-1"></DemoCareTeamCard>
      </v-col>
      <v-col cols="12">
        <DemoCareProgramsCard></DemoCareProgramsCard>
      </v-col>
    </v-row>
    <DemoGeofenceAlert></DemoGeofenceAlert>
    <DemoSOSAlert></DemoSOSAlert>
    <DemoFallAlert></DemoFallAlert>
  </v-container>
</template>

<script lang="ts">
import {mapState} from 'vuex';
import DemoCareProgramsCard from '@/views/iugo-care/components/DemoCareProgramsCard.vue';
import DemoActiveConditions from '@/views/iugo-care/components/DemoActiveConditions.vue';
import DemoCareTeamCard from '@/views/iugo-care/components/DemoCareTeamCard.vue';
import DemoGeofenceAlert from '@/views/iugo-care/components/DemoGeofenceAlert.vue';
import DemoSOSAlert from '@/views/iugo-care/components/DemoSOSAlert.vue';
import DemoFallAlert from '@/views/iugo-care/components/DemoFallAlert.vue';

export default({
  name: 'PatientProfile',
  title: 'iUGO Care - Patient Profile',
  data() {
    return {
      showSuccessMessage: false,
      successMessage: '',
      geoAlert: false,
      sosAlert: false,
      fallAlert: false,
    };
  },
  computed: {
    ...mapState('demo', ['sosWorkspace', 'geofenceWorkspace', 'fallWorkspace']),
    ...mapState('patients', ['currentPatient']),
  },
  watch: {
    sosWorkspace(value: boolean): void {
      // @ts-ignore
      this.showSuccessMessage = value;
      // @ts-ignore
      this.successMessage = 'The SOS Incident Report has been saved!';
    },
    fallWorkspace(value: boolean): void {
      // @ts-ignore
      this.showSuccessMessage = value;
      // @ts-ignore
      this.successMessage = 'The Fall Incident Report has been saved!';
    },
    geofenceWorkspace(value: boolean): void {
      // @ts-ignore
      this.showSuccessMessage = value;
      // @ts-ignore
      this.successMessage = 'The Geofence Violation Incident Report has been saved!';
    },
  },
  components: {
      DemoCareProgramsCard,
      DemoActiveConditions,
      DemoCareTeamCard,
      DemoGeofenceAlert,
      DemoSOSAlert,
      DemoFallAlert,
    },
});
</script>
