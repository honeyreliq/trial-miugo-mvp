<template>
  <BaseCard color="primary" dark class="flex">
    <v-row class="primary flex-nowrap" align="center">
      <v-col cols="8" class="flex-grow-1 engagement-card-width">
        <v-carousel height="auto" :show-arrows="false" hide-delimiters delimiter-icon v-model="model" v-bind:reverse="shouldReverse">
          <v-carousel-item v-for="(slide, i) in slides" :key="i">
            <div class="text-h5">{{ $t(`${slide}`) }}</div>
          </v-carousel-item>
        </v-carousel>
      </v-col>
      <v-col cols="auto">
        <BaseButton class="mr-4" color="primary darken-1" @click="decrementModel">
          <fa-icon :icon="['fad', 'chevron-left']" class="fa-fw" />
        </BaseButton>
        <BaseButton color="primary darken-1" @click="incrementModel">
          <fa-icon :icon="['fad', 'chevron-right']" class="fa-fw" />
        </BaseButton>
      </v-col>
    </v-row>
  </BaseCard>
</template>

<script lang="ts">
  import Vue from 'vue';

  export default Vue.extend({
  name: 'EngagementCard',

    data() {
      return {
        slides: [
          'engagementCardText1',
          'engagementCardText2',
          'engagementCardText3',
          'engagementCardText4',
          'engagementCardText5',
          'engagementCardText6',
        ],
        model: 0,
        shouldReverse: false,
      };
    },
   methods: {
      // Corrects transition animation when navigating backward from first to last slide
      decrementModel() {
        if (this.model === 0) {
            this.shouldReverse = true;
            this.model = this.slides.length - 1;
        } else {
            this.shouldReverse = false;
            this.model--;
          }
      },
      incrementModel() {
        this.shouldReverse = false;
        this.model++;
      },
    },
});
</script>
<style lang="scss">
  .engagement-card-width {
    min-width: 100px;
    max-width: 100%;
  }
</style>
