<template>
  <div>
    <v-card flat class="program-main" :disabled="!programStatus">
      <v-card-title justify="center" class="white--text darkGrey">
        <v-col>
          <v-row v-if="title" justify="center" :class="{'mb-7': !subtitle}">{{ title }}</v-row>
          <v-row justify="center" class="font-weight-bold">{{ subtitle }}</v-row>
        </v-col>
      </v-card-title>
      <v-expand-transition>
        <v-card-text class="pt-8 px-8" v-if="expanded">
        <slot/>
      </v-card-text>
      </v-expand-transition>
    </v-card>
    <v-divider/>
    <v-card flat class="program-toggle mb-4">
      <v-card-actions>
        <v-row justify="center" align="center">
          <v-switch
            color="primary"
            hide-details
            class="ma-0 pa-0"
            inset
            :label="label"
            v-model="programStatus"
            @click="clickHandler"
          ></v-switch>
        </v-row>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script lang="ts">
    import Vue from 'vue';

    export default Vue.extend({
        name: 'BaseProgramDisplay',
        data() {
            return {
                programStatus: false,
                key: '',
            };
        },
        mounted() {
            this.programStatus = this.status;
            this.key = this.subtitle.substring(1, this.subtitle.length - 1).toLowerCase();
        },
        methods: {
            clickHandler(): void {
                this.statusHandler(this.programStatus, this.key);
            },
        },
        props: {
            title: {
              type: String,
              default: '',
            },
            subtitle: {
              type: String,
              default: '',
            },
            status: {
              type: Boolean,
              default: false,
            },
            statusHandler: {
              type: Function,
            },
            expanded: {
              type: Boolean,
              default: false,
            },
            label: {
              type: String,
              default: '',
            }
        },
    });
</script>
<style lang='scss' scoped>
  .program-main {
    border-bottom-left-radius: 0px;
    border-bottom-right-radius: 0px;
    background-color: var(--v-background-base) !important;
  }

  .program-toggle {
    border-top-left-radius: 0px;
    border-top-right-radius: 0px;
    background-color: var(--v-background-base) !important;
  }

</style>
