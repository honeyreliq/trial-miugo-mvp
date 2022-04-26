<template>
  <BaseCard class="flex">
    <v-data-table
      required
      :headers="table.headers"
      :items="table.rows"
      class="striped"
      :search="search"
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
        <tr
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
  name: 'DemoEducationList',
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
    }
  },
  data(): any {
    return {
      search: '',
      selectedRow: null,
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
            sortable: false,
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
            fid: 21,
            name: 'Blood Glucose Info Sheet.pdf',
            category: 'Education',
            date: subtractFromToday(0, 30),
            time: '3:02 PM',
          },
          {
            fid: 22,
            name: 'Blood Pressure Info Sheet.pdf',
            category: 'Education',
            date: subtractFromToday(0, 30),
            time: '2:54 PM',
          },
          {
            fid: 23,
            name: 'What is high blood pressure.pdf',
            category: 'Education',
            date: subtractFromToday(0, 35),
            time: '9:53 AM',
          },
          {
            fid: 24,
            name: 'What is diabetes.pdf',
            category: 'Education',
            date: subtractFromToday(0, 37),
            time: '10:23 AM',
          },
        ],
      },
    };
  },
  mounted() {
    this.selectFile(this.table.rows[0]);
  },
});
</script>
