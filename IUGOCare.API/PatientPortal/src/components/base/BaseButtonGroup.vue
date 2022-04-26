<template>
  <v-btn-toggle
    v-bind="$attrs"
    v-on="$listeners"
    :color="color"
    active-class="primary white--text"
    borderless
    v-model="currentValue"
    @change="$emit('input', currentValue)"
  >
    <slot/>
  </v-btn-toggle>
</template>

<script lang="ts">
  import Vue from 'vue';

  export default Vue.extend({
    name: 'BaseButtonGroup',
    props: {
      value: String,
      color: {
        type: String,
        default: '',
      },
    },
    data(): any {
      return {
        // using props that can be changed by the parent, and also works as model in component is an anti-pattern in vue
        // the way to avoid those error is like this
        currentValue: this.value,
      };
    },
  });
</script>
