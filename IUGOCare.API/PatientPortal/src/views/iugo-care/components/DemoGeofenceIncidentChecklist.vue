<template>
    <BaseCard title="Checklist">
      <BaseChecklist disabled :checked="geofenceAlert.triggered">
        Geofence violation detected
        <span class="text-subtitle-2" v-if="geofenceAlert.triggered">({{ geofenceAlert.time }})</span>
      </BaseChecklist>
      <BaseChecklist disabled
                    :checked="(geofencePatientLocated.located=== 'Yes')"
                    :notrequired="(geofencePatientLocated.located=== 'No')">
        Patient located
        <span class="text-subtitle-2"
              v-if="(geofencePatientLocated.located=== 'Yes')">({{ geofencePatientLocated.time }})</span>
      </BaseChecklist>
      <BaseChecklist disabled :checked="geofenceCallInformation.callStarted">
        Call established
        <span class="text-subtitle-2" v-if="geofenceCallInformation.callStarted">({{ geofenceCallInformation.time }})</span>
      </BaseChecklist>
      <BaseChecklist disabled :checked="isEventDescriptionCompleted">
        Description of the event
      </BaseChecklist>
      <BaseChecklist disabled :checked="isFollowUpCompleted">
        Follow up items
      </BaseChecklist>
    </BaseCard>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapMutations } from 'vuex';
import { IFollowUp, IIncidentDetails } from '@/store/demo';

export default Vue.extend({
  name: 'DemoGeofenceIncidentChecklist',
  data(): any {
    return {
      isEventDescriptionCompleted: false,
      isFollowUpCompleted: false,
      checkList: {
        violationDetected: {triggered: null, time: ''},
        patientLocated: {located: null, time: ''},
        callEstablished: false,
        isEventDescriptionCompleted: false,
        isFollowUpCompleted: false,
      },
    };
  },
  computed: {
    ...mapGetters('demo', ['geofenceFormIncidentDetails',
      'geofenceFormFlowUp',
      'geofencePatientLocated',
      'geofenceAlert',
      'geofenceCallInformation',
    ]),
  },
  watch: {
    geofenceFormIncidentDetails(incidentDetails: IIncidentDetails): void {
      this.checkIncidentDetails(incidentDetails);
    },
    geofenceFormFlowUp(flowUp: IFollowUp): void {
      this.checkFlowUp(flowUp);
    },
  },
  methods: {
    ...mapMutations('demo', {
      setGeofenceChecklist: 'setGeofenceChecklist',
    }),
    updateChecklistData(field: string, value: any): void {
      this.checkList[field] = value;
      this.setGeofenceChecklist({...this.checkList});
    },
    checkIncidentDetails(incidentDetails: IIncidentDetails): void {
      this.isEventDescriptionCompleted = (
          (incidentDetails.description !== '') &&
          (incidentDetails.issue !== '') &&
          (incidentDetails.immediateAttention !== null) &&
          (incidentDetails.patientLocated !== null) &&
          (incidentDetails.issueResolved !== null)
      );

      this.updateChecklistData('isEventDescriptionCompleted', this.isEventDescriptionCompleted);
    },
    checkFlowUp(flowUp: IFollowUp): void {
      this.isFollowUpCompleted = (
          (
              (flowUp.partiesContacted.caregiver) ||
              (flowUp.partiesContacted.emergencyServices) ||
              (flowUp.partiesContacted.patient) ||
              (flowUp.partiesContacted.primaryCareProvider)
          ) &&
          (this.checkOtherFieldsValid(flowUp))
      );
      this.updateChecklistData('isFollowUpCompleted', this.isFollowUpCompleted);
    },
    checkOtherFieldsValid(flowUp: IFollowUp): boolean {
      if (!flowUp.partiesStillToBeContacted.other) {
        // "Other" option could not be selected, but any of the other must to be valid
        return (flowUp.partiesStillToBeContacted.caregiver || flowUp.partiesStillToBeContacted.primaryCareProvider);
      }
      // if "Other" option is selected, must have text in details
      return (flowUp.partiesStillToBeContacted.otherDetails !== '');
    },
  },
});
</script>
