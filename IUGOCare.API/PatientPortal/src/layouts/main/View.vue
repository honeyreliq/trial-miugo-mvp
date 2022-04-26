<template>
  <v-main>
    <router-view />
    <patient-portal-footer />
  </v-main>
</template>

<script>
  export default {
    name: 'PatientPortalView',

    components: {
      PatientPortalFooter: () => import('./Footer'),
    },
  };
</script>
