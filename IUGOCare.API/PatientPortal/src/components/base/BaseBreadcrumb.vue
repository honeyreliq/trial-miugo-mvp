<template>
  <v-row>
    <v-col cols="12" sm="12">
      <v-card flat>
        <v-toolbar dense flat class="transparent">
          <v-toolbar-title>
            <v-breadcrumbs :items="breadcrumbs" class="pa-0">
              <template v-slot:divider v-if="breadcrumbs">
                <fa-icon :icon="['fad', 'chevron-right']" class="fa-fw" />
              </template>
            </v-breadcrumbs>
            <slot></slot>
          </v-toolbar-title>
          <v-spacer></v-spacer>
        </v-toolbar>
      </v-card>
    </v-col>
  </v-row>
</template>

<script>
export default {
  name: 'BaseBreadcrumb',

  props: {
    title: String,
    breadcrumbs: Array,
    icon: String,
  },
  data: () => ({}),
};
</script>

