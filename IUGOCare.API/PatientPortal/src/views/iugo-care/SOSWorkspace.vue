<template>
<v-container id="sosworkspace" fluid data-v-sticky-container class="pt-0">
    <div class="flex incident-title" v-sticky="sosTitleStickyOptions">
      <v-row class="incident-title-inner" data-v-sticky-inner>
        <v-col cols="12" xs="12" sm="12" md="4" class="flex-grow-1 d-flex">
          <DemoTitleCardSOS class="fill-width mt-4"></DemoTitleCardSOS>
        </v-col>
        <v-col cols="12" xs="12" sm="12" md="8" class="flex-grow-0 d-flex">
          <DemoTelemedicineCard
            class="fill-width mt-4"
            store-name="sos"
          ></DemoTelemedicineCard>
        </v-col>
      </v-row>
    </div>

    <v-row>
      <v-col cols="4" data-v-sticky-container>
        <div class="flex" v-sticky="sosInfoStickyOptions">
          <v-row class="flex-column" data-v-sticky-inner>
            <v-col>
              <DemoPlanCardSOS></DemoPlanCardSOS>
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
            <DemoSOSIncidentForm></DemoSOSIncidentForm>
          </v-col>
        </v-row>
      </v-col>

      <v-col cols="3" class="d-flex" data-v-sticky-container>
        <div class="flex" v-sticky="sosCheckStickyOptions">
          <v-row class="flex-column" data-v-sticky-inner>
            <v-col>
              <DemoSOSIncidentChecklist></DemoSOSIncidentChecklist>
            </v-col>
          </v-row>
        </div>
      </v-col>
    </v-row>
    <DemoSOSAlert></DemoSOSAlert>
  </v-container>
</template>
<script lang="ts">
import DemoTitleCardSOS from '@/views/iugo-care/components/DemoTitleCardSOS.vue';
import DemoTelemedicineCard from '@/views/iugo-care/components/DemoTelemedicineCard.vue';
import DemoConditionsCard from '@/views/iugo-care/components/DemoConditionsCard.vue';
import DemoMedicationsCard from '@/views/iugo-care/components/DemoMedicationsCard.vue';
import DemoPlanCardSOS from '@/views/iugo-care/components/DemoPlanCardSOS.vue';
import DemoSOSIncidentForm from '@/views/iugo-care/components/DemoSOSIncidentForm.vue';
import DemoSOSIncidentChecklist from '@/views/iugo-care/components/DemoSOSIncidentChecklist.vue';
import DemoSOSAlert from '@/views/iugo-care/components/DemoSOSAlert.vue';

export default({
  name: 'sosWorkspace',
  title: 'iUGO Care - SOS Workspace',
  data() {
    return {
      sosAlert: false,
      sosCheckStickyOptions: {
        topSpacing: 185,
        bottomSpacing: 0,
        resizeSensor: true,
      },
      sosTitleStickyOptions: {
        topSpacing: 62,
        resizeSensor: true,
      },
      sosInfoStickyOptions:  {
        topSpacing: 185,
        bottomSpacing: 0,
        resizeSensor: true,
      },
    };
  },
  components: {
    DemoTitleCardSOS,
    DemoTelemedicineCard,
    DemoConditionsCard,
    DemoMedicationsCard,
    DemoPlanCardSOS,
    DemoSOSIncidentForm,
    DemoSOSIncidentChecklist,
    DemoSOSAlert,
  },
});
</script>

