<template>
  <BaseCard title="Checklist" :input="updateModel()">
    <BaseChecklist disabled :checked="fallAlert.triggered">
      Fall detected
      <span class="text-subtitle-2" v-if="fallAlert.triggered">
        ({{fallAlert.time}})
      </span>
    </BaseChecklist>
    <BaseChecklist disabled
                   :checked="fallCallInformation.callStarted">
      Call established
      <span class="text-subtitle-2" v-if="fallCallInformation.time">
        ({{fallCallInformation.time}})
      </span>
    </BaseChecklist>
    <BaseChecklist disabled
                   :checked="model.fallOccurred === 'Yes'"
                   :notrequired="model.fallOccurred === 'No'">
      Fall occurred
    </BaseChecklist>
    <BaseChecklist disabled
                   :checked="model.assistanceRequired === 'Yes'"
                   :notrequired="model.injuryIncurred === 'No' || model.assistanceRequired === 'No'">
      Immediate assistance required
    </BaseChecklist>
    <BaseChecklist disabled
                   :checked="model.headInjuryIncurred === 'Yes'"
                   :notrequired="model.injuryIncurred === 'No' || model.headInjuryIncurred === 'No'">
      Head injury incurred
    </BaseChecklist>
    <BaseChecklist disabled
                   :checked="model.injuryIncurred === 'Yes'"
                   :notrequired="model.injuryIncurred === 'No'">
      Injury incurred
    </BaseChecklist>
    <BaseChecklist disabled
                   :checked="model.locationFall"
                   :notrequired="model.fallOccurred === 'No'">
      Location of fall
    </BaseChecklist>
    <BaseChecklist disabled
                   :checked="model.typeFall"
                   :notrequired="model.fallOccurred === 'No'">
      Type of fall
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
    import {mapGetters, mapMutations} from 'vuex';
    import {
        IAlertInformation,
        IContactInformation,
        IInjury,
        IFallIncidentDetails,
        IFallFollowUp,
    } from '@/store/demo';

    export default Vue.extend({
        name: 'DemoFallIncidentChecklist',
        data(): any {
            return {
                model: {
                    fallDetected: {
                        time: null,
                        triggered: null,
                    },
                    fallOccurred: null,
                    assistanceRequired: null,
                    headInjuryIncurred: null,
                    injuryIncurred: null,
                    locationFall: null,
                    typeFall: null,
                    descriptionEvent: null,
                    followUpItems: null,
                },
            };
        },
        computed: {
            ...mapGetters('demo', ['fallAlert', 'fallFormContactInformation', 'fallFormInjury',
                'fallFormIncidentDetails', 'fallFormFollowUp', 'fallCallInformation']),
        },
        watch: {
            fallAlert(alert: IAlertInformation): void {
                this.model.fallDetected = {
                    triggered: this.model.fallOccurred,
                    time: alert.time,
                };
            },
            fallFormContactInformation(contactInformation: IContactInformation): void {
                this.checkContactInformation(contactInformation);
            },
            fallFormInjury(injury: IInjury): void {
                this.checkInjury(injury);
            },
            fallFormIncidentDetails(incidentDetails: IFallIncidentDetails): void {
                this.checkIncidentDetails(incidentDetails);
            },
            fallFormFollowUp(followup: IFallFollowUp): void {
                this.checkFollowUp(followup);
            },
        },
        methods: {
            ...mapMutations('demo', {
                setFallCheckList: 'setFallCheckList',
            }),
            updateModel(): void {
                this.setFallCheckList(this.model);
            },
            checkContactInformation(contactInformation: IContactInformation): void {
                this.model.fallDetected.triggered = contactInformation.fallOccur;
                this.model.fallOccurred = contactInformation.fallOccur;
                this.model.injuryIncurred = contactInformation.injuryIncurred;
            },
            checkInjury(injury: IInjury): void {
                this.model.assistanceRequired = injury.immediateAttention;
                this.model.headInjuryIncurred = injury.patientHitHead;
            },
            checkIncidentDetails(incidentDetails: IFallIncidentDetails): void {
                this.model.locationFall = (incidentDetails.whereFallOccurred !== null);
                this.model.typeFall = (this.model.fallOccurred === null || this.model.fallOccurred === 'Yes') ?
                    (incidentDetails.type.value !== null)  : false;
                this.model.descriptionEvent = (this.checkWhereFallOccurred(incidentDetails) &&
                    this.checkFallType(incidentDetails) && incidentDetails.actions !== null &&
                    incidentDetails.whenFallOccurred !== null && incidentDetails.description !== null);
            },
            checkWhereFallOccurred(incidentDetails: IFallIncidentDetails): boolean {
                if (incidentDetails.whereFallOccurred !== null) {
                    if (incidentDetails.whereFallOccurred === 'Other') {
                        return incidentDetails.whereFallOccurredDetail !== '';
                    } else {
                        return true;
                    }
                }
            },
            checkFallType(incidentDetails: IFallIncidentDetails): boolean {
                if (incidentDetails.type.value !== null) {
                    if (incidentDetails.type.value === 'Other') {
                        return incidentDetails.type.answer !== '';
                    } else {
                        return true;
                    }
                }
            },
            checkFollowUp(followUp: IFallFollowUp): void {
                this.model.followUpItems =
                    ((followUp.partiesContacted.length > 0) && (this.checkFollowUpOtherField(followUp)));
            },
            checkFollowUpOtherField(followUp: IFallFollowUp): boolean {
                if (followUp.partiesToBeContacted.length > 0) {
                    if (followUp.partiesToBeContacted.includes('Other')) {
                        return (followUp.answer !== '');
                    } else {
                        return true;
                    }
                }
            },
        },
    });
</script>
