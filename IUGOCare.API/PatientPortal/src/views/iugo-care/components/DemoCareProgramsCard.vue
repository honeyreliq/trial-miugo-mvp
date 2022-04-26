<template>
  <BaseCard>
    <section class="d-flex justify-space-between mb-4">
      <div class="text-h6"> {{ $t('carePrograms') }}</div>
        <BaseButton color="primary" dark @click="expanded = !expanded">
          {{expanded ? $t('collapse') : $t('expand')}}
        </BaseButton>
    </section>
    <v-row>
      <v-col
        cols="12"
        xs="12"
        sm="12"
        md="6"
        lg="4"
        xl="3"
        v-for="program in programs"
        :key="program.key"
      >
        <BaseProgramDisplay
          :subtitle="$t(program.subtitle)"
          :title="$t(program.title)"
          :status="program.status"
          :status-handler="changeStatus"
          :expanded="expanded"
          :label="$t(program.label)"
        >
          <!--- care program --->
          <div v-if="program.type === 'careProgram'">
            <v-row :class="{'mb-5': !program.status}">
              <v-select :label="$t('provider')" v-model="program.provider" :items="providers" disabled></v-select>
            </v-row>
            <v-row justify="space-between" v-if="program.status">
              <div class="text-subtitle-2">{{program.timeSpent}}</div>
              <div class="text-subtitle-2">{{program.timeRemaining}}</div>
            </v-row>
            <v-row class="mb-2">
              <v-progress-linear
                color="light-blue"
                height="10"
                :value="program.progressStatus"
              ></v-progress-linear>
              <v-row justify="center" class="mt-2">
                <div class="caption">{{$t('targetTime')}}: {{program.targetTime}}</div>
              </v-row>
            </v-row>
            <v-row justify="center" align="center" class="mb-2" :class="{'mb-6': !program.since}">
              <div class="caption pr-2">{{$t('onboardingStatus')}}:</div>
              <v-chip :color="program.onboardingColor" text-color="white" class="caption pr-2" small>
                {{$t(program.onboardingLabel)}}
              </v-chip>
            </v-row>
            <v-row justify="end">
              <div class="caption" v-for="since in program.since" :key="since.key">
                {{ $t(since.label) }}
                {{ since.value }}
                ({{ since.number }} {{ $t(since.period) }})
              </div>
            </v-row>
          </div>
          <!--- iugoHome --->
          <div v-else>
            <v-row>
              <v-select :label="$t('provider')" v-model="program.provider" :items="providers" disabled></v-select>
            </v-row>
            <v-row class="mb-1" v-for="monitoring in program.monitoringList" :key="monitoring.label">
              <v-col cols="8">
                <div class="caption">{{$t(monitoring.label)}}:</div>
              </v-col>
              <v-col cols="3">
                <v-switch
                  color="primary"
                  hide-details
                  class="ma-0 pa-0 disable-events"
                  v-model="monitoring.value"
                ></v-switch>
              </v-col>
            </v-row>
            <v-row justify="end" class="pt-3">
              <div class="caption" v-for="since in program.since" :key="since.key">
                {{ $t(since.label) }}
                {{ since.value }}
                ({{ since.number }} {{ $t(since.period) }})
              </div>
            </v-row>
          </div>
        </BaseProgramDisplay>
      </v-col>
    </v-row>
  </BaseCard>
</template>

<script lang="ts">
    import Vue from 'vue';
    import { timestampToString, subtractFromToday } from '@/views/miugo-demo/components/utils';

    export default Vue.extend({
        name: 'DemoCareProgramsCard',
        components: {},
        computed: {},
        methods: {
            changeStatus(status: boolean, programKey: string) {
                const program = this.programs.find(({key}) => key === programKey);
                if (program) {
                    program.status = status;
                    program.progressStatus = 0;
                    program.timeSpent = '00:00';
                    program.timeRemaining = program.targetTime;
                    program.onboardingLabel = status ? 'inProgress' : 'inactive';
                    program.onboardingColor = status ? 'warning' : 'lightGrey darken-2';
                }
            },
        },

        data: () => ({
            expanded: false,
            providers: ['Dr. Michael Vanier', 'Dr. Jane Smith'],
            programs: [
                {
                    key: 'rpm',
                    title: 'rpm',
                    subtitle: '(RPM)',
                    type: 'careProgram',
                    provider: 'Dr. Michael Vanier',
                    status: true,
                    timeSpent: '10:00',
                    timeRemaining: '10:00',
                    progressStatus: 50,
                    targetTime: '20:00',
                    onboardingLabel: 'onboarded',
                    onboardingColor: 'success',
                    since: [
                      {
                        key: 'rpm',
                        label:'since',
                        value: `${timestampToString(subtractFromToday(0, 0, 3), 'L')}`,
                        number: '3',
                        period: 'months',
                      },
                    ],
                    label: 'switchStatus',
                },
                {
                    key: 'ccm',
                    title: 'ccm',
                    subtitle: '(CCM)',
                    type: 'careProgram',
                    provider: 'Dr. Michael Vanier',
                    status: true,
                    timeSpent: '10:00',
                    timeRemaining: '10:00',
                    progressStatus: 50,
                    targetTime: '20:00',
                    onboardingLabel: 'inProgress',
                    onboardingColor: 'warning',
                    label: 'switchStatus',

                },
                {
                    key: 'pcm',
                    title: 'pcm',
                    subtitle: '(PCM)',
                    type: 'careProgram',
                    provider: '',
                    status: false,
                    timeSpent: '00:00',
                    timeRemaining: '30:00',
                    progressStatus: 0,
                    targetTime: '30:00',
                    onboardingLabel: 'inactive',
                    onboardingColor: 'lightGrey darken-2',
                    label: 'switchStatus',

                },
                {
                    key: 'tcm',
                    title: 'tcm',
                    subtitle: '(TCM)',
                    type: 'careProgram',
                    provider: 'Dr. Michael Vanier',
                    status: false,
                    timeSpent: '00:00',
                    timeRemaining: '30 days',
                    progressStatus: 0,
                    targetTime: '30 days',
                    onboardingLabel: 'inactive',
                    onboardingColor: 'lightGrey darken-2',
                    label: 'switchStatus',

                },
                {
                    key: 'bhi',
                    title: 'bhi',
                    subtitle: '(BHI)',
                    type: 'careProgram',
                    provider: '',
                    status: false,
                    timeSpent: '00:00',
                    timeRemaining: '20:00',
                    progressStatus: 0,
                    targetTime: '20:00',
                    onboardingLabel: 'inactive',
                    onboardingColor: 'lightGrey darken-2',
                    label: 'switchStatus',

                },
                {
                    key: 'iugohome',
                    title: 'iUGO Home',
                    type: 'iugoHome',
                    status: true,
                    provider: 'Dr. Michael Vanier',
                    monitoringList: [
                        {
                            label:'fallMonitoring',
                            value: true,
                        },
                        {


                            label: 'sosMonitoring',
                            value: true,
                        },
                        {
                            label: 'geofenceMonitoring',

                            value: true,
                        },
                    ],
                    since: [
                      {
                        key: 'iugohome',
                        label:'since',
                        value: `${timestampToString(subtractFromToday(0, 2, 2), 'L')}`,
                        number: '2',
                        period: 'months',
                      },
                    ],
                    label: 'switchStatus',
                },
            ],
        }),
    });
</script>
