<template>
  <v-container id="dashboard" fluid>
    <v-row class="fill-height flex-column" justify="center">
      <v-col class="flex-grow-0">
        <ObservationsHeader></ObservationsHeader>
      </v-col>
    </v-row>
   <DemoActivityCards></DemoActivityCards>
    <v-row>
      <v-col cols="6" class="flex-grow-1 d-flex">
        <DemoUpcomingAppointments v-if="currentPatient"></DemoUpcomingAppointments>
      </v-col>
      <v-col cols="6" class="flex-grow-1 d-flex">
        <BaseCard class="flex">
          <div class="text-h6 pb-4">{{$t("about")}} <strong>MiUGO Care</strong></div>
          <p> <strong> MiUGO</strong> {{$t("aboutMiugoText")}}</p>
          <div class="text-h6 pb-4">{{$t("whatDoWithMiugo")}} <strong>MiUGO Care</strong></div>
          <p>{{$t("whatDoWithMiugoText1")}} <strong>MiUGO</strong> {{$t("whatDoWithMiugoText2")}}</p>
          <ul class="pb-4">
            <li>{{$t("whatDoWithMiugoText3")}}</li>
            <li>{{$t("whatDoWithMiugoText4")}}</li>
            <li>{{$t("whatDoWithMiugoText6")}}</li>
          </ul>
          <p>{{$t("whatDoWithMiugoText7")}} <strong>MiUGO</strong> {{$t("whatDoWithMiugoText8")}}</p>
        </BaseCard>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { mapState } from 'vuex';
import ObservationsHeader from '../../components/observations/ObservationsHeader.vue';
import DemoUpcomingAppointments from '@/views/miugo-demo/components/DemoUpcomingAppointments.vue';
import DemoActivityCards from '@/views/miugo-demo/components/DemoActivityCards.vue';

export default({
  name: 'MiugoDashboard',
  components: {ObservationsHeader, DemoUpcomingAppointments, DemoActivityCards},
  title: 'MiUGO - Dashboard',
  computed: {
    ...mapState('patients', ['currentPatient'])
  },
});
</script>
