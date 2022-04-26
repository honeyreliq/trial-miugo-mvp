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
        <tr
          :class="{
            'primary white--text row-active': selectedRow === item.sid,
          }"
          @click="item.sid ? selectSurvey(item) : $event.preventDefault()"
        >
          <td>{{ item.survey }}</td>
          <td>{{ item.status }}</td>
          <td>{{ formatStringFromTimestamp(item.date, 'L', item.time) }}</td>
        </tr>
      </template>
    </v-data-table>
  </BaseCard>
</template>
<script lang="ts">
import Vue from 'vue';
import { mapMutations } from 'vuex';
import { ISurvey } from '@/store/demo';
import { timestampToString, subtractFromToday } from '@/views/miugo-demo/components/utils';

export default Vue.extend({
  name: 'DemoSubmittedSurveysList',
  components: {},
  methods: {
    ...mapMutations('demo', {
      setSelectedSurvey: 'setSelectedSurvey',
    }),
    selectSurvey(survey: ISurvey): void {
      this.selectedRow = survey.sid;
      this.setSelectedSurvey(survey);
    },
    formatStringFromTimestamp(timestamp: number, format: string, time: string): string {
        return `${timestampToString(timestamp, format)} ` + ((time != null) ? `@ ${time}` : '');
    }
  },
  data(): any {
    return {
      search: '',
      selectedRow: null,
      showSuccessMessage: false,
      table: {
        headers: [
          {
            text: 'Survey',
            value: 'survey',
            sortable: true,
            class: 'text-subtitle-1',
          },
          {
            text: 'Status',
            value: 'status',
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
            sid: 1,
            survey: 'Generalized Anxiety Disorder-7',
            status: 'Assigned',
            date: subtractFromToday(0, 1),
            time: null,
          },
          {
            sid: 2,
            survey: 'Generalized Anxiety Disorder-7',
            status: 'Completed',
            date: subtractFromToday(0, 15),
            time: '1:03 PM',
          },
          {
            survey: 'Generalized Anxiety Disorder-7',
            status: 'Reviewed',
            date: subtractFromToday(0, 11, 2),
            time: '2:20 PM',
          },
          {
            survey: 'Generalized Anxiety Disorder-7',
            status: 'Reviewed',
            date: subtractFromToday(0, 3, 7),
            time: '10:21 AM',
          },
          {
            survey: 'CAGE Questionnaire',
            status: 'Reviewed',
            date: subtractFromToday(0, 0, 9),
            time: '10:14 AM',
          },
          {
            survey: 'Generalized Anxiety Disorder-7',
            status: 'Reviewed',
            date: subtractFromToday(0, 9, 12),
            time: '10:12 AM',
          },
          {
            survey: 'Patient Health Questionnaire-9',
            status: 'Reviewed',
            date: subtractFromToday(0, 3, 13),
            time: '9:30 PM',
          },
          {
            survey: 'CAGE Questionnaire',
            status: 'Reviewed',
            date: subtractFromToday(0, 10, 13),
            time: '12:04 PM',
          },
        ],
      },
    };
  },
  mounted() {
    this.selectSurvey(this.table.rows[0]);
    this.$root.$on('submitForm', () => {
      this.table.rows[0].status = 'Completed';
      this.table.rows[0].date = subtractFromToday()
      this.table.rows[0].time = `${timestampToString(subtractFromToday(), 'hh:mm A')}`
      this.selectSurvey(this.table.rows[0]);
    });
  },
});
</script>
