<template>
  <div>
    <div v-for="(item, i) in services" :key="`service-item-${i}`">
      <BaseCard :title="item.title" divider head-class="primary white--text">
        <v-row v-for="(services, index) in item.services" :key="`services-${index}`">
          <v-col
            cols="12"
            :class="index < item.services.length - 1 ? 'mb-4' : ''"
          >
            <span class="font-weight-bold mb-4">{{ services.name }}</span> -
            <span class="mb-4">{{ services.description }}</span>
          </v-col>
        </v-row>
      </BaseCard>
    </div>
    <div v-for="(item, i) in contacts" :key="`contacts-item-${i}`">
      <BaseCard :title="item.title" divider head-class="primary white--text">
        <v-row
          v-for="(contacts, index) in item.contacts"
          :key="`services-${index}`"
        >
          <v-col
            cols="12"
            :class="index < item.contacts.length - 1 ? 'mb-4' : ''"
          >
            <div class="font-weight-bold mb-4">{{ contacts.service }}</div>
            <div class="mb-4">
              <span v-if="contacts.days" class="mr-4 light">
                <fa-icon :icon="['fad', 'clock']" class="fa-fw" />
              </span>
              <span>{{ contacts.days }}</span>
            </div>
            <div class="mb-4">
              <span v-if="contacts.phone" class="mr-4 light">
                <fa-icon :icon="['fad', 'phone']" class="fa-fw" />
              </span>
              <span>{{ contacts.phone }}</span>
            </div>
            <div v-if="contacts.note">{{ contacts.note }}</div>
            <v-divider
              v-if="contacts.divider"
              :key="index"
              class="mt-4"
            ></v-divider>
          </v-col>
        </v-row>
      </BaseCard>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'DemoCarePlanServices',
  components: {},
  computed: {},
  watch: {},
  props: {},
  data() {
    return {
      services: [
        {
          title: 'Services',
          services: [
            {
            name: 'Good Health Clinic',
            description: 'General inquiries regarding health.',
            },
            {
            name: 'Diabetes Specialist',
            description: 'Resource for diabetes specific issues or concerns.',
            }
          ],
        },
      ],
      contacts: [
        {
          title: '24/7 Access to Care',
          contacts: [
            {
              service: 'Good Health Clinic - Office hours:',
              days: 'Monday to Saturday, 8:30 AM - 4:30 PM',
              phone: '+1 (905) 555-5555',
              note: 'Outside office hours contact +1 (905) 541-5555',
              divider: true,
            },
            {
              service: 'Diabetes Specialist - Office hours:',
              days: 'Monday to Friday, 9:00 AM - 3:00 PM',
              phone: '+1 (905) 655-4343',
              note: 'In case of emergency, call 9-1-1',
            },
          ],
        },
      ],
    };
  },
  methods: {},
});
</script>
