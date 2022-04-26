<template>
  <v-container id="medications" fluid class="fill-height">
    <v-row class="fill-height">
      <v-col cols="12" class="flex-grow-1 d-flex">
        <DemoMedicationsTable></DemoMedicationsTable>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue';
import DemoMedicationsTable from '@/views/miugo-demo/components/DemoMedicationsTable.vue';
import { mapMutations, mapState } from 'vuex';

export default Vue.extend({
  name: 'Medications',
  data() {
    return {
      appBarColor: {
        class: 'colorMedications',
        dark: true,
      },
    };
  },
  components: {
      DemoMedicationsTable,
  },
  computed:{
    ...mapState('demo', ['miugo'])
  },
  methods:{
    ...mapMutations('demo', {
      setAppStarted: 'setAppStarted',
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
