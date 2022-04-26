<template>
  <BaseCard class="flex">
    <v-data-table
      :headers="table.headers"
      :items="table.rows"
      class="striped"
      :search="search"
      :sort-by="table.headers[2].value"
      :sort-desc="true"
    >
      <template v-slot:top>
        <v-row>
          <v-col cols="12">
            <v-text-field
              hide-details
              v-model="search"
              label="Search"
              class="mb-4"
            >
              <fa-icon
                :icon="['far', 'search']"
                class="fa-fw fa-button"
                slot="append"
              />
            </v-text-field>
          </v-col>
        </v-row>
      </template>
      <template v-slot:item="{ item }">
        <!-- Fake file submit for Demo -->
         <tr v-if="item.fid!=1 || showExample"
          :class="{
            'primary white--text row-active': selectedRow === item.fid,
          }"
          @click="item.fid ? selectFile(item) : $event.preventDefault()"
        >
          <td>{{ item.name }}</td>
          <td>{{ item.category }}</td>
          <td>{{ formatStringFromTimestamp(item.date, 'L', item.time) }}</td>
        </tr>
      </template>
    </v-data-table>
  </BaseCard>
</template>
<script lang="ts">
import Vue from 'vue';
import { mapMutations } from 'vuex';
import { IFile } from '@/store/demo';
import { timestampToString, subtractFromToday } from '@/views/miugo-demo/components/utils';

export default Vue.extend({
  name: 'DemoFilesList',
  components: {},
  methods: {
    ...mapMutations('demo', {
      setSelectedFile: 'setSelectedFile',
    }),
    selectFile(file: IFile): void {
      this.selectedRow = file.fid;
      this.setSelectedFile(file);
    },
    formatStringFromTimestamp(timestamp: number, format: string, time: string): string {
        return `${timestampToString(timestamp, format)} ` + ((time != null) ? `@ ${time}` : '');
    },
     // Fake file submit for Demo
    toggleExampleRow(state: boolean) {
      this.showExample = state;
      this.table.rows[0].date = subtractFromToday() // Fake file submit for Demo
      this.table.rows[0].time = `${timestampToString(subtractFromToday(), 'hh:mm A')}` // Fake file submit for Demo
      this.selectFile(this.table.rows[0]); // Fake file submit for Demo
    },
  },
  data(): any {
    return {
      search: '',
      selectedRow: null,
      // Fake file submit for Demo
      showExample: false,
      currentTime: null,
      table: {
        headers: [
          {
            text: 'File Name',
            value: 'name',
            sortable: true,
            class: 'text-subtitle-1',
          },
          {
            text: 'Category',
            value: 'category',
            sortable: true,
            class: 'text-subtitle-1',
          },
          {
            text: 'Date',
            value: 'date',
            sortable: true,
            class: 'text-subtitle-1',
          },
        ],
        rows: [
          {
            fid: 1,
            // Fake file submit for Demo
            name: 'Wound Care@3x.png',
            category: 'Wound Image',
            date: subtractFromToday(0, 0),
            time: null,
          },

          {
            fid: 2,
            name: 'Blood Pressure 3 months',
            category: 'Report',
            date: subtractFromToday(0, 2),
            time: '2:43 PM',
          },
          {
            fid: 3,
            name: 'Blood Glucose 3 months',
            category: 'Report',
            date: subtractFromToday(0, 7),
            time: '1:39 PM',
          },
          {
            fid: 4,
            name: 'Left foot 003.jpg',
            category: 'Wound Image',
            date: subtractFromToday(0, 9),
            time: '1:23 PM',
          },
          {
            fid: 5,
            name: 'Left foot 002.jpg',
            category: 'Wound Image',
            date: subtractFromToday(0, 22),
            time: '1:32 PM',
          },
          {
            fid: 6,
            name: 'Left foot 001.jpg',
            category: 'Wound Image',
            date: subtractFromToday(0, 43),
            time: '1:34 PM',
          },
          {
            fid: 7,
            name: 'Chronic Care Management Agreement.pdf',
            category: 'Records',
            date: subtractFromToday(0, 29),
            time: '3:02 PM',
          },
          {
            fid: 8,
            name: 'CCM Consent.pdf',
            category: 'Records',
            date: subtractFromToday(0, 30),
            time: '2:54 PM',
          },
          {
            fid: 9,
            name: 'RPM Consent.pdf',
            category: 'Records',
            date: subtractFromToday(0, 35),
            time: '10:23 AM',
          },
        ],
      },
    };
  },
  mounted() {
    this.selectFile(this.table.rows[1]);
    this.$root.$on('file-submitted', this.toggleExampleRow);  // Fake file submit for Demo
  },
  destroyed() {
    this.$root.$off('file-submitted', this.toggleExampleRow);  // Fake file submit for Demo
  },
});
</script>
<style lang="scss">
.v-data-table-header th {
  white-space: nowrap;
}
// Fake file submit for Demo
.v-btn:before {
  opacity: 0 !important;
}

.v-ripple__container {
  opacity: 0 !important;
}
</style>

