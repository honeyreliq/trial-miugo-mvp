<template>
  <BaseCard title="Incident Report">
    <v-form ref="sos-form" :input="updateModel()">
      <!-- Contact Information -->
      <section>
        <div class="text-h6">Contact Information</div>
        <!-- Confirmed identity with  -->
        <section>
          <div class="text-subtitle-2">Confirmed identity with:</div>
          <v-radio-group v-model="model.contactInformation.confirmedIdentity" hide-details>
            <v-row>
              <v-col md="6" cols="12">
                <v-radio class="mb-2"
                         label="Patient"
                         value="Patient"
                         name="confirmedIdentity"
                         hide-details
                ></v-radio>
              </v-col>
              <v-col md="6" cols="12">
                <v-radio class="mb-2"
                         label="Caregiver"
                         value="Caregiver"
                         name="confirmedIdentity"
                         hide-details
                ></v-radio>
              </v-col>
            </v-row>
          </v-radio-group>
        </section>
        <!-- Were other parties contacted as well? -->
        <section>
          <div class="text-subtitle-2">Were other parties contacted as well?</div>
          <v-radio-group v-model="model.contactInformation.otherPartiesContacted" hide-details>
            <v-row>
              <v-col md="6" cols="12">
                <v-radio class="mb-2"
                         label="Yes"
                         value="Yes"
                         name="otherPartiesContacted"
                         hide-details
                ></v-radio>
              </v-col>
              <v-col md="6" cols="12">
                <v-radio class="mb-2"
                         label="No"
                         value="No"
                         name="otherPartiesContacted"
                         hide-details
                ></v-radio>
              </v-col>
              <v-col cols="12">
                <v-text-field v-if="(model.contactInformation.otherPartiesContacted === 'Yes')"
                              v-model="model.contactInformation.confirmedIdentityDetails"
                              hide-details
                              maxlength="50"
                              label="Please specify other parties contacted"
                ></v-text-field>
              </v-col>
            </v-row>
          </v-radio-group>
        </section>
      </section>

      <!-- Incident Details -->
      <section>
        <v-divider class="my-6"></v-divider>
        <div class="text-h6">Incident Details</div>
        <!-- Give a brief factual description of the event: -->
        <v-row>
          <v-col cols="12">
            <v-textarea
                v-model="model.incidentDetails.description"
                rows="1"
                name="incidentDetails"
                label="Give a brief factual description of the event"
                auto-grow
                hide-details
                value=""
            ></v-textarea>
          </v-col>
        </v-row>
        <!-- Type of issue -->
        <v-row>
          <v-col cols="12">
            <v-text-field v-model="model.incidentDetails.issue"
                          label="Type of issue"
                          hide-details
                          maxlength="50"
            ></v-text-field>
          </v-col>
        </v-row>
        <!-- Do they need immediate attention?  -->
        <section>
          <div class="text-subtitle-2 mt-6">Do they need immediate attention?</div>
          <v-radio-group v-model="model.incidentDetails.immediateAttention" hide-details>
            <v-row>
              <v-col md="6" cols="12">
                <v-radio class="mb-2"
                         label="Yes"
                         value="Yes"
                         hide-details
                         name="needImmediateAttention"
                ></v-radio>
              </v-col>
              <v-col md="6" cols="12">
                <v-radio class="mb-2"
                         label="No"
                         value="No"
                         hide-details
                         name="needImmediateAttention"
                ></v-radio>
              </v-col>
            </v-row>
          </v-radio-group>
        </section>
        <!-- Was the issue resolved at the end of the call?  -->
        <section>
          <div class="text-subtitle-2">Was the issue resolved at the end of the call?</div>
          <v-radio-group v-model="model.incidentDetails.issueResolved" hide-details>
            <v-row>
              <v-col md="6" cols="12">
                <v-radio class="mb-2"
                         label="Yes"
                         value="Yes"
                         hide-details
                         name="issueResolved"
                ></v-radio>
              </v-col>
              <v-col md="6" cols="12">
                <v-radio class="mb-2"
                         label="No"
                         value="No"
                         hide-details
                         name="issueResolved"
                ></v-radio>
              </v-col>
            </v-row>
          </v-radio-group>
        </section>
      </section>

      <!-- Follow Up -->
      <section>
        <v-divider class="my-6"></v-divider>
        <div class="text-h6">Follow Up</div>
        <!-- Identify the parties contacted: -->
        <section>
          <div class="text-subtitle-2">Identify the parties contacted:</div>
          <v-row>
            <v-col md="6" cols="12">
              <v-checkbox
                  v-model="model.followUp.partiesContacted.patient"
                  hide-details
                  label="Patient"
              ></v-checkbox>
              <v-checkbox
                  v-model="model.followUp.partiesContacted.caregiver"
                  hide-details
                  label="Caregiver(s)"
              ></v-checkbox>
            </v-col>
            <v-col md="6" cols="12">
              <v-checkbox
                  v-model="model.followUp.partiesContacted.primaryCareProvider"
                  hide-details
                  label="Primary Care Provider"
              ></v-checkbox>
              <v-checkbox
                  v-model="model.followUp.partiesContacted.emergencyServices"
                  hide-details
                  label="Emergency Services"
              ></v-checkbox>
            </v-col>
          </v-row>
        </section>
        <!-- Identify the parties still to be contacted: -->
        <section>
          <div class="text-subtitle-2 mt-6">Identify the parties still to be contacted:</div>
          <v-row>
            <v-col md="6" cols="12">
              <v-checkbox
                  v-model="model.followUp.partiesStillToBeContacted.caregiver"
                  hide-details
                  label="Caregiver(s)"
              ></v-checkbox>
            </v-col>
            <v-col md="6" cols="12">
              <v-checkbox
                  v-model="model.followUp.partiesStillToBeContacted.primaryCareProvider"
                  hide-details
                  label="Primary Care Provider"
              ></v-checkbox>
            </v-col>
            <v-col cols="12">
              <v-checkbox
                  v-model="model.followUp.partiesStillToBeContacted.other"
                  hide-details
                  label="Other"
              ></v-checkbox>
            </v-col>
            <v-col cols="12" class="mt-2">
              <v-text-field v-if="(model.followUp.partiesStillToBeContacted.other)"
                            v-model="model.followUp.partiesStillToBeContacted.otherDetails"
                            hide-details
                            maxlength="50"
                            label="Please specify"
              ></v-text-field>
            </v-col>
          </v-row>
        </section>
      </section>

      <!-- Form options -->
      <section>
        <v-row>
          <v-spacer></v-spacer>
          <v-col cols="12" class="d-flex justify-end mt-4">
            <BaseButton class="mr-4" color="transparent">{{$t("cancel")}}</BaseButton>
            <BaseButton class="mr-4" color="primary" @click.stop="sosIncidentSummary = true">{{$t("submit")}}</BaseButton>
          </v-col>
        </v-row>
      </section>
    </v-form>
    <DemoSOSIncidentSummary v-model="sosIncidentSummary"></DemoSOSIncidentSummary>
  </BaseCard>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapMutations, mapState } from 'vuex';
import DemoSOSIncidentSummary from '@/views/iugo-care/components/DemoSOSIncidentSummary.vue';

let componentLoaded = false;
export default Vue.extend({
  name: 'DemoSOSIncidentForm',
  components: {DemoSOSIncidentSummary},
  computed: {
    ...mapState('demo', ['sosWorkspace']),
  },
  watch: {},
  props: {},
  data(): any {
    return {
      sosIncidentSummary: false,
      model: {
        contactInformation: {
          confirmedIdentity: null,
          otherPartiesContacted: null,
          confirmedIdentityDetails: '',
        },
        incidentDetails: {
          description: '',
          issue: '',
          immediateAttention: null,
          issueResolved: null,
        },
        followUp: {
          partiesContacted: {
            patient: false,
            caregiver: false,
            primaryCareProvider: false,
            emergencyServices: false,
          },
          partiesStillToBeContacted: {
            caregiver: false,
            primaryCareProvider: false,
            other: false,
            otherDetails: '',
          },
        },
      },

    };
  },
  methods: {
    ...mapMutations('demo', {
      setSOSFormModel: 'setSOSFormModel',
    }),
    updateModel() {
      if (componentLoaded) {
        if (this.model.contactInformation.otherPartiesContacted === 'No') {
          this.model.contactInformation.confirmedIdentityDetails = '';
        }

        if (!this.model.followUp.partiesStillToBeContacted.other) {
          this.model.followUp.partiesStillToBeContacted.otherDetails = '';
        }
      } else {
        this.model = {...this.sosWorkspace.form};
        componentLoaded = true;
      }
      this.setSOSFormModel(this.model);
    },
  },
  beforeMount() {
    componentLoaded = false;
  },
});
</script>
