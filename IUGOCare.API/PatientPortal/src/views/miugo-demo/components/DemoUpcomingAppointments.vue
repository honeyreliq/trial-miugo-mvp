<template>
  <BaseCard class="DemoUpcomingAppointments flex">
    <section class="d-flex justify-space-between mb-2">
      <div class="text-h6">{{$t("upcomingAppointments")}}</div>
      <BaseButton color="primary" @click.stop="appointmentModal = true">
        {{$t("requestAppointment")}}
      </BaseButton>
    </section>
    <v-data-table
      :headers="headers"
      :items="appointments"
      class="striped"
      hide-default-footer
      :disable-sort="true"
    >
    </v-data-table>
    <DemoRequestAppointmentModal v-model="appointmentModal"></DemoRequestAppointmentModal>
  </BaseCard>
</template>

<style lang="scss"></style>
<script lang="ts">
import Vue from 'vue';
import DemoRequestAppointmentModal from '@/views/miugo-demo/components/DemoRequestAppointmentModal.vue';
import { timestampToString, addToToday } from '@/views/miugo-demo/components/utils';

export default Vue.extend({
  name: 'DemoUpcomingAppointments',
  components: {
      DemoRequestAppointmentModal,
  },
  computed: {},
  watch: {},
  props: {},
  data(): any {
    return {
      appointmentModal: false,
      headers: [
        { text: this.$t('appointment'), value: 'title', sortable: true, class: 'text-subtitle-1' },
        { text: this.$t('location'), value: 'location', sortable: false, class: 'text-subtitle-1' },
        { text: this.$t('date'), value: 'date', sortable: true, class: 'text-subtitle-1' },
      ],
      appointments: [
        {
          title:'Nutrition Counseling with Jackie',
          location: 'Virtual Meeting',
          date: `${timestampToString(addToToday(0, 15, 0), 'L')} @ 11:00 AM`,
        },
        {
          title: 'Routine Bloodwork',
          location: '175 Longwood Rd S',
          date: `${timestampToString(addToToday(0, 11, 1), 'L')} @ 3:00 PM`,
        },
        {
          title: 'Annual Wellness Visit with Dr. Smith',
          location: '175 Longwood Rd S',
          date: `${timestampToString(addToToday(0, 3, 2), 'L')} @ 2:00 PM`,
        },
        {
          title: 'Diabetic Specialist Visit with Sam',
          location: 'Virtual Meeting',
          date: `${timestampToString(addToToday(0, 2, 3), 'L')} @ 9:45 AM`,
        },
        {
          title: 'BHI Check-in with Tom Biel',
          location: 'Phone Call',
          date: `${timestampToString(addToToday(0, 1, 5), 'L')} @ 1:30 PM`,
        },
        {
          title: 'CCM Engagement Call (focus Medications)',
          location: 'Phone Call',
          date: `${timestampToString(addToToday(0, 3, 8), 'L')} @ 12:00 PM`,
        },
      ],

    };
  },
  methods: {},
});
</script>
