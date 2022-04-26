<template>
  <div class="DemoNotificationsMenu">
    <div class="text-h6 my-4">Notifications</div>
    <div class="notifications-container">
      <BaseCard v-for="item in notifications" v-bind:key="item.title.text" :to="item.to">
        <div @click="item.title.className = null">
          <v-row class="mb-2">
            <v-col cols="12" :class="item.title.className">{{ item.title.text }}</v-col>
          </v-row>
          <v-row class="mb-2">
            <v-col cols="12">{{ item.text }}</v-col>
          </v-row>
          <v-row>
            <v-col cols="12" class="text-right caption">{{ item.date }}</v-col>
          </v-row>
        </div>
      </BaseCard>
    </div>
  </div>
</template>

<style lang="scss">
.DemoNotificationsMenu {
  .notifications-container {
    max-height: 400px;
    overflow-y: auto;
  }
}
</style>
<script lang="ts">
import Vue from 'vue';
import { timestampToString, subtractFromToday } from '@/views/miugo-demo/components/utils';

export default Vue.extend({
  name: 'DemoNotificationsMenu',
  components: {},
  computed: {},
  watch: {},
  props: {},
  data(): any {
    return {
      notifications: [
        {
          title: {
            text: 'Care Plan Updated',
            className: 'font-weight-bold',
          },
          text: 'Your care plan for Chronic Care Management has been updated.',
          date: `${timestampToString(subtractFromToday(0, 1, 0), 'L')} @ 10:46 AM`,
          to: '/miugo/careplans/ccm',
        },
        {
          title: {
            text: 'Self Assessment Survey',
            className: 'font-weight-bold',
          },
          text: 'Please review and complete the survey to submit to your care team.',
          date: `${timestampToString(subtractFromToday(0, 1, 0), 'L')} @ 9:23 AM`,
          to: '/miugo/surveys?surveymodal=1',
        },
        {
          title: {
            text: 'Appointment Reminder',
            className: '',
          },
          text: 'You have an appointment with your doctor on 09/05/2020 at 10:00 AM, if you are unable to attend please contact our office at Good Health Clinic 888-888-8888.',
          date: `${timestampToString(subtractFromToday(0, 3, 0), 'L')} @ 11:00 AM`,
          to: '/miugo/dashboard',
        },
        {
          title: {
            text: 'RPM Adherence',
            className: '',
          },
          text: 'You have not submitted a blood pressure reading in 3 days, please check that your device is working properly or contact the office with any issues.',
          date: `${timestampToString(subtractFromToday(0, 5, 0), 'L')} @ 9:03 AM`,
          to: '/miugo/observations/vital-signs?observationdialog=1',
        },
      ],
    };
  },
});
</script>
