<template>
<div>
  <v-row>
  <v-col>
  <BaseCard>
    <v-row class="fill-height">
      <v-col
        cols="auto"
        class="flex-grow-1 align-self-center"
      >
        <span class="text-subtitle-1 font-weight-bold">Provider: </span>
        <span class="text-subtitle-1 mr-8">{{ carePlanData.provider }}</span>

        <span class="text-subtitle-1 font-weight-bold">Organization: </span>
        <span class="text-subtitle-1 mr-8">{{ carePlanData.organization }}</span>

        <span class="text-subtitle-1 font-weight-bold">Last Updated: </span>
        <span class="text-subtitle-1 mr-8">{{ carePlanData.updated }}</span>

      </v-col>
      <v-col cols="auto" class="d-flex justify-end">
        <BaseButton color="primary">
          <fa-icon :icon="['fal', 'print']" class="fa-fw"/>
        </BaseButton>
      </v-col>
    </v-row>
  </BaseCard>
  </v-col>
</v-row>
</div>
</template>

<script lang="ts">
import Vue from 'vue';
import { timestampToString, subtractFromToday } from '@/views/miugo-demo/components/utils';

interface IProgram {
  name: string;
  value: string;
  disabled: boolean;
}

const programs: IProgram[] = [
  {name: 'Remote Patient Monitoring (RPM)', value: 'RPM', disabled: true},
  {name: 'Chronic Care Management (CCM)', value: 'CCM', disabled: false},
  {name: 'Principal Care Management (PCM)', value: 'PCM', disabled: true},
  {name: 'Behavioral Health Integration (BHI)', value: 'BHI', disabled: true},
];

export default Vue.extend({
  name: 'DemoProgramInformation',
  components: {},
  computed: {},
  watch: {},
  props: {},
  data() {
    return {
      carePlanData: {
        provider: 'Dr. Michael Vanier',
        organization: 'Good Heath Clinic',
        updated: `${timestampToString(subtractFromToday(0, 1, 0), 'L')} @ 10:46 AM by Dr. Michael Vanier`,
    },
      selectedProgram: programs[1],
      selectedOption: programs[1].value,
      carePrograms: [...programs],
    };
  },
  methods: {},
});
</script>
