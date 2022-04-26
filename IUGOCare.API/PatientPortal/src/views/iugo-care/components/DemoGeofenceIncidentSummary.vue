<template>
  <v-row justify="center">
    <v-dialog
        max-width="600px"
        :value="value"
        @input="$emit('input')"
        scrollable
        persistent>
      <v-card>
        <BaseToolbar title="Incident Summary - Geofence Violation"></BaseToolbar>
        <v-divider></v-divider>
        <v-card-text>
          <section class="ml-4">
            <v-row>
              <div class="text-h6">{{ model.patientInformation.name }} - {{ model.patientInformation.age }}
                {{ model.patientInformation.gender }}
                - {{ model.patientInformation.birthDate }} - {{ model.patientInformation.language }}
              </div>
            </v-row>
          </section>
          <section class="ml-2 mt-5">
            <BaseGoogleMap :home-location="{lat: 43.235701, lng: 43.235701}"
                           :patient-location="{lat: 43.237345, lng: -79.786272}"
                           :event-time="geofenceAlert.triggered ? `${geofenceAlert.date} @ ${geofenceAlert.time}` : ''"
            ></BaseGoogleMap>
          </section>
          <section class="ml-4 mt-5">
            <template v-for="check in model.checklist" >
              <v-row :key="check.name" v-if="check.time || check.answered">
                <div class="text-subtitle-2">{{ check.name }}<span v-if="check.time"> at {{ check.time }}</span></div>
              </v-row>
            </template>
          </section>
          <section class="ml-4 mt-5">
            <template v-for="frm in model.form">
              <v-row :key="frm.label" v-if="frm.render">
                <v-col cols="12" class="ml-0 mb-3">
                  <v-row>
                    <v-divider class="my-2"></v-divider>
                  </v-row>
                  <v-row>
                    <div class="text-h6">{{ frm.label }}</div>
                  </v-row>
                </v-col>
                <template v-for="question in frm.questions">
                  <v-col cols="12" :sm="setColumns(question.type)"
                        :key="question.question" class="mb-4" v-if="question.answer">
                    <v-row v-if="question.answer">
                      <div>{{ question.question }}</div>
                    </v-row>
                    <v-row v-if="question.answer">
                      <div v-html="question.answer"></div>
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
          <div class="caption">By: {{ model.user }}</div>
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
import {mapGetters, mapMutations, mapState} from 'vuex';
import {IGeofenceCheckList, IGeofenceFormModel} from '@/store/demo';

export default Vue.extend({
  name: 'DemoGeofenceIncidentSummary',
  computed: {
    ...mapState('demo', ['geofenceWorkspace']),
    ...mapGetters('demo', ['geofenceChecklist', 'geofenceForm', 'geofenceAlert']),
  },
  watch: {
    geofenceChecklist(checkList: IGeofenceCheckList): void {
      this.model.checklist[0].time = checkList.violationDetected.time;
      this.model.checklist[1].time = checkList.patientLocated.time;
      this.model.checklist[2].time = checkList.callEstablished.time;
      this.model.checklist[3].answered = checkList.isEventDescriptionCompleted;
      this.model.checklist[4].answered = checkList.isFollowUpCompleted;
    },
    geofenceForm(form: IGeofenceFormModel): void {
      this.extractDataFromForm(form);
    },
  },
  props: ['value'],
  data(): any {
    return {
      saveSuccess: false,
      renderContactInformation: false,
      renderIncidentDetails: false,
      renderFollowUp: false,
      model: {
        user: 'David Smith',
        patientInformation: {
          name: 'Caron, Monique Alicia (Alicia)',
          age: '67',
          gender: 'F',
          birthDate: '04/28/1953',
          language: 'English',
        },
        checklist: [
          {
            name: 'Geofence Violation Detected',
            time: null,
          },
          {
            name: 'Patient Located',
            time: null,
          },
          {
            name: 'Call Established',
            time: null,
          },
          {
            name: 'Description of the event',
            answered: false,
          },
          {
            name: 'Follow up items',
            answered: false,
          },
        ],
        form: [
          {
            render: false,
            label: 'Contact Information',
            questions: [
              {
                question: 'Established contact with:',
                answer: '',
                type: 'short',
              },
              {
                question: 'Were other parties contacted as well?',
                answer: 'Yes',
                type: 'short',
              },
              {
                question: 'Please specify other parties contacted',
                answer: '',
                type: 'long',
              },
            ],
          },
          {
            render: false,
            label: 'Incident Details',
            questions: [
              {
                question: 'Give a brief factual description of the event',
                answer: '',
                type: 'long',
              },
              {
                question: 'Type of issue',
                answer: '',
                type: 'long',
              },
              {
                question: 'Do they need immediate attention?',
                answer: null,
                type: 'short',
              },
              {
                question: 'Was the patient located?',
                answer: null,
                type: 'short',
              },
              {
                question: 'Was the issue resolved at the end of the call?',
                answer: null,
                type: 'short',
              },
            ],
          },
          {
            render: false,
            label: 'Follow Up',
            questions: [
              {
                question: 'Identify the parties contacted:',
                answer: '',
                type: 'short',
              },
              {
                question: 'Identify the parties still to be contacted:',
                answer: '',
                type: 'short',
              },
              {
                question: 'Please specify',
                answer: '',
                type: 'long',
              },
            ],
          },
        ],
        notes: '',
      },
    };
  },
  methods: {
    ...mapMutations('demo', {
      cleanGeofenceStore: 'cleanGeofenceStore',
      showSuccessMessage: 'showSuccessMessage',
    }),
    extractDataFromForm(form: IGeofenceFormModel): void {
      this.extractContactInformation(form);
      this.extractIncidentDetails(form);
      this.extractFollowUp(form);
    },
    extractContactInformation(form: IGeofenceFormModel): void {
      const data = this.model.form[0];

      // checklist
      const contactWith = form.contactInformation.contactWith;
      const contacts: string[] = [];
      if (contactWith.patient) contacts.push('Patient');
      if (contactWith.caregiver) contacts.push('Caregiver');
      if (contactWith.sunnySideLongTermCareFacility) contacts.push('Sunny Side Long Term Care Facility');
      if (contactWith.emergencyServices) contacts.push('Emergency Services');

      // Established contact with:
      const answer: string = contacts.toLocaleString().replace(/,/gi, ', ');
      data.questions[0].answer = answer;

      // Were other parties contacted as well?
      data.questions[1].answer = form.contactInformation.otherParties;

      // Please specify other parties contacted
      const otherDetails: string = (form.contactInformation.otherParties === 'Yes') ?
          form.contactInformation.otherDetails : '';
      data.questions[2].answer = otherDetails;

      // check if contact information section should be rendered
      data.render = (
          data.questions[0].answer !== '' ||
          data.questions[1].answer
      );
    },
    extractIncidentDetails(form: IGeofenceFormModel): void {
      const data = this.model.form[1];
      const incidentDetails = form.incidentDetails;
      // Give a brief factual description of the event
      data.questions[0].answer = incidentDetails.description.replace(/\n/gi, '</br>');

      // Type of issue
      data.questions[1].answer = incidentDetails.issue;

      // Do they need immediate attention?
      data.questions[2].answer = incidentDetails.immediateAttention;

      // Was the patient located?
      data.questions[3].answer = incidentDetails.patientLocated.located;

      // Was the issue resolved at the end of the call?
      data.questions[4].answer = incidentDetails.issueResolved;

      // check if incident details section should be rendered
      data.render = (
          data.questions[0].answer !== '' ||
          data.questions[1].answer ||
          data.questions[2].answer ||
          data.questions[3].answer ||
          data.questions[4].answer
      );
    },
    extractFollowUp(form: IGeofenceFormModel): void {
      const data = this.model.form[2];

      // Identify the parties contacted:
      const partiesContacted: string[] = [];
      if (form.followUp.partiesContacted.patient)
          partiesContacted.push('Patient');
      if (form.followUp.partiesContacted.caregiver)
          partiesContacted.push('Caregiver(s)');
      if (form.followUp.partiesContacted.primaryCareProvider)
          partiesContacted.push('Primary Care Provider');
      if (form.followUp.partiesContacted.emergencyServices)
          partiesContacted.push('Emergency Services');

      let answer: string = partiesContacted.toLocaleString().replace(/,/gi, ', ');
      data.questions[0].answer = answer;

      // Identify the parties still to be contacted:
      const partiesStillToBeContacted: string[] = [];
      if (form.followUp.partiesStillToBeContacted.caregiver)
        partiesStillToBeContacted.push('Caregiver(s)');
      if (form.followUp.partiesStillToBeContacted.primaryCareProvider)
        partiesStillToBeContacted.push('Primary Care Provider');
      if (form.followUp.partiesStillToBeContacted.other)
        partiesStillToBeContacted.push('Other');

      answer = partiesStillToBeContacted.toLocaleString().replace(/,/gi, ', ');
      data.questions[1].answer = answer;

      // Please specify
      data.questions[2].answer = form.followUp.partiesStillToBeContacted.otherDetails;

      // check if follow up section should be rendered
      data.render = (
          data.questions[0].answer !== '' ||
          data.questions[1].answer !== ''
      );
    },
    setColumns(type: string) {
      return type === 'short' ? 6 : 12;
    },
    cancel() {
      this.$emit('input');
    },
    save() {
      this.saveSuccess = true;
      this.$emit('input');
      this.$router.push({ path : '/iugo-care/' });
      setTimeout(() => {
        this.cleanGeofenceStore();
        this.showSuccessMessage('geofenceWorkspace');
      }, 0);
    },
  },
});
</script>
