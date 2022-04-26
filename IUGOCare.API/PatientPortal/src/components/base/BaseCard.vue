<template>
  <v-card
      v-bind="$attrs"
      v-bind:class="margin"
      :color="color"
      flat
  >
    <section :class="headClass">
      <v-card-title v-if="title" v-bind:class="[{'text-h6 pb-4': divider || subtitle,  'text-h6 pb-0': (!divider && !subtitle) || !subtitle}]">{{ title }}</v-card-title>
      <v-card-subtitle v-if="subtitle" v-bind:class="[{'text-subtitle-2': divider,  'text-subtitle-2 pb-0': !divider}]">{{ subtitle }}</v-card-subtitle>
      <v-divider v-if="(title || subtitle) && divider"></v-divider>
    </section>
    <section :class="bodyClass">
      <v-card-text v-if="$slots['default']" class="pa-4">
        <slot/>
      </v-card-text>
    </section>
  </v-card>
</template>

<script>
export default {
  name: 'BaseCard',

  props: {
    color: {
      type: String,
      default: '',
    },
    title: {
      type: String,
      default: '',
    },
    subtitle: {
      type: String,
      default: '',
    },
    margin: {
      type: String,
      default: 'mb-4',
    },
    divider: {
      type: Boolean,
      default: false,
    },
    headClass: {
      type: String,
      default: '',
    },
    bodyClass: {
      type: String,
      default: '',
    },
  },
  computed: {},
};
</script>
