<template>
  <v-btn
    v-bind="$attrs"
    v-on="$listeners"
    :color="color"
    depressed
    :disabled="disabled"
  >
    <span>
      <slot />
    </span>
  </v-btn>
</template>

<script>
  export default {
    name: 'BaseButton',

    props: {
      color: {
        type: String,
        default: '',
      },
      disabled: {
        type: Boolean,
        default: false,
      },
    },
  };
</script>
