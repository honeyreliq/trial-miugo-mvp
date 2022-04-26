<template>
  <v-row justify="center">
    <v-dialog
      max-width="600px"
      :value="value"
      @input="$emit('input')"
      scrollable
      persistent>
      <v-card>
        <BaseToolbar title="Incident Summary - SOS"></BaseToolbar>
        <v-divider></v-divider>
        <v-card-text>
          <section class="ml-4">
            <v-row>
              <div class="text-h6">{{model.patientInformation.name}} - {{model.patientInformation.age}}
                {{model.patientInformation.gender}}
                - {{model.patientInformation.birthDate}} - {{model.patientInformation.language}}
              </div>
            </v-row>
          </section>
          <section class="ml-4 mt-5">
            <template v-for="check in model.checklist">
              <v-row :key="check.name" v-if="check.display">
                <div class="text-subtitle-2">{{check.name}} {{check.time}}</div>
              </v-row>
            </template>
          </section>
          <section class="ml-4  mt-5">
            <template v-for="frm in model.form">
              <v-row :key="frm.label" v-if="frm.displaySection">
                <v-col cols="12" class="ml-0 mb-3">
                  <v-row>
                    <v-divider class="my-2"></v-divider>
                  </v-row>
                  <v-row>
                    <div class="text-h6">{{frm.label}}</div>
                  </v-row>
                </v-col>
                <template v-for="question in frm.questions">
                  <v-col cols="12" :sm="setColumns(question.type)"
                        :key="question.question"
                        class="mb-4" v-if="question.answer">
                    <v-row>
                      <div>{{question.question}}</div>
                    </v-row>
                    <v-row>
                      <div>{{question.answer}}</div>
                    </v-row>
                  </v-col>
                </template>
              </v-row>
            </template>
          </section>
          <section class="ml-2">
            <v-row>
              <v-divider class="my-2"></v-divider>
            </v-row>
            <v-textarea :rows="1"
                        auto-grow
                        v-model="model.notes"
                        :label="'Notes'"></v-textarea>
          </section>
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <div class="caption" v-if="currentPatient">By:
            <!-- {{currentPatient.givenName}} {{currentPatient.familyName}} -->
            David Smith
          </div>
          <v-spacer></v-spacer>
          <BaseButton @click.native="cancel()">Cancel</BaseButton>
          <BaseButton color="primary" @click.native="save()">Save</BaseButton>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script lang="ts">
    import Vue from 'vue';
    import {mapState, mapGetters, mapMutations} from 'vuex';
    import {ISOSFormModel, ISOSCheckList} from '@/store/demo';

    export default Vue.extend({
        name: 'DemoSOSIncidentSummary',
        components: {},
        computed: {
            ...mapState('patients', ['currentPatient']),
            ...mapGetters('demo', ['sosCheckList', 'sosForm']),
        },
        props: ['value'],
        watch: {
            sosCheckList(sosCheckList: ISOSCheckList): void {
                this.model.checklist = [
                    {
                        name: 'SOS Detected at ',
                        time: sosCheckList.sosDetected ? sosCheckList.sosDetected.time : null,
                        display: sosCheckList.sosDetected ? !!sosCheckList.sosDetected.triggered : false,
                    },
                    {
                        name: 'Call Established at ',
                        time: sosCheckList.callEstablished ? sosCheckList.callEstablished.time : null,
                        display: sosCheckList.callEstablished ? !!sosCheckList.callEstablished.callStarted : false,
                    },
                    {
                        name: 'Immediate assistance required',
                        display: !!sosCheckList.assistanceRequired,
                    },
                    {
                        name: 'Description of the event',
                        display: !!sosCheckList.descriptionEvent,
                    },
                    {
                        name: 'Follow up items',
                        display: !!sosCheckList.followUpItems,
                    },
                ];
            },
            sosForm(sosForm: ISOSFormModel): void {
                this.model.form = [
                    {
                        label: 'Contact Information',
                        displaySection: (sosForm.contactInformation.confirmedIdentity ||
                            sosForm.contactInformation.otherPartiesContacted ||
                            sosForm.contactInformation.confirmedIdentityDetails),
                        questions: [
                            {
                                question: 'Confirmed identity with:',
                                answer: sosForm.contactInformation.confirmedIdentity,
                                type: 'short',
                            },
                            {
                                question: 'Were other parties contacted as well?',
                                answer: sosForm.contactInformation.otherPartiesContacted,
                                type: 'short',
                            },
                            {
                                question: 'Please specify other parties contacted',
                                answer: sosForm.contactInformation.confirmedIdentityDetails,
                                type: 'long',
                            },
                        ],
                    },
                    {
                        label: 'Incident Details',
                        displaySection: (sosForm.incidentDetails.description ||
                            sosForm.incidentDetails.issue || sosForm.incidentDetails.immediateAttention ||
                            sosForm.incidentDetails.issueResolved),
                        questions: [
                            {
                                question: 'Give a brief factual description of the event',
                                answer: sosForm.incidentDetails.description,
                                type: 'long',
                            },
                            {
                                question: 'Type of issue',
                                answer: sosForm.incidentDetails.issue,
                                type: 'long',
                            },
                            {
                                question: 'Do they need immediate attention?',
                                answer: sosForm.incidentDetails.immediateAttention,
                                type: 'short',
                            },
                            {
                                question: 'Was the issue resolved at the end of the call?',
                                answer: sosForm.incidentDetails.issueResolved,
                                type: 'short',
                            },
                        ],
                    },
                    {
                        label: 'Follow Up',
                        displaySection: ((sosForm.followUp.partiesContacted.patient ||
                            sosForm.followUp.partiesContacted.caregiver ||
                            sosForm.followUp.partiesContacted.primaryCareProvider ||
                            sosForm.followUp.partiesContacted.emergencyServices) ||
                            (sosForm.followUp.partiesStillToBeContacted.caregiver ||
                                sosForm.followUp.partiesStillToBeContacted.primaryCareProvider ||
                                sosForm.followUp.partiesStillToBeContacted.other ||
                                sosForm.followUp.partiesStillToBeContacted.otherDetails)),
                        questions: [
                            {
                                question: 'Identify the parties contacted:',
                                answer: this.getPartiesContacted(sosForm.followUp.partiesContacted),
                                type: 'short',
                            },
                            {
                                question: 'Identify the parties still to be contacted:',
                                answer: this.getPartiesStillToBeContacted(sosForm.followUp.partiesStillToBeContacted),
                                type: 'short',
                            },
                            {
                                question: 'Please specify',
                                answer: this.getPartiesStillToBeContacted(sosForm.followUp.partiesStillToBeContacted)
                                    .includes('Other') ? sosForm.followUp.partiesStillToBeContacted.otherDetails : null,
                                type: 'long',
                            },
                        ],
                    },
                ];
            },
        },
        methods: {
          ...mapMutations('demo', {
            cleanSOSStore: 'cleanSOSStore',
            showSuccessMessage: 'showSuccessMessage',
          }),
          getPartiesContacted(partiesContacted: any): string {
              const elements = [];
              if (partiesContacted.patient) {
                  elements.push('Patient');
              }
              if (partiesContacted.caregiver) {
                  elements.push('Caregiver');
              }
              if (partiesContacted.primaryCareProvider) {
                  elements.push('Primary Care Provider');
              }
              if (partiesContacted.emergencyServices) {
                  elements.push('Emergency Service');
              }
              return elements.join(', ');
          },
          getPartiesStillToBeContacted(partiesStillToBeContacted: any): string {
              const elements = [];
              if (partiesStillToBeContacted.caregiver) {
                  elements.push('Caregiver');
              }
              if (partiesStillToBeContacted.primaryCareProvider) {
                  elements.push('Primary Care Provider');
              }
              if (partiesStillToBeContacted.other) {
                  elements.push('Other');
              }
              return elements.join(', ');
          },
          setColumns(type: string): number {
              return type === 'short' ? 6 : 12;
          },
          cancel(): void {
              this.$emit('input');
          },
          save(): void {
            this.$emit('input');
            this.$router.push({path: '/iugo-care/'});
            setTimeout(() => {
              this.cleanSOSStore();
              this.showSuccessMessage('sosWorkspace');
            }, 0);
          },
        },
        data(): any {
            return {
                model: {
                    patientInformation: {
                        name: 'Caron, Monique Alicia (Alicia)',
                        age: '67',
                        gender: 'F',
                        birthDate: '04/28/1953',
                        language: 'English',
                    },
                    checklist: [],
                    form: [],
                    notes: '',
                    user: {},
                },
            };
        },
    });
</script>
