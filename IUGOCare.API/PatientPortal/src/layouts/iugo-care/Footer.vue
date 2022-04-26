<template>
  <v-footer id="iugo-care-footer" inset flat>
    <v-row align="center" no-gutters>
      <v-col class="col-12 text-center py-2">
        <span class="text-subtitle-1"
          >&copy; {{ new Date().getFullYear() }} {{ $t("footerText") }}</span
        >
      </v-col>
    </v-row>
    <v-fade-transition>
    <v-btn
      v-scroll="onScroll"
      v-show="buttonTop"
      buttonTop
      dark
      fab
      fixed
      bottom
      right
      color="primary"
      @click="toTop"
    >
      <fa-icon :icon="['far', 'chevron-up']" class="fa-fw" />
    </v-btn>
    </v-fade-transition>
  </v-footer>
</template>

<script>
export default {
  name: 'IugoCareFooter',
  data: () => ({
    buttonTop: false,
  }),

  methods: {
    onScroll(e) {
      if (typeof window === 'undefined') {
        return;
      }
      const top = window.pageYOffset || e.target.scrollTop || 0;
      this.buttonTop = top > 20;
    },
    toTop() {
      this.$vuetify.goTo(0);
    },
  },
};
</script>
