<template>
  <BaseCard color="primary" dark class="flex">
    <div class="text-h5" v-if="firstName">{{$t('greeting')}},
      <!-- {{firstName}} -->
      Alicia
      !</div>
    <div class="text-h5" v-else>{{$t('greeting')}}!</div>
  </BaseCard>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapState } from 'vuex';

export default Vue.extend({
  name: 'GreetingsCard',

  data: () => ({
    patientName: '',
  }),
  computed: {
    ...mapState('patients', ['currentPatient']),
    firstName(): string {
      return this.currentPatient ? this.currentPatient.givenName : '';
    },
  },

  async created() {
    this.$store.dispatch('patients/fetchCurrentPatient');
  },
});
</script>
