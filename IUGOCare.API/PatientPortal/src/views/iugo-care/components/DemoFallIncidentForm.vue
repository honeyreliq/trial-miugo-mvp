<template>
  <BaseCard title="Incident Report">
    <v-form ref="incident-form" :input="updateModel()">
      <section>
        <div class="text-h6">Contact Information</div>
        <div class="text-subtitle-2">Confirmed identity with:</div>
        <v-radio-group v-model="model.contactInformation.confirmedIdentity" hide-details>
          <v-row>
            <v-col cols="12" md="6" v-for="option in identityOptions" :key="option.label">
              <v-radio class="mb-2" :label="option.label" :value="option.value" hide-details/>
            </v-col>
          </v-row>
        </v-radio-group>

        <div class="text-subtitle-2">Did a fall occur?</div>
        <v-radio-group v-model="model.contactInformation.fallOccur" hide-details>
          <v-row>
            <v-col cols="12" md="6" v-for="option in YesNoOptions" :key="option.label">
              <v-radio class="mb-2" :label="option.label" :value="option.value" hide-details/>
            </v-col>
          </v-row>
        </v-radio-group>

        <div class="text-subtitle-2">Was injury incurred?</div>
        <v-radio-group v-model="model.contactInformation.injuryIncurred" hide-details>
          <v-row>
            <v-col cols="12" md="6" v-for="option in YesNoOptions" :key="option.label">
              <v-radio class="mb-2" :label="option.label" :value="option.value" hide-details/>
            </v-col>
          </v-row>
        </v-radio-group>
      </section>
      <section v-if="model.contactInformation.injuryIncurred === 'Yes'">
        <v-divider class="my-6"></v-divider>
        <div class="text-h6">Injury</div>

        <div class="text-subtitle-2">Did the patient hit their head?</div>
        <v-radio-group v-model="model.injury.patientHitHead" hide-details>
          <v-row>
            <v-col cols="12" md="6" v-for="option in YesNoOptions" :key="option.label">
              <v-radio class="mb-2" :label="option.label" :value="option.value" hide-details/>
            </v-col>
          </v-row>
        </v-radio-group>
        <v-row>
          <v-col cols="12">
            <v-text-field :label="'What was injured?'"
                          v-model="model.injury.injured"
                          hide-details
                          maxlength="50"
                          type="text"></v-text-field>
          </v-col>
        </v-row>

        <div class="text-subtitle-2 mt-4">Do they need immediate attention?</div>
        <v-radio-group v-model="model.injury.immediateAttention" hide-details>
          <v-row>
            <v-col cols="12" md="6" v-for="option in YesNoOptions" :key="option.label">
              <v-radio class="mb-2" :label="option.label" :value="option.value" hide-details/>
            </v-col>
          </v-row>
        </v-radio-group>
      </section>
      <section>
        <v-divider class="my-6"></v-divider>
        <div class="text-h6">Assessment Details</div>
        <v-row>
          <v-col cols="12">
            <v-textarea :rows="1"
                        auto-grow
                        hide-details
                        v-model="model.assessmentDetails.answer"
                        :label="'Please describe the details of the assessment.'"></v-textarea>
          </v-col>
        </v-row>
      </section>
      <section>
        <v-divider class="my-6"></v-divider>
        <div class="text-h6">Incident Details</div>
        <v-row>
          <v-col cols="12" class="mt-2">
            <v-select :items="fallOccurOptions"
                      hide-details
                      v-model="model.incidentDetails.whereFallOccurred"
                      :label="'Where did the fall occur?'"></v-select>
            <v-text-field :label="'Please specify where the fall occurred.'"
                          hide-details
                          v-model="model.incidentDetails.whereFallOccurredDetail"
                          maxlength="50"
                          v-if="model.incidentDetails.whereFallOccurred === 'Other'"
                          type="text"></v-text-field>
          </v-col>
        </v-row>
        <div class="text-subtitle-2 mt-6">What type of fall occurred?</div>

        <v-radio-group v-model="model.incidentDetails.type.value" hide-details>
          <v-row>
            <v-col cols="12" md="6" class="mt-2" v-for="option in fallOccurTypes" :key="option.label">
              <v-radio class="mb-2" :label="option.label" :value="option.value" hide-details/>
            </v-col>
          </v-row>
        </v-radio-group>
        <v-row>
          <v-col cols="12">
            <v-text-field :label="'Please specify the type of fall.'"
                          v-model="model.incidentDetails.type.answer"
                          hide-details
                          maxlength="50"
                          v-if="model.incidentDetails.type.value === 'Condition Related' || model.incidentDetails.type.value === 'Other'"
                          type="text"></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" class="mt-2">
            <BaseTimePicker :label="'When did the fall occur?'"
                            hide-details="auto"
                            format="24"
                            dense
                            :initial-value="model.incidentDetails.whenFallOccurred"
                            v-model="model.incidentDetails.whenFallOccurred"></BaseTimePicker>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" class="mt-2">
            <v-text-field :label="'What actions lead up to the incident?'"
                          v-model="model.incidentDetails.actions"
                          hide-details
                          maxlength="50"
                          type="text"></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" class="mt-2">
            <v-textarea :rows="1"
                        auto-grow
                        hide-details
                        v-model="model.incidentDetails.description"
                        :label="'Give a brief description of the event incident.'"></v-textarea>
          </v-col>
        </v-row>
      </section>
      <section>
        <v-divider class="my-6"></v-divider>
        <div class="text-h6">Follow Up</div>

        <div class="text-subtitle-2">Identify the parties contacted:</div>
        <v-row class="mb-6">
          <v-col cols="12" md="6" v-for="option in partiesContacted" :key="option.label">
            <v-checkbox :label="option.label" :value="option.value" hide-details
                        v-model="model.followUp.partiesContacted"/>
          </v-col>
        </v-row>

        <div class="text-subtitle-2">Identify the parties still to be contacted:</div>
        <v-row>
          <v-col cols="12" md="6" v-for="option in partiesToBeContacted" :key="option.label">
            <v-checkbox :label="option.label" :value="option.value" hide-details
                        v-model="model.followUp.partiesToBeContacted"/>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" class="mt-2">
            <v-text-field :label="'Please specify other parties to be contacted.'"
                          hide-details
                          maxlength="50"
                          v-model="model.followUp.answer"
                          v-if="model.followUp.partiesToBeContacted.includes('Other')"
                          type="text"></v-text-field>
          </v-col>
        </v-row>
      </section>

      <!-- Form options -->
      <section>
        <v-row>
          <v-spacer></v-spacer>
          <v-col cols="12" class="d-flex justify-end mt-4">
            <BaseButton class="mr-4" color="transparent">{{$t("cancel")}}</BaseButton>
            <BaseButton class="mr-4" color="primary" @click.stop="incidentSummary = true">{{$t("submit")}}</BaseButton>
          </v-col>
        </v-row>
      </section>
    </v-form>
    <DemoIncidentSummaryFall v-model="incidentSummary"></DemoIncidentSummaryFall>
  </BaseCard>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapMutations, mapState } from 'vuex';
import DemoIncidentSummaryFall from '@/views/iugo-care/components/DemoIncidentSummaryFall.vue';

let componentLoaded = false;
export default Vue.extend({
  name: 'DemoFallIncidentForm',
  components: {DemoIncidentSummaryFall},
  computed: {
    ...mapState('demo', ['fallWorkspace']),
  },
  data: () => ({
    timeMenu: false,
    incidentSummary: false,
    model: {
      contactInformation: {
        confirmedIdentity: null,
        fallOccur: null,
        injuryIncurred: null,
      },
      injury: {
        patientHitHead: null,
        injured: null,
        immediateAttention: null,
      },
      assessmentDetails: {
        answer: null,
      },
      incidentDetails: {
        whereFallOccurred: null,
        whereFallOccurredDetail: null,
        type: {
          value: null,
          answer: null,
        },
        whenFallOccurred: null,
        actions: null,
        description: null,
      },
      followUp: {
        partiesContacted: [],
        partiesToBeContacted: [],
        answer: null,
      },
    },
    identityOptions: [
      {label: 'Patient', value: 'Patient'},
      {label: 'Caregiver', value: 'Caregiver'},
    ],
    YesNoOptions: [
      {label: 'Yes', value: 'Yes'},
      {label: 'No', value: 'No'},
    ],
    fallOccurOptions: [
      'Home', 'Work', 'Outside', 'Mall', 'Other',
    ],
    fallOccurTypes: [
      {label: 'Slipped/Tripped', value: 'Slipped/Tripped'},
      {label: 'Equipment Related', value: 'Equipment Related'},
      {label: 'Environmental Hazard', value: 'Environmental Hazard'},
      {label: 'Condition Related', value: 'Condition Related'},
      {label: 'Other', value: 'Other'},
    ],
    partiesContacted: [
      {label: 'Patient', value: 'Patient'},
      {label: 'Caregiver', value: 'Caregiver'},
      {label: 'Primary Care Provider', value: 'Primary Care Provider'},
      {label: 'Emergency Services', value: 'Emergency Services'},
    ],
    partiesToBeContacted: [
      {label: 'Caregiver(s)', value: 'Caregiver(s)'},
      {label: 'Primary Care Provider', value: 'Primary Care Provider'},
      {label: 'Other', value: 'Other'},
    ],
  }),
  methods: {
    ...mapMutations('demo', {
      setFallFormModel: 'setFallFormModel',
    }),

    updateModel() {
      if (componentLoaded) {
        if (this.model.contactInformation.injuryIncurred === 'No') {
          this.model.injury.patientHitHead = null;
          this.model.injury.injured = null;
          this.model.injury.immediateAttention = null;
        }

        if (this.model.incidentDetails.whereFallOccurred !== 'Other') {
          this.model.incidentDetails.whereFallOccurredDetail = null;
        }

        if (this.model.incidentDetails.type.value !== 'Other') {
          this.model.incidentDetails.type.answer = null;
        }

        if (!this.model.followUp.partiesToBeContacted.includes('Other')) {
          this.model.followUp.answer = '';
        }
      } else {
        this.model = {...this.fallWorkspace.form};
        componentLoaded = true;
      }

      this.setFallFormModel(this.model);
    },
  },
  beforeMount() {
    componentLoaded = false;
  },
});
</script>
