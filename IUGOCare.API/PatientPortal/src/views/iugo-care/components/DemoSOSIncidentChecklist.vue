<template>
  <BaseCard title="Checklist" :input="updateModel()">
    <BaseChecklist disabled
                   :checked="sosAlert.triggered">
      SOS detected
      <span class="text-subtitle-2">
        ({{ sosAlert.time }})
      </span>
    </BaseChecklist>
    <BaseChecklist disabled
                   :checked="sosCallInformation.callStarted">
      Call established
      <span class="text-subtitle-2" v-if="sosCallInformation.time">
        ({{ sosCallInformation.time }})
      </span>
    </BaseChecklist>
    <BaseChecklist disabled
                   :checked="model.assistanceRequired === 'Yes'"
                   :notrequired="model.assistanceRequired === 'No'">
      Immediate assistance required
    </BaseChecklist>
    <BaseChecklist disabled
                   :checked="model.descriptionEvent">
      Description of the event
    </BaseChecklist>
    <BaseChecklist disabled
                   :checked="model.followUpItems">
      Follow up items
    </BaseChecklist>
  </BaseCard>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapMutations } from 'vuex';
import { IFollowUp, IIncidentDetails} from '@/store/demo';

export default Vue.extend({
  name: 'DemoSOSIncidentChecklist',
  data(): any {
    return {
      model: {
        assistanceRequired: null,
        descriptionEvent: null,
        followUpItems: null,
      },
    };
  },
  computed: {
    ...mapGetters('demo', ['sosFormIncidentDetails', 'sosFormFollowUp', 'sosAlert', 'sosCallInformation']),
  },
  watch: {
    sosFormIncidentDetails(incidentDetails: IIncidentDetails): void {
      this.checkIncidentDetails(incidentDetails);
    },
    sosFormFollowUp(flowUp: IFollowUp): void {
      this.checkFollowUp(flowUp);
    },
  },
  methods: {
    ...mapMutations('demo', {
      setSOSCheckList: 'setSOSCheckList',
    }),

    updateModel(): void {
      this.setSOSCheckList(this.model);
    },

    checkIncidentDetails(incidentDetails: IIncidentDetails): void {
      this.model.assistanceRequired = incidentDetails.immediateAttention;
      this.model.descriptionEvent = !!(incidentDetails.description !== '' &&
          incidentDetails.issue !== '' &&
          incidentDetails.immediateAttention !== null &&
          incidentDetails.issueResolved !== null);

    },
    checkFollowUp(followUp: IFollowUp): void {
      this.model.followUpItems =
          (((followUp.partiesContacted.patient) ||
                  (followUp.partiesContacted.caregiver) ||
                  (followUp.partiesContacted.primaryCareProvider) ||
                  (followUp.partiesContacted.emergencyServices)) &&
              (this.checkFollowUpOtherField(followUp))
          );
    },
    checkFollowUpOtherField(followUp: IFollowUp): boolean {
      if (!followUp.partiesStillToBeContacted.other) {
        return (followUp.partiesStillToBeContacted.caregiver
            || followUp.partiesStillToBeContacted.primaryCareProvider);
      }
      return (followUp.partiesStillToBeContacted.otherDetails !== '');
    },
  },
});
</script>
