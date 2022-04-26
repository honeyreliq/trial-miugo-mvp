<template>
    <div>
      <v-list-item v-for="(release, index) in releases" :key="index" :target="release.target" :href="release.href">
        <v-list-item-icon class="mr-4">
          <fa-icon :icon="['fad', 'code-branch']" class="fa-fw" />
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title v-text="$t(release.label)"></v-list-item-title>
          <v-list-item-subtitle v-text="release.current ? currentFullVersion : ''"></v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>
    </div>
</template>

<script>
export default {
  name: 'VersionMenu',

  data: () => ({
    releases: [
      // URLs should be pulled from the DB for when we have an admin section.
      {
        current: true,
        label: 'currentRelease',
        href: 'https://iugocare.freshdesk.com/support/solutions/folders/44001215254',
        target: '_blank',
      },
    ],
    closeOnContentClick: true,
  }),
  computed: {
    currentVersion() {
      const fvArray = this.$store.state.version.split('.');
      let version = '';

      if (fvArray.length > 1) {
        version = `V${fvArray[0]}.${fvArray[1]}`;
      } else {
        version = fvArray.length === 1 ? fvArray[0] : '';
      }

      return version;
    },
    currentFullVersion() {
      return this.$store.state.version;
    },
  },
};
</script>
