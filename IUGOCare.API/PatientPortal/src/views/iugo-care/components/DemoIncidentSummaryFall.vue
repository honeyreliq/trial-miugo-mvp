<template>
  <v-row justify="center">
    <v-dialog
      max-width="600px"
      :value="value"
      @input="$emit('input')"
      scrollable
      persistent>
      <v-card>
        <BaseToolbar title="Incident Summary - Fall"></BaseToolbar>
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
            <v-row  :key="check.name" v-if="check.display">
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
                        :key="question.question" class="mb-4" v-if="question.answer">
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
    import {IFallCheckList, IFallFormModel} from '@/store/demo';

    export default Vue.extend({
        name: 'DemoIncidentSummaryFall',
        components: {},
        props: ['value'],
        computed: {
            ...mapState('patients', ['currentPatient']),
            ...mapGetters('demo', ['fallCheckList', 'fallForm']),
        },
        watch: {
            fallCheckList(fallCheckList: IFallCheckList): void {
                this.model.checklist = [
                    {
                        name: fallCheckList.fallDetected.time ? 'Fall Detected at ' : 'Fall Detected',
                        time: fallCheckList.fallDetected ? fallCheckList.fallDetected.time : null,
                        display: fallCheckList.fallDetected ? !!fallCheckList.fallDetected.triggered : false,
                    },
                    {
                        name: 'Call Established at ',
                        time: fallCheckList.callEstablished ? fallCheckList.callEstablished.time : null,
                        display: fallCheckList.callEstablished ? !!fallCheckList.callEstablished.callStarted : false,
                    },
                    {
                        name: 'Fall Occurred',
                        display: !!fallCheckList.fallOccurred,
                    },
                    {
                        name: 'Immediate assistance required',
                        display: !!fallCheckList.assistanceRequired,
                    },
                    {
                        name: 'Head Injury Incurred',
                        display: !!fallCheckList.headInjuryIncurred,
                    },
                    {
                        name: 'Injury Incurred',
                        display: !!fallCheckList.injuryIncurred,
                    },
                    {
                        name: 'Location of fall',
                        display: fallCheckList.locationFall !== null,
                    },
                    {
                        name: 'Type of fall',
                        display: fallCheckList.typeFall !== null,
                    },
                    {
                        name: 'Description of the event',
                        display: !!fallCheckList.descriptionEvent,
                    },
                    {
                        name: 'Follow up items',
                        display: !!fallCheckList.followUpItems,
                    },
                ];
            },
            fallForm(fallForm: IFallFormModel): void {
                this.model.form = [
                    {
                        label: 'Contact Information',
                        displaySection: (fallForm.contactInformation.confirmedIdentity ||
                            fallForm.contactInformation.fallOccur ||
                            fallForm.contactInformation.injuryIncurred),
                        questions: [
                            {
                                question: 'Confirmed identity with:',
                                answer: fallForm.contactInformation.confirmedIdentity,
                                type: 'short',
                            },
                            {
                                question: 'Did a fall occur?',
                                answer: fallForm.contactInformation.fallOccur,
                                type: 'short',
                            },
                            {
                                question: 'Was injury incurred?',
                                answer: fallForm.contactInformation.injuryIncurred,
                                type: 'short',
                            },
                        ],
                    },
                    {
                        label: 'Injury',
                        displaySection: (fallForm.contactInformation.injuryIncurred &&
                            (fallForm.injury.patientHitHead || fallForm.injury.injured ||
                                fallForm.injury.immediateAttention)),
                        questions: [
                            {
                                question: 'Did the patient hit their head?',
                                answer: fallForm.injury.patientHitHead,
                                type: 'short',
                            },
                            {
                                question: 'What was injured?',
                                answer: fallForm.injury.injured,
                                type: 'long',
                            },
                            {
                                question: 'Do they need immediate attention?',
                                answer: fallForm.injury.immediateAttention,
                                type: 'short',
                            },
                        ],
                    },
                    {
                        label: 'Assessment Details',
                        displaySection: fallForm.assessmentDetails.answer,
                        questions: [
                            {
                                question: 'Please describe the details of the assessment.',
                                answer: fallForm.assessmentDetails.answer,
                                type: 'long',
                            },
                        ],
                    },
                    {
                        label: 'Incident Details',
                        displaySection: (fallForm.incidentDetails.whereFallOccurred ||
                            fallForm.incidentDetails.whereFallOccurredDetail || fallForm.incidentDetails.type.value ||
                            fallForm.incidentDetails.whenFallOccurred || fallForm.incidentDetails.actions ||
                            fallForm.incidentDetails.description),
                        questions: [
                            {
                                question: 'Where did the fall occur?',
                                answer: fallForm.incidentDetails.whereFallOccurred,
                                type: 'short',
                            },
                            {
                                question: 'Please specify where the fall occurred.',
                                answer: fallForm.incidentDetails.whereFallOccurredDetail,
                                type: 'long',
                            },
                            {
                                question: 'What type of fall occurred?',
                                answer: fallForm.incidentDetails.type.value,
                                type: 'short',
                            },
                            {
                                question: 'Please specify the type of fall.',
                                answer: fallForm.incidentDetails.type.answer,
                                type: 'long',
                            },
                            {
                                question: 'When did the fall occur?',
                                answer: fallForm.incidentDetails.whenFallOccurred,
                                type: 'short',
                            },
                            {
                                question: 'What actions lead up to the incident?',
                                answer: fallForm.incidentDetails.actions,
                                type: 'long',
                            },
                            {
                                question: 'Give a brief description of the event incident.',
                                answer: fallForm.incidentDetails.description,
                                type: 'long',
                            },
                        ],
                    },
                    {
                        label: 'Follow Up',
                        displaySection: (fallForm.followUp.partiesContacted.length > 0 ||
                            fallForm.followUp.partiesToBeContacted.length > 0),
                        questions: [
                            {
                                question: 'Identify the parties contacted:',
                                answer: fallForm.followUp.partiesContacted.join(', '),
                                type: 'short',
                            },
                            {
                                question: 'Identify the parties still to be contacted:',
                                answer: fallForm.followUp.partiesToBeContacted.join(', '),
                                type: 'short',
                            },
                            {
                                question: 'Please specify other parties to be contacted.',
                                answer: fallForm.followUp.answer,
                                type: 'long',
                            },
                        ],
                    },
                ];
            },
        },
        methods: {
          ...mapMutations('demo', {
            cleanFallStore: 'cleanFallStore',
            showSuccessMessage: 'showSuccessMessage',
          }),
          setColumns(type: string) {
              return type === 'short' ? 6 : 12;
          },
          cancel() {
              this.$emit('input');
          },
          save() {
            this.$emit('input');
            this.$router.push({path: '/iugo-care/'});
            setTimeout(() => {
              this.cleanFallStore();
              this.showSuccessMessage('fallWorkspace');
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
                },
            };
        },
    });
</script>
