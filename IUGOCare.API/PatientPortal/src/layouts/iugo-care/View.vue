<template>
  <v-main>
    <patient-bar />
    <router-view />
    <iugo-care-footer />
  </v-main>
</template>

<script>
  export default {
    name: 'IugoCareView',

    components: {
      IugoCareFooter: () => import('./Footer'),
      PatientBar: () => import('./PatientBar'),
    },
  };
</script>
