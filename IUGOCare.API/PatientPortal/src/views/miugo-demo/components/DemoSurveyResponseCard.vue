<template>
  <v-fade-transition :hide-on-leave="true">
    <v-card flat v-if="currentSurvey.sid" class="flex mb-4 pa-4">
      <v-row>
        <v-col class="text-h6 mb-4 flex-grow-1" cols="auto">{{
          currentSurvey.data.survey
        }}</v-col>
        <v-col class="subtitle-1 mt-1" cols="auto">{{
          formatStringFromTimestamp(currentSurvey.data.date, 'L', currentSurvey.data.time)
        }}</v-col>
      </v-row>
      <DemoSurveyModal v-model="surveyModal"></DemoSurveyModal>
      <section
        v-if="currentSurvey.data.status === 'Assigned'"
        class="fill-height d-flex align-center justify-center pb-10"
      >
        <v-row>
          <v-col cols="12" class="mb-4 text-center">
            <fa-icon :icon="['fad', 'poll-h']" class="fa-fw fa-empty-icon" />
          </v-col>
          <v-col cols="12" class="mb-4 text-center">
            You have not completed this assigned survey yet.
          </v-col>
          <v-col cols="12" class="text-center">
            <BaseButton color="primary" dark @click.stop="surveyModal = true">
              Start The Survey
            </BaseButton>
          </v-col>
        </v-row>
      </section>
      <section v-if="currentSurvey.data.status !== 'Assigned'">
        <v-row>
          <v-col cols="12 mt-4"
            >Over the last two weeks, how often have you been bothered by the
            following problems?
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12 mt-4">
            <ol type="1">
              <li v-for="(item, i) in currentSurvey.questions" :key="`survey-item-${i}`">
                <v-row>
                  <v-col cols="12 mb-1">{{ item.question }}</v-col>
                </v-row>
                <v-row>
                  <v-col cols="12 mb-4">{{ item.answer }}</v-col>
                </v-row>
              </li>
            </ol>
          </v-col>
        </v-row>
      </section>
    </v-card>
  </v-fade-transition>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters } from 'vuex';
import { ISurvey } from '@/store/demo';
import { timestampToString } from '@/views/miugo-demo/components/utils';
import DemoSurveyModal from '@/views/miugo-demo/components/DemoSurveyModal.vue';

interface IQuestion {
  question: string;
  answer: string;
}

interface ISurveyData {
  sid: number;
  data: ISurvey;
  questions: IQuestion[];
}

const fakeSurveyQuestions1: IQuestion[] = [
  {
    question: 'Feeling nervous, anxious, or on edge.',
    answer: 'Not at all.',
  },
  {
    question: 'Not being able to stop or control worrying.',
    answer: 'Several days.',
  },
  {
    question: 'Worrying too much about different things.',
    answer: 'More than half the days.',
  },
  {
    question: 'Trouble relaxing.',
    answer: 'Not at all.',
  },
  {
    question: 'Being so restless that it is hard to sit still.',
    answer: 'Several days.',
  },
  {
    question: 'Becoming easily annoyed or irritable.',
    answer: 'More than half the days.',
  },
  {
    question: 'Feeling afraid, as if something awful might happen.',
    answer: 'Not at all.',
  },
];

const fakeSurveyQuestions2: IQuestion[] = [
  {
    question: 'Feeling nervous, anxious, or on edge.',
    answer: 'Several days.',
  },
  {
    question: 'Not being able to stop or control worrying.',
    answer: 'Several days.',
  },
  {
    question: 'Worrying too much about different things.',
    answer: 'More than half the days.',
  },
  {
    question: 'Trouble relaxing.',
    answer: 'Several days.',
  },
  {
    question: 'Being so restless that it is hard to sit still.',
    answer: 'Several days.',
  },
  {
    question: 'Becoming easily annoyed or irritable.',
    answer: 'Several days.',
  },
  {
    question: 'Feeling afraid, as if something awful might happen.',
    answer: 'Not at all.',
  },
];

const dataSurvey: ISurveyData[] = [
  {sid: 1, data: null, questions: [...fakeSurveyQuestions1]},
  {sid: 2, data: null, questions: [...fakeSurveyQuestions2]},
];

export default Vue.extend({
  name: 'DemoSurveyResponseCard',
  components: {
    DemoSurveyModal,
  },
  computed: {
    ...mapGetters('demo', ['selectedSurvey']),
  },
  watch: {
    selectedSurvey(data: ISurvey): void {
      if (data && (data.sid === 1 || data.sid === 2)) {
        this.currentSurvey = this.checkSelectedSurvey(data);
      }
    },
  },
  props: {},
  data(): any {
    return {
      surveyModal: false,
      currentSurvey: {
        date: null,
        sid: null,
        status: null,
        survey: null,
      },
    };
  },
  mounted() {
    if (this.$route.query.surveymodal === '1') {
      this.surveyModal = true;
    }
  },
  methods: {
    checkSelectedSurvey(data: ISurvey): ISurveyData {
      const survey: ISurveyData = {...dataSurvey.find((item) => item.sid === data.sid)};
      survey.data = data;

      return survey;
    },
    formatStringFromTimestamp(timestamp: number, format: string, time: string): string {
        return `${timestampToString(timestamp, format)} ` + ((time != null) ? `@ ${time}` : '');
    }
  },
});
</script>
