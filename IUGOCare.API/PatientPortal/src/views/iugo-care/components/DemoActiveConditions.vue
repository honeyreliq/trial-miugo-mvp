<template>
  <BaseCard :title=  "$t('activeConditions')">
    <v-data-table
        :headers="headers"
        :items='activeConditionItems'
        class="striped"
        hide-default-footer
    >
      <template v-slot:[`item.enrolledPrograms`]="{ item }">
        <v-chip small class="ma-1" color="primary" v-for="program in item.enrolledPrograms" :key="program">
          {{program}}
        </v-chip>
      </template>
    </v-data-table>
  </BaseCard>
</template>

<script>
export default {
  name: 'DemoActiveConditions',
  data() {
    return{
       activeConditionItems: [
        {
          activeConditions: this.$t('hypertension'),
          diagnosisCode: this.$t('hypertensionCode'),
          enrolledPrograms: ['RPM', 'CCM'],
        },
        {
          activeConditions: this.$t('dementia'),
          diagnosisCode: this.$t('dementiaCode'),
          enrolledPrograms: [],
        },
        {
          activeConditions: this.$t('diabetesType2'),
          diagnosisCode: this.$t('diabetesCode'),
          enrolledPrograms: ['CCM'],
        },
      ],
      headers: [
        { text: this.$t('activeConditions'), value:'activeConditions', class: 'text-subtitle-1' },
        { text: this.$t('diagnosisCode'), value: 'diagnosisCode', class: 'text-subtitle-1' },
        { text: this.$t('enrolledPrograms'), value: 'enrolledPrograms', class: 'text-subtitle-1' },
      ],
    };
  },
};
</script>
<style lang="scss">
.v-data-table-header th {
  white-space: nowrap;
}
</style>
