<template>
  <v-sheet class="lighterGrey darken-1 fill-height" flat light>
    <v-row class="px-4 fill-height">
      <v-col cols="auto" class="align-self-center">
        <span class="text-h5 text-no-wrap">{{timer.min}} : {{timer.sec}}</span>
      </v-col>
      <v-col cols="auto" class="align-self-center">
        <v-btn
          depressed
          small
          icon
          light
          class="lighterGrey darken-2"
          @click="toggleTimer"
        >
          <fa-icon class="fa-lg" :icon="['fas', currentIcon]" />
        </v-btn>
      </v-col>
      <v-col cols="auto" class="align-self-center">
        <v-btn
          depressed
          small
          icon
          light
          class="lighterGrey darken-2"
          @click="resetTimer"
        >
          <fa-icon class="fa-lg" :icon="['fas', 'check']"></fa-icon>
        </v-btn>
      </v-col>
      <v-col cols="auto" class="align-self-center">
        <v-btn
          depressed
          small
          icon
          light
          class="lighterGrey darken-2"
          @click="resetTimer"
        >
          <fa-icon class="fa-lg" :icon="['fas', 'sync-alt']"></fa-icon>
        </v-btn>
      </v-col>
    </v-row>
  </v-sheet>
</template>
<script lang="ts">
    import Vue from 'vue';
    export default Vue.extend({
        name: 'DemoTimer',
        components: {},
        data: () => ({
            timer: {
              sec: '00',
              min: '00',
            },
            currentIcon: 'play',
            isRunning: false,
            interval: null,
        }),
        methods: {
            toggleTimer() {
                this.currentIcon = this.currentIcon === 'play' ? 'stop' : 'play';
                if (this.isRunning) {
                    clearInterval(this.interval);
                } else {
                    this.interval = setInterval(this.incrementTime, 1000);
                }
                this.isRunning = !this.isRunning;
            },

            incrementTime() {
                const newSecValue = parseInt(this.timer.sec, 10) + 1;
                const newMinValue = parseInt(this.timer.min, 10) + 1;
                this.timer.sec = (newSecValue > 59) ? '00' : newSecValue.toString().padStart(2, '0');
                this.timer.min = (newSecValue > 59) ? newMinValue.toString().padStart(2, '0') :
                    this.timer.min.toString().padStart(2, '0');
            },

            resetTimer() {
                clearInterval(this.interval);
                this.timer.sec = '00';
                this.timer.min = '00';
                this.currentIcon = 'play';
                this.isRunning = false;
                this.interval = null;
            },
        },
    });
</script>
