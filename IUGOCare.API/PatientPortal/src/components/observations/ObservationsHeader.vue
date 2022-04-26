<template>
<v-row>
  <v-col cols="12" sm="12" md="4" class="d-flex">
    <GreetingsCard></GreetingsCard>
  </v-col>
  <v-col cols="12" sm="12" md="8" class="d-none d-md-flex">
    <EngagementCard></EngagementCard>
  </v-col>
</v-row>
</template>

<script>
// Components
import EngagementCard from '@/components/cards/EngagementCard.vue';
import GreetingsCard from '@/components/cards/GreetingsCard.vue';

export default {
  name: 'ObservationsHeader',
  components: { EngagementCard, GreetingsCard },
};
</script>
