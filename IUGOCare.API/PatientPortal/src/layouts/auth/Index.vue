<template>
  <v-app>
    <v-row class="flex-column flex-lg-row">
      <auth-sidebar />
      <auth-view />
    </v-row>
    <auth-footer />
  </v-app>
</template>

<script>
  export default {
    name: 'PagesIndex',

    components: {
      AuthFooter: () => import('./Footer'),
      AuthView: () => import('./View'),
      AuthSidebar: () => import('./Sidebar'),
    },
  };
</script>
