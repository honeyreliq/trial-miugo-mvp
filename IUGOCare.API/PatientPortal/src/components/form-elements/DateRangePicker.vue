<template>
<v-row>
  <v-col cols="auto">
    <BaseButtonGroup
      class="mb-4"
      v-model="dateRangeSelection"
      @input="setInputValue"
    >
      <BaseButton value="week">
        {{ $t("timePeriodWeek") }}
      </BaseButton>
      <BaseButton value="month">
        {{ $t("timePeriodMonth") }}
      </BaseButton>
      <BaseButton value="6-month">
        {{ $t("timePeriod6month") }}
      </BaseButton>
      <BaseButton value="year">
        {{ $t("timePeriodYear") }}
      </BaseButton>
    </BaseButtonGroup>
  </v-col>
  <v-col class="flex-grow-1">
    <v-row class="flex-nowrap"
      ><v-col cols="auto" class="column-date-range">
        <v-menu
          v-model="dateRangePickerOpen"
          :close-on-content-click="false"
          :nudge-right="40"
          transition="scale-transition"
          offset-y
          min-width="290px"
        >
          <template v-slot:activator="{ on }">
            <div
              class="v-input text-subtitle-1 v-input--hide-details v-input--is-label-active v-input--is-dirty v-input--is-readonly v-text-field v-text-field--is-booted"
            >
              <div class="v-input__prepend-outer">
                <div class="v-input__icon v-input__icon--prepend">
                  <fa-icon :icon="['fal', 'calendar-alt']" class="fa-fw mb-2" />
                </div>
              </div>
              <div v-on="on" class="v-input__control" justify-end>
                <div class="v-input__slot">
                  <div class="v-text-field__slot">
                    <label
                      class="v-label v-label--active date-range"
                      >{{ $t("dateRange") }}</label
                    >
                    <span
                      >{{ formatDate(value[0]) }} ~
                      {{ formatDate(value[1]) }}</span
                    >
                  </div>
                </div>
              </div>
            </div>
          </template>
          <v-date-picker
            range
            v-model="value"
            @change="setCustomDateRange"
            :locale="$i18n.locale"
            :max="nowDate"
          ></v-date-picker>
        </v-menu>
      </v-col>
      <v-spacer></v-spacer>
      <v-col cols="auto">
        <BaseButton class="primary" @click="print">
          <fa-icon :icon="['fal', 'print']" class="fa-fw" />
        </BaseButton>
      </v-col>
    </v-row>
  </v-col>
</v-row>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapState } from 'vuex';
import { format } from 'date-fns';

export default Vue.extend({
  name: 'DateRangePickAndPrint',

  data() {
    return {
      startDate: '',
      endDate: '',
      value: ['', ''],
      dateRangeSelection: 'week',
      dateRangePickerOpen: false,
      nowDate: new Date().toISOString().slice(0, 10),
    };
  },
  computed: {
    ...mapState('patients', ['currentPatient']),
    prettyPrintDateRange(): string {
      return this.value.map(this.formatDate).join(' ~ ');
    },
  },
  methods: {
    formatDate(date: string): string {
      if (!date) {
        return null;
      }
      const [year, month, day] = date.split('-').map((d) => parseInt(d, 10));
      const dateFormat = this.currentPatient?.dateFormat || 'MM/dd/yyyy';
      return format(new Date(year, (month - 1), day), dateFormat);
    },
    print() {
      this.$emit('print');
    },
    setCustomDateRange() {
      this.dateRangePickerOpen = false;
      this.dateRangeSelection = null;
      this.value = this.value.filter(Boolean).sort((a: string, b: string) =>
        new Date(a).getTime() - new Date(b).getTime());
      this.$emit('input', this.value);
    },
    setInputValue(newValue: string): void {
      this.dateRangeSelection = newValue;
    },
    updateDateRangeSelection() {
      const startDate = new Date();

      switch (this.dateRangeSelection) {
        case 'week':
          startDate.setDate(startDate.getDate() - 7);
          break;
        case 'month':
          startDate.setMonth(startDate.getMonth() - 1);
          break;
        case '6-month':
          startDate.setMonth(startDate.getMonth() - 6);
          break;
        case 'year':
          startDate.setFullYear(startDate.getFullYear() - 1);
          break;
        default:
          /* Custom date range selected; get outta here. */
          return;
      }

      this.value[0] = startDate.toISOString().substr(0, 10);
      this.value[1] = new Date().toISOString().substr(0, 10);
      this.$forceUpdate();
      this.$emit('input', this.value);
    },
  },
  watch: {
    dateRangeSelection() {
      this.updateDateRangeSelection();
    },
  },
  created() {
    this.updateDateRangeSelection();
  },
});
</script>
<style lang="scss">
.date-range {
  left: 0px;
  right: auto;
  position: absolute;
}
</style>
