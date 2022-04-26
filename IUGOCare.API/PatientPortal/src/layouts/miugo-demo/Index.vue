<template>
  <v-app>
    <patient-portal-app-bar />
    <patient-portal-drawer />
    <patient-portal-view />
  </v-app>
</template>

<script>
export default {
  name: 'PatientPortalIndex',

  components: {
    PatientPortalAppBar: () => import('./AppBar'),
    PatientPortalDrawer: () => import('./Drawer'),
    PatientPortalView: () => import('./View'),
  },
};
</script>
