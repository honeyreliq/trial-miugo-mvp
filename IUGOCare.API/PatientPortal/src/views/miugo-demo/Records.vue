<template>
  <v-container id="records" fluid class="fill-height">
     <v-row class="fill-height">
      <v-col cols="12" md="6" class="flex-grow-1 d-flex">
        <DemoReportsList></DemoReportsList>
      </v-col>
      <v-col cols="12" md="6" class="flex-grow-1 d-flex">
        <DemoFilePreview></DemoFilePreview>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
  import Vue from 'vue';
  import DemoReportsList from '@/views/miugo-demo/components/DemoFilesList.vue';
  import DemoFilePreview from '@/views/miugo-demo/components/DemoFilePreview.vue';
  import { mapMutations, mapState } from 'vuex';

  export default Vue.extend({
    name: 'Records',
    data() {
      return {
        appBarColor: {
          class: 'colorFiles',
          light: true,
        },
      };
    },
    components: {
        DemoReportsList,
        DemoFilePreview,
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
      this.appBarColor.light = false;
      this.$root.$emit('setAppBarColor', this.appBarColor);
    },
  });
</script>
