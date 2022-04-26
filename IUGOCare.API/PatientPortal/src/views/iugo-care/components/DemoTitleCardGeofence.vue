<template>
    <BaseCard class="error" dark>
      <v-row>
        <v-col cols="auto" class="flex-grow-0 flex-shrink-0 align-self-center hidden-md-and-down">
          <fa-icon
            class="mr-2"
            icon="exclamation-triangle"
            style="font-size: 3em"
          ></fa-icon>
        </v-col>
        <v-col cols="auto" class="flex-grow-1 flex-shrink-0 align-self-center">
          <v-card-title class="text-subtitle-2 ma-0 pa-0"
            >Incident Occurrence:</v-card-title
          >
          <v-card-text class="text-h4 ma-0 pa-0">
            Geofence Violation
          </v-card-text>
        </v-col>
      </v-row>
    </BaseCard>
</template>

<script lang="ts">
export default {
  name: 'DemoTitleCardGeofence',
  data(): any {
    return {
    };
  },
};
</script>
