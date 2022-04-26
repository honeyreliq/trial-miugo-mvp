<template>
  <BaseCard class="flex">
    <v-data-table
      :headers="table.headers"
      :items="table.rows"
      item-key="key"
      :single-expand="singleExpand"
      :expanded.sync="expanded"
      :item-class="setClass"
      class="striped"
      show-expand
      @click:row="checkExpandedInfo"
      :search="search"
      :sort-by="table.headers[4].value"
      :sort-desc="true">
      <template v-slot:top>
      <v-row>
      <!-- search -->
        <v-col cols="auto" class="flex-grow-1">
          <v-text-field
              hide-details
              v-model="search"
              label="Search"
              class="mb-4"
          >
          <fa-icon :icon="['far', 'search']" class="fa-fw fa-button" slot="append" />
          </v-text-field>
        </v-col>
        <!-- print -->
        <v-col cols="auto" class="d-flex justify-end">
          <BaseButton color="primary">
            <fa-icon :icon="['fal', 'print']" class="fa-fw white--text"/>
          </BaseButton>
        </v-col>
      </v-row>
      </template>
      <template v-slot:item.startDate="{item}">
        {{ formatStringFromTimestamp(item.startDate, 'L') }}
      </template>
      <template v-slot:expanded-item="{item}">
        <td :colspan="table.headers.length" :class="item.class">
          {{ item.expandedInfo }}
        </td>
      </template>
    </v-data-table>
  </BaseCard>
</template>

<script lang="ts">
  import Vue from 'vue';
  import { subtractFromToday, timestampToString } from '@/views/miugo-demo/components/utils';

  export default Vue.extend({
    name: 'DemoMedicationsTable',
    components: {},
    methods: {
      setClass(item: any): string {
        return item.class;
      },
      checkExpandedInfo(item: any, slot: any): any {
        if (item.expandedInfo)
        return slot.expand(!slot.isExpanded);
      },
      formatStringFromTimestamp(timestamp: number, format: string): string {
        return timestampToString(timestamp, format);
      }
    },
    data: () => ({
      search: '',
      expanded: [],
      singleExpand: true,
      table: {
        headers: [
          {text: 'Medication', value: 'medication', sortable: true, class: 'text-subtitle-1'},
          {text: 'Dose', value: 'dose', sortable: false, class: 'text-subtitle-1'},
          {text: 'Route', value: 'route', sortable: false, class: 'text-subtitle-1'},
          {text: 'Frequency', value: 'frequency', sortable: false, class: 'text-subtitle-1'},
          {text: 'Start Date', value: 'startDate', sortable: true, class: 'text-subtitle-1'},
          {text: 'End Date', value: 'endDate', sortable: true, class: 'text-subtitle-1'},
          {text: 'Status', value: 'status', sortable: true, class: 'text-subtitle-1'},
          {value: 'data-table-expand'},
        ],
        rows: [
          {
            key: 'Atorvastatin',
            medication: 'Atorvastatin',
            dose: '40mg',
            route: 'PO',
            frequency: 'qHS',
            startDate: subtractFromToday(0, 3, 34),
            endDate: '-',
            status: 'Active',
            expandedInfo: 'Take once a day at bedtime.',
          },
          {
            key: 'Donepezil',
            medication: 'Donepezil',
            dose: '10mg',
            route: 'PO',
            frequency: 'Daily',
            startDate: subtractFromToday(0, 15, 11),
            endDate: '-',
            status: 'Active',
            expandedInfo: 'Take once a day with or without food, in the evening just before bedtime. ' +
              'Take donepezil at around the same time every day.',
          },
          {
            key: 'Lorazepam1',
            medication: 'Lorazepam',
            dose: '1mg',
            route: 'PO',
            frequency: 'qHS',
            startDate: subtractFromToday(0, 3, 14),
            endDate: '-',
            status: 'Active',
            expandedInfo: 'Take at bedtime.',
          },
          {
            key: 'Lorazepam2',
            medication: 'Lorazepam',
            dose: '1mg',
            route: 'PO',
            frequency: 'TID PRN',
            startDate: subtractFromToday(0, 3, 14),
            endDate: '-',
            status: 'Active',
            expandedInfo: 'Take only when needed and up to 3 doses.',
          },
          {
            key: 'Ramipril',
            medication: 'Ramipril',
            dose: '2.5mg',
            route: 'PO',
            frequency: 'Daily',
            startDate: subtractFromToday(0, 8, 22),
            endDate: '-',
            status: 'Active',
            expandedInfo: 'Take once a day with or without food at the same time every day.',
          },
          {
            key: 'Metformin',
            medication: 'Metformin',
            dose: '500mg',
            route: 'PO',
            frequency: 'BID',
            startDate: subtractFromToday(0, 2, 4),
            endDate: '-',
            status: 'Active',
            expandedInfo: 'Take one before breakfast and one before supper.',
          },
        ],
      },
    }),
  });
</script>
<style lang="scss">
// WIP, TO BE CLEANED UP
  .v-data-table__expanded td {
    border-bottom: none !important;
  }
  .v-data-table__expanded.v-data-table__expanded__row {
    background-color: var(--v-primary-base) !important;
    color:#ffffff;
  }
  .v-data-table__expanded.v-data-table__expanded__row .v-data-table__expand-icon {
    color:#ffffff;
  }
  .v-data-table__expanded.v-data-table__expanded__content {
    box-shadow: none !important;
  }
  .v-data-table__expanded__content  td {
    border-bottom: 1px solid rgba(0, 0, 0, 0.1) !important
}
</style>
