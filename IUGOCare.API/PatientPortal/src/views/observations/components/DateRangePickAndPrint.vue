<template>
  <v-row class="fill-height">
    <v-col cols="auto" class="flex-grow-1 align-self-center">
      <BaseDateRangeFilter
        :week="$t('timePeriodWeek')"
        :month="$t('timePeriodMonth')"
        :sixMonth="$t('timePeriod6month')"
        :year="$t('timePeriodYear')"
        :to="$t('to')"
        :from="$t('from')"
        @updateDateRange="updateDateRange"
      >
      </BaseDateRangeFilter>
    </v-col>
    <!-- <v-col cols="auto" class="d-flex justify-end">
      <BaseButton class="primary" @click="print">
        <fa-icon :icon="['fal', 'print']" class="fa-fw" />
      </BaseButton>
    </v-col> -->
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'DateRangePickAndPrint',
  methods: {
    print() {
      this.$emit('print');
    },
    updateDateRange() {
      // This needs to be handled by parent component or Vuex
      this.$emit('dateRangeChanged');
    },
  },
});
</script>

<style lang="scss" scoped>
.date-range {
  left: 0px;
  right: auto;
  position: absolute;
}
</style>
