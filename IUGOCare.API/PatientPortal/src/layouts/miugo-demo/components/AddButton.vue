<template>
  <section v-if="showAdd.indexOf($route.name) > -1">
    <ObservationsDialog v-model="modals.openObservations"></ObservationsDialog>
    <DemoAddFileModal v-model="modals.openAddFile"></DemoAddFileModal>
    <v-speed-dial
        v-model="addButton"
        bottom
        right
        hover
        fixed
        direction="top"
        open-on-hover
        transition="slide-y-reverse-transition"
        class="add-speed-dial mb-4 mr-4"
    >
      <template v-slot:activator>
        <BaseButton
            v-model="addButton"
            color="primary"
            dark
            fab
            x-large
            elevation="4"
        >
          <fa-icon v-if="addButton" :icon="['fal', 'times']" class="fa-fw"/>
          <fa-icon v-else :icon="['fal', 'plus']" class="fa-fw"/>
        </BaseButton>
      </template>

      <BaseButton
          v-for="item in items" :key="item.title"
          fab
          dark
          :color="item.color"
          elevation="4"
          @click.stop="(item.handle) ? modals[item.handle] = true : null"
          v-tooltip="{
          content: item.tooltip,
          placement: 'left',
          classes: ['add-tooltop',$vuetify.theme.dark ? 'theme--dark' : 'theme--light'],
          offset: 10,
        }"
      >
        <fa-icon :icon="['fad', item.icon]" class="fa-fw"/>
      </BaseButton>
    </v-speed-dial>
  </section>
</template>

<script lang="ts">
// @ts-ignore
import Vue from 'vue';
// Utilities
import ObservationsDialog from '@/layouts/main/components/ObservationsDialog.vue';
import DemoAddFileModal from '@/views/miugo-demo/components/DemoAddFileModal.vue';

export default Vue.extend({
  name: 'AddButton',
  watch: {
    $route(to: any): void {
      const query: any = to.query;
      const openObservations: boolean = (query && query.observationdialog && query.observationdialog === '1');

      setTimeout(() => {
        this.modals.openObservations = openObservations;
        // delay to enhance the experience when open only
      }, (openObservations) ? 500 : 0);
    },
  },
  data(): any {
    return {
      addButton: false,
      modals: {
        openObservations: false,
        openAddFile: false,
      },
      openObservations: false,
      items: [
        {
          title: 'Record',
          icon: 'files-medical',
          tooltip: 'Add Record',
          color: 'colorFiles',
          handle: 'openAddFile',
        },
        {
          title: 'medication',
          icon: 'capsules',
          tooltip: 'Add Medication',
          color: 'colorMedications',
          handle: null,
        },
        {
          title: 'Observation',
          icon: 'analytics',
          tooltip: this.$t('addObservation'),
          color: 'colorObservations',
          handle: 'openObservations',
        },
      ],
      showAdd: [
        'observations',
        'My Care Plans - Chronic Care Management',
        'medications',
        'surveys',
        'education',
        'records',
      ],
    };
  },
  components: {ObservationsDialog, DemoAddFileModal},
});
</script>
