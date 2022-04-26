<template>
  <v-container id="geoworkspace" fluid data-v-sticky-container class="pt-0">
    <div class="flex incident-title" v-sticky="geoTitleStickyOptions" ref="incidentTitle">
      <v-row class="incident-title-inner" data-v-sticky-inner>
        <v-col cols="12" xs="12" sm="12" md="4" class="flex-grow-1 d-flex">
          <DemoTitleCardGeofence class="fill-width mt-4"></DemoTitleCardGeofence>
        </v-col>
        <v-col cols="12" xs="12" sm="12" md="8" class="flex-grow-0 d-flex">
          <DemoTelemedicineCard class="fill-width mt-4" store-name="geofence"></DemoTelemedicineCard>
        </v-col>
      </v-row>
    </div>

    <v-row>
      <v-col cols="4" data-v-sticky-container>
        <div class="flex" v-sticky="geoInfoStickyOptions">
          <v-row class="flex-column" data-v-sticky-inner>
            <v-col>
              <DemoPlanCardGeofence></DemoPlanCardGeofence>
            </v-col>
            <v-col>
              <DemoConditionsCard></DemoConditionsCard>
            </v-col>
            <v-col>
              <DemoMedicationsCard></DemoMedicationsCard>
            </v-col>
          </v-row>
        </div>
      </v-col>

      <v-col cols="5">
        <v-row>
          <v-col>
            <DemoGeofenceIncidentForm></DemoGeofenceIncidentForm>
          </v-col>
        </v-row>
      </v-col>

      <v-col cols="3" class="d-flex" data-v-sticky-container>
        <div class="flex" v-sticky="geoCheckStickyOptions">
          <v-row class="flex-column" data-v-sticky-inner>
            <v-col>
              <DemoGeofenceIncidentChecklist></DemoGeofenceIncidentChecklist>
            </v-col>
          </v-row>
        </div>
      </v-col>
    </v-row>
    <DemoGeofenceAlert></DemoGeofenceAlert>
  </v-container>
</template>
<script lang="ts">
import DemoTitleCardGeofence from '@/views/iugo-care/components/DemoTitleCardGeofence.vue';
import DemoTelemedicineCard from '@/views/iugo-care/components/DemoTelemedicineCard.vue';
import DemoConditionsCard from '@/views/iugo-care/components/DemoConditionsCard.vue';
import DemoMedicationsCard from '@/views/iugo-care/components/DemoMedicationsCard.vue';
import DemoPlanCardGeofence from '@/views/iugo-care/components/DemoPlanCardGeofence.vue';
import DemoGeofenceIncidentForm from '@/views/iugo-care/components/DemoGeofenceIncidentForm.vue';
import DemoGeofenceIncidentChecklist from '@/views/iugo-care/components/DemoGeofenceIncidentChecklist.vue';
import DemoGeofenceAlert from '@/views/iugo-care/components/DemoGeofenceAlert.vue';

export default({
  name: 'GeoWorkspace',
  title: 'iUGO Care - Geofence Workspace',
  data() {
    return {
      geoAlert: false,
      geoCheckStickyOptions: {
        topSpacing: 185,
        bottomSpacing: 0,
        resizeSensor: true,
      },
      geoTitleStickyOptions: {
        topSpacing: 62,
        resizeSensor: true,
      },
      geoInfoStickyOptions:  {
        topSpacing: 185,
        bottomSpacing: 0,
        resizeSensor: true,
      },
      height: 0,
    };

  },
  components: {
    DemoTitleCardGeofence,
    DemoTelemedicineCard,
    DemoConditionsCard,
    DemoMedicationsCard,
    DemoPlanCardGeofence,
    DemoGeofenceIncidentForm,
    DemoGeofenceIncidentChecklist,
    DemoGeofenceAlert,
  },
});
</script>
