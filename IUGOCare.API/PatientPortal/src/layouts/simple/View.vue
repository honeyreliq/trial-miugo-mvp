<template>
<v-main class="primary fill-height">
  <v-container
    class="fill-height white--text"
    tag="section"
    fluid
  >
    <router-view />
  </v-container>
</v-main>
</template>

<script>
  export default {
    name: 'SimplePagesView',
  };
</script>
