<template>
    <v-expansion-panels v-bind="$attrs" v-model="expandableCard" flat>
      <v-expansion-panel :class="color">
        <v-expansion-panel-header class="text-h6 font-weight-bold"
          >{{ title }}</v-expansion-panel-header
        >
        <v-expansion-panel-content>
            <slot/>
        </v-expansion-panel-content>
      </v-expansion-panel>
    </v-expansion-panels>
</template>

<script>
export default {
    name: 'ExpandableCard',
    data: () => ({
        expandableCard: 0,
    }),
    props: {
      title: {
        type: String,
        default: '',
      },
      color: {
        type: String,
        default: '',
      },
      expanded: {
        type: Boolean,
        default: false,
      },
  },
}
</script>
