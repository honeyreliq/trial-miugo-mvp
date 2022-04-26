<template>
  <v-container id="surveys" fluid class="fill-height">
    <v-row class="fill-height">
      <v-col cols="6" class="flex-grow-1 d-flex">
        <DemoSubmittedSurveysList></DemoSubmittedSurveysList>
      </v-col>
      <v-col cols="6" class="flex-grow-1 d-flex">
        <DemoSurveyResponseCard></DemoSurveyResponseCard>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
  import Vue from 'vue';
  import DemoSubmittedSurveysList from '@/views/miugo-demo/components/DemoSubmittedSurveysList.vue';
  import DemoSurveyResponseCard from '@/views/miugo-demo/components/DemoSurveyResponseCard.vue';
  import { mapMutations, mapState } from 'vuex';

  export default Vue.extend({
    name: 'Surveys',
    data() {
      return {
        appBarColor: {
          class: 'colorSurveys',
          dark: true,
        },
      };
    },
    components: {
      DemoSubmittedSurveysList,
      DemoSurveyResponseCard,
    },
    computed:{
      ...mapState('demo', ['miugo'])
    },
    methods:{
      ...mapMutations('demo', {
        setAppStarted: 'setAppStarted'
      })
    },
    mounted() {
      setTimeout(() => {
        this.setAppStarted(true);
        this.$root.$emit('setAppBarColor', this.appBarColor);
      }, (this.miugo.appStarted) ? 0 : 200);
    },
    destroyed(): void {
      this.appBarColor.class = '';
      this.appBarColor.dark = false;
      this.$root.$emit('setAppBarColor', this.appBarColor);
    },
  });
</script>
