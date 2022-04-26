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
    width="300"
    height="100%"
    :mobile-breakpoint="0"
  >
    <div class="py-0 drawer-wrapper">
      <router-link to="/demos/" class="d-flex mt-4 pa-0 overflow-hidden">
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
      <template v-for="(item, i) in computedItems">
        <BaseItemGroup v-if="item.children" :key="`group-${i}`" :item="item" :use-tooltip="isMini" :class="item.class"/>
        <BaseItem v-else :key="`item-${i}`" :item="item" v-tooltip="enableTooltip(item)" :class="item.class"/>
        <v-divider v-if="item.divider" :key="i" dark class="my-4" :class="item.class"></v-divider>
      </template>
    </v-list>

    <template v-slot:append>
      <v-menu
        top
        offset-y
        origin="bottom left"
        transition="scroll-x-transition"
        min-width="276"
        :value="openMenu"
      >
        <template v-slot:activator="{ attrs, on }">
          <v-list dense nav class="py-0 drawer-wrapper">
            <v-divider dark class="mb-4" />
            <v-list-item class="mb-4" @click="openMenu = true" v-bind="attrs" v-on="on">
              <v-list-item-icon>
                <fa-icon :icon="['fad', 'info-circle']" class="fa-fw" />
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title class="text-subtitle-1">
                  {{$t("information")}}
                </v-list-item-title>
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
// Components
import AboutDialog from './components/AboutDialog.vue';
import PrivacyDialog from './components/PrivacyDialog.vue';
import VersionMenu from './components/VersionMenu.vue';

export default {
  name: 'PatientPortalDrawer',
  components: {
      AboutDialog,
      PrivacyDialog,
      VersionMenu,
    },

  props: {},

  data: () => ({
    iugoImg: 'MiUGO_Care_W.svg',
    miniVariant: false,
    openMenu: false,
    items: [
      {
        group: '/miugo/observations',
        icon: 'analytics',
        title: 'observations',
        tooltip: 'observations',
        divider: true,
        children: [
          {
            icon: 'running',
            title: 'activity',
            to: 'activity',
            tooltip: 'activity',
          },
          {
            icon: 'bed',
            title: 'sleep',
            to: 'sleep',
            tooltip: 'sleep',
          },
          {
            icon: 'heartbeat',
            title: 'vitalSigns',
            to: 'vital-signs',
            tooltip: 'vitalSigns',
          },
        ],
      },
      {
        group: '/miugo/careplans',
        icon: 'laptop-medical',
        title: 'myCarePlans',
        tooltip: 'myCarePlans',
        divider: true,
        children: [
          {
            title: 'rpm',
            tooltip: 'rpm',
          },
          {
            title: 'ccm',
            to: 'ccm',
            tooltip: 'ccm',
          },
          {
            title: 'pcm',
            tooltip: 'pcm',
          },
          {
            title: 'bhi',
            tooltip: 'bhi',
          },
        ],
      },
      {
        title: 'medications',
        icon: 'capsules',
        to: '/miugo/medications',
        tooltip: 'medications',
      },
      {
        title: 'surveys',
        icon: 'poll-h',
        to: '/miugo/surveys',
        tooltip: 'surveys',
      },
      {
        title: 'records',
        icon: 'files-medical',
        to: '/miugo/records',
        tooltip: 'records',
      },
      {
        title: 'education',
        icon: 'books',
        to: '/miugo/education',
        tooltip: 'education',
        divider: true,
      },
      {
        title: 'messages',
        icon: 'comments',
        to: '/miugo/messages',
        tooltip: 'messages',
        divider: true,
      },
      {
        title: 'Components',
        icon: 'code',
        tooltip: 'components',
        class: 'dev-hidden',
        to: '/miugo/components',
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

  mounted() {
    this.$root.$on('closeMenu', this.closeMenu);
  },

  methods: {
    closeMenu() {
      this.openMenu = false;
    },
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
