<template>
  <v-container id="dashboard" fluid>
    <v-row class="fill-height flex-column" justify="center">
      <v-col class="flex-grow-0">
        <observations-header></observations-header>
      </v-col>
      <v-col cols="12">
        <BaseCard>
          <v-card-text class="text-center">
            <v-row justify="center">
              <v-col>iUGO Care Dashboard</v-col>
            </v-row>
          </v-card-text>
        </BaseCard>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import ObservationsHeader from '../../components/observations/ObservationsHeader.vue';
export default({
  name: 'iUGOCareDashboard',
  components: {ObservationsHeader},
  title: 'iUGO Care - Dashboard',
});
</script>
