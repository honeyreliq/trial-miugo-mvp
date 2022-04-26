<template>
  <v-row>
    <v-col cols="12" md="auto">
      <BaseButtonGroup
        v-model="dateRangeSelection"
        @input="changeQuickSelectRange"
        class="mb-4 mb-md-0"
      >
        <BaseButton value="week">
          {{ $t('week') }}
        </BaseButton>
        <BaseButton value="month">
          {{ $t('month') }}
        </BaseButton>
        <BaseButton value="6-month">
          {{ $t('sixMonths') }}
        </BaseButton>
        <BaseButton value="year">
          {{ $t('year') }}
        </BaseButton>
      </BaseButtonGroup>
    </v-col>
    <v-col class="flex-grow-1">
      <v-row class="flex-nowrap">
        <v-col cols="auto" class="column-date-range">
          <v-menu
            v-model="showDateFromPicker"
            :close-on-content-click="false"
            transition="scale-transition"
            offset-y
            min-width="auto"
          >
            <template v-slot:activator="{ on, attrs }">
              <v-text-field
                v-model="formattedDateFrom"
                :label="$t('from')"
                v-bind="attrs"
                v-on="on"
                class="mt-3"
                hide-details="true"
                dense
              >
                <fa-icon
                  :icon="['fal', 'calendar-alt']"
                  class="fa-fw"
                  slot="prepend"
                />
              </v-text-field>
            </template>
            <v-date-picker
              v-model="dateFromPickerValue"
              color="primary"
              @input="updateDateFromPicker"
            ></v-date-picker>
          </v-menu>
        </v-col>
        <v-col cols="auto" class="column-date-range">
          <v-menu
            v-model="showDateToPicker"
            :close-on-content-click="false"
            transition="scale-transition"
            offset-y
            min-width="auto"
          >
            <template v-slot:activator="{ on, attrs }">
              <v-text-field
                v-model="formattedDateTo"
                :label="$t('to')"
                v-bind="attrs"
                v-on="on"
                class="mt-3"
                hide-details="true"
                dense
              >
                <fa-icon
                  :icon="['fal', 'calendar-alt']"
                  class="fa-fw"
                  slot="prepend"
                />
              </v-text-field>
            </template>
            <v-date-picker
              v-model="dateToPickerValue"
              color="primary"
              @input="updateDateToPicker"
            ></v-date-picker>
          </v-menu>
        </v-col>
      </v-row>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue';
import { format, subDays, subMonths, subYears } from 'date-fns';

export default Vue.extend({
  name: 'BaseDateRangeFilter',
  props: {
    week: {
      type: String,
      default: 'week'
    },
    month: {
      type: String,
      default: 'month',
    },
    sixMonth: {
      type: String,
      default: 'sixMonths',
    },
    year: {
      type: String,
      default: 'year',
    },
    to: {
      type: String,
      default: 'to',
    },
    from: {
      type: String,
      default: 'from',
    },
  },
  data(): any {
    return {
      // defaults to "1 week ago to today"
      dateFrom: subDays(new Date(), 7).toISOString().substr(0, 10),
      dateTo: new Date().toISOString().substr(0, 10),
      showDateFromPicker: false,
      showDateToPicker: false,
      dateFromPickerValue: null,
      dateToPickerValue: null,
      dateRangeSelection: 'week',
    };
  },
  computed: {
    formattedDateFrom(): string {
      return this.formatDate(this.dateFrom);
    },
    formattedDateTo(): string {
      return this.formatDate(this.dateTo);
    },
  },
  methods: {
    // Change displayed date from Vuetify date to application-specific format
    formatDate(date: string): string {
      if (!date) {
        return null;
      }
      const [year, month, day] = date.split('-').map((d) => parseInt(d, 10));
      const dateFormat = 'MM/dd/yyyy';
      return format(new Date(year, month - 1, day), dateFormat);
    },
    changeQuickSelectRange() {
      // When button value changes, set date range from X days in the past to now
      this.dateTo = new Date().toISOString().substr(0, 10);

      switch (this.dateRangeSelection) {
        case 'week':
          this.dateFrom = subDays(new Date(), 7).toISOString().substr(0, 10);
          break;
        case 'month':
          this.dateFrom = subMonths(new Date(), 1).toISOString().substr(0, 10);
          break;
        case '6-month':
          this.dateFrom = subMonths(new Date(), 6).toISOString().substr(0, 10);
          break;
        case 'year':
          this.dateFrom = subYears(new Date(), 1).toISOString().substr(0, 10);
          break;
        default:
          // deselected a button; set date range to a week by default
          this.dateFrom = subDays(new Date(), 7).toISOString().substr(0, 10);
          break;
      }
      this.$emit('updateDateRange');
    },
    updateDateFromPicker() {
      this.showDateFromPicker = false;
      this.dateRangeSelection = null; // should deselect button but doesn't
      this.dateFrom = this.dateFromPickerValue;
      this.$emit('updateDateRange');
    },
    updateDateToPicker() {
      this.showDateToPicker = false;
      this.dateRangeSelection = null; // should deselect button but doesn't
      this.dateTo = this.dateToPickerValue;
      this.$emit('updateDateRange');
    },
  },
});
</script>