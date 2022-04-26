<template>
  <v-toolbar color="primary" flat :dark="defaultTheme" :light="!defaultTheme" v-bind="$attrs">
    <v-toolbar-title>{{title}}</v-toolbar-title>
    <slot/>
  </v-toolbar>
</template>

<script>
  export default {
    name: 'BaseToolbar',

    props: {
      title: {
        type: String,
        default: '',
      },
    },
    data() {
      return {
        defaultTheme: true,
      };
    },
    mounted() {
      this.defaultTheme = this.$attrs.light === undefined;
    },
  };
</script>
