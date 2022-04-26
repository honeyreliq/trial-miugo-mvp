<template>
<v-container id="fallworkspace" fluid data-v-sticky-container class="pt-0">
    <div class="flex incident-title" v-sticky="fallTitleStickyOptions">
      <v-row class="incident-title-inner" data-v-sticky-inner>
        <v-col cols="12" xs="12" sm="12" md="4" class="flex-grow-1 d-flex">
          <DemoTitleCardFall class="fill-width mt-4"></DemoTitleCardFall>
        </v-col>
        <v-col cols="12" xs="12" sm="12" md="8" class="flex-grow-0 d-flex">
          <DemoTelemedicineCard
            class="fill-width mt-4"
            store-name="fall"
          ></DemoTelemedicineCard>
        </v-col>
      </v-row>
    </div>

    <v-row>
      <v-col cols="4" data-v-sticky-container>
        <div class="flex" v-sticky="fallInfoStickyOptions">
          <v-row class="flex-column" data-v-sticky-inner>
            <v-col>
              <DemoPlanCardFall></DemoPlanCardFall>
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
            <DemoFallIncidentForm></DemoFallIncidentForm>
          </v-col>
        </v-row>
      </v-col>

      <v-col cols="3" class="d-flex" data-v-sticky-container>
        <div class="flex" v-sticky="fallCheckStickyOptions">
          <v-row class="flex-column" data-v-sticky-inner>
            <v-col>
              <DemoFallIncidentChecklist></DemoFallIncidentChecklist>
            </v-col>
          </v-row>
        </div>
      </v-col>
    </v-row>
    <DemoFallAlert></DemoFallAlert>
  </v-container>
</template>

<script lang="ts">
import DemoTitleCardFall from '@/views/iugo-care/components/DemoTitleCardFall.vue';
import DemoTelemedicineCard from '@/views/iugo-care/components/DemoTelemedicineCard.vue';
import DemoConditionsCard from '@/views/iugo-care/components/DemoConditionsCard.vue';
import DemoMedicationsCard from '@/views/iugo-care/components/DemoMedicationsCard.vue';
import DemoPlanCardFall from '@/views/iugo-care/components/DemoPlanCardFall.vue';
import DemoFallIncidentForm from '@/views/iugo-care/components/DemoFallIncidentForm.vue';
import DemoFallIncidentChecklist from '@/views/iugo-care/components/DemoFallIncidentChecklist.vue';
import DemoFallAlert from '@/views/iugo-care/components/DemoFallAlert.vue';

export default({
  name: 'FallWorkspace',
  title: 'iUGO Care - Fall Workspace',
  data() {
    return {
      fallAlert: false,
      fallCheckStickyOptions: {
        topSpacing: 185,
        bottomSpacing: 0,
        resizeSensor: true,
      },
      fallTitleStickyOptions: {
        topSpacing: 62,
        resizeSensor: true,
      },
      fallInfoStickyOptions:  {
        topSpacing: 185,
        bottomSpacing: 0,
        resizeSensor: true,
      },
    };
  },
  components: {
    DemoTitleCardFall,
    DemoTelemedicineCard,
    DemoConditionsCard,
    DemoMedicationsCard,
    DemoPlanCardFall,
    DemoFallIncidentForm,
    DemoFallIncidentChecklist,
    DemoFallAlert,
  },
});
</script>
