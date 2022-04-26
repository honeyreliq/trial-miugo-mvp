<template>
  <v-app>
    <iugo-care-app-bar />
    <iugo-care-drawer />
    <iugo-care-view />
  </v-app>
</template>

<script>
export default {
  name: 'IugoCareIndex',

  components: {
    IugoCareAppBar: () => import('./AppBar'),
    IugoCareDrawer: () => import('./Drawer'),
    IugoCareView: () => import('./View'),
  },
};
</script>
