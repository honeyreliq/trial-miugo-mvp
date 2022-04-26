<template>
  <v-container id="education" fluid class="fill-height">
     <v-row class="fill-height">
      <v-col cols="6" class="flex-grow-1 d-flex">
        <DemoEducationList></DemoEducationList>
      </v-col>
      <v-col cols="6" class="flex-grow-1 d-flex">
        <DemoFilePreview></DemoFilePreview>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
  import Vue from 'vue';
  import DemoEducationList from '@/views/miugo-demo/components/DemoEducationList.vue';
  import DemoFilePreview from '@/views/miugo-demo/components/DemoFilePreview.vue';
  import { mapMutations, mapState } from 'vuex';

  export default Vue.extend({
    name: 'Education',
    data() {
      return {
        appBarColor: {
          class: 'colorEducation',
          dark: true,
        },
      };
    },
    components: {
        DemoEducationList,
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
      this.appBarColor.dark = false;
      this.$root.$emit('setAppBarColor', this.appBarColor);
    },
  });
</script>
