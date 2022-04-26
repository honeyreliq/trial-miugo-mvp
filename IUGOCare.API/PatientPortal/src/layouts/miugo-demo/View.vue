<template>
  <v-main>
    <router-view />
    <patient-portal-footer />
    <AddButton class="mb-4 mr-4"></AddButton>
  </v-main>
</template>

<script>
export default {
  name: 'PatientPortalView',

  components: {
    PatientPortalFooter: () => import('./Footer'),
    AddButton: () => import('./components/AddButton.vue'),
  },
};
</script>
