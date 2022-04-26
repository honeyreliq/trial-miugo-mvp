<template>
  <v-navigation-drawer
    id="patient-portal-drawer"
    v-model="isVisible"
    :mini-variant="isMini"
    :temporary="isMobile"
    dark
    app
    v-bind="$attrs"
    :mini-variant-width="miniWidth"
    width="260"
    height="100%"
    :mobile-breakpoint="0"
  >
    <div class="py-0 drawer-wrapper">
      <router-link to="/" class="d-flex mt-4 pa-0 overflow-hidden">
        <span class="logo">
          <BaseLinkedImage
            :src="require(`@/assets/${iugoImg}`)"
            contain
            height="100%"
            position="left center"
          />
        </span>
      </router-link>
    </div>

    <v-list dense nav class="py-0 drawer-wrapper">
      <v-divider class="my-4" />
      <ObservationsDialog></ObservationsDialog>
      <v-divider class="my-4" />
      <template v-for="(item, i) in computedItems">
        <BaseItemGroup v-if="item.children" :key="`group-${i}`" :item="item" v-tooltip="enableTooltip(item)"/>
        <BaseItem v-else :key="`item-${i}`" :item="item" v-tooltip="enableTooltip(item)"/>
        <v-divider v-if="item.divider" :key="i" dark class="my-4"></v-divider>
      </template>
    </v-list>

    <template v-slot:append>
    <v-menu
      top
      offset-y
      origin="bottom left"
      transition="scroll-x-transition"
      min-width="200"
    >
      <template v-slot:activator="{ attrs, on }">
        <v-list dense nav class="py-0">
          <v-divider dark class="mb-4" />
          <v-list-item class="mb-4" v-bind="attrs" v-on="on">
            <v-list-item-icon>
              <fa-icon :icon="['fad', 'info-circle']" class="fa-fw" />
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title class="text-subtitle-1">{{
                $t('about')
              }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </v-list>
      </template>

      <v-list class="py-0" flat>
        <v-list-item-group>
          <AboutDialog></AboutDialog>
          <PrivacyDialog></PrivacyDialog>
          <v-divider />
          <VersionMenu></VersionMenu>
        </v-list-item-group>
      </v-list>
     </v-menu>
     </template>
  </v-navigation-drawer>
</template>

<script>
// Utilities
import ObservationsDialog from './components/ObservationsDialog.vue';

// Components
import AboutDialog from './components/AboutDialog.vue';
import PrivacyDialog from './components/PrivacyDialog.vue';
import VersionMenu from './components/VersionMenu.vue';

export default {
  name: 'PatientPortalDrawer',
  components: {
      ObservationsDialog,
      AboutDialog,
      PrivacyDialog,
      VersionMenu,
    },

  props: {},

  data: () => ({
    iugoImg: 'iUGO_Care_W.svg',
    miniVariant: false,
    items: [
      {
        group: '/observations',
        icon: 'mdi-chart-timeline-variant',
        title: 'observations',
        tooltip: 'observations',
        divider: true,
        children: [
          {
            icon: 'mdi-run',
            title: 'activity',
            to: 'activity',
            tooltip: 'activity',
          },
          {
            icon: 'mdi-bed',
            title: 'sleep',
            to: 'sleep',
            tooltip: 'sleep',
          },
          {
            icon: 'mdi-heart-pulse',
            title: 'vitalSigns',
            to: 'vital-signs',
            tooltip: 'vitalSigns',
          },
        ],
      },
      {
        icon: 'mdi-access-point-network',
        title: 'commandTests',
        to: '/commandTests',
      },
      {
        icon: 'mdi-key',
        title: 'activateAccount',
        to: '/activate-account',
        divider: true,
      },
    ],
  }),

  computed: {
    miniWidth() {
      return 42 + (16 * 2);
    },
    isMobile() {
      return this.$vuetify.breakpoint.mdAndDown;
    },
    isVisible: {
      get() {
        return this.isMobile ? this.$store.state.settings.drawer : true;
      },
      set(val) {
        this.$store.commit('settings/SET_DRAWER', val);
      },
    },
    isMini() {
      return this.isMobile ? false : !this.$store.state.settings.drawer;
    },
    computedItems() {
      return this.items.map(this.mapItem);
    },
  },

  methods: {
    mapItem(item) {
      return {
        ...item,
        children: item.children ? item.children.map(this.mapItem) : undefined,
        title: this.$t(item.title),
      };
    },
    logout() {
      this.$auth.logout({
        returnTo: window.location.origin,
      });
    },
    enableTooltip(item) {
      // v-tooltip has not a prop to disable, have to be manually
      if (this.isMini && item.tooltip) {
        return {
          content: this.$t(item.tooltip),
          placement: 'right',
          boundariesElement: 'body',
          classes: ['add-tooltop', this.$vuetify.theme.dark ? 'theme--dark' : 'theme--light'],
          offset: 17,
        };
      }
      return null;
    },
  },
};
</script>
