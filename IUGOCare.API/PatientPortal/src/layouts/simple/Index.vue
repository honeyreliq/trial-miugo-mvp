<template>
  <v-app>
    <pages-view />
    <pages-footer />
  </v-app>
</template>

<script>
  export default {
    name: 'SimplePagesIndex',

    components: {
      PagesFooter: () => import('./Footer'),
      PagesView: () => import('./View'),
    },
  };
</script>
