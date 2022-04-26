<template>
  <v-container fluid>
    <v-row>
      <v-col cols="12">
        <p class="text-h6 my-4">Demo Card and Form Structure</p>
      </v-col>
      <v-col cols="7">
        <DemoCardTemplate></DemoCardTemplate>
      </v-col>
      <v-col cols="5">
        <DemoFormTemplate></DemoFormTemplate>
      </v-col>
    </v-row>
  </v-container>
</template>
<script lang="ts">
import DemoFormTemplate from '@/views/iugo-care/components/DemoFormTemplate.vue';
import DemoCardTemplate from '@/views/iugo-care/components/DemoCardTemplate.vue';

export default({
  name: 'ChrisComponents',
  components: {DemoFormTemplate, DemoCardTemplate},
});
</script>
