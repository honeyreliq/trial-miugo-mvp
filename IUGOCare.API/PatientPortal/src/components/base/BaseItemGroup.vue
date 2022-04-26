<template>
  <v-list-group
      :group="group"
      :sub-group="subGroup"
      append-icon="mdi-menu-down"
      :active-class="`naturalGrey white--text`"
      :class="item.color"
      class="BaseItemGroup"
      v-model="expanded"
  >
    <template v-slot:activator>
      <v-list-item v-tooltip="enableTooltip(item)" class="ma-0" :class="!useTooltip ? 'pa-0' : ''">
        <v-list-item-icon v-if="item.icon" class="icon">
          <span class="item-icon p-relative d-flex justify-center">
            <fa-icon :icon="['fad', item.icon]" class="fa-fw"/>
            <v-fade-transition>
              <fa-icon v-if="useTooltip"
                       :icon="['fas', ((expanded) ? 'caret-up' : 'caret-down')]"
                       class="fa-fw item-arrow mt-8"/>
            </v-fade-transition>
          </span>
        </v-list-item-icon>

        <v-list-item-content>
          <v-list-item-title v-text="item.title" class="text-subtitle-1"/>
        </v-list-item-content>
      </v-list-item>
    </template>
    <template v-for="(child, i) in children">
      <BaseItemSubGroup
          v-if="child.children"
          :key="`sub-group-${i}`"
          :item="child"
          class="second-dd"
      />
      <BaseItem
          v-tooltip="enableTooltip(child)"
          v-if="!child.children"
          :key="`item-${i}`"
          :item="child"
          text
      />
    </template>
  </v-list-group>
</template>

<script>
// Utilities
import kebabCase from 'lodash/kebabCase';

export default {
  name: 'BaseItemGroup',
  inheritAttrs: false,

  data() {
    return {
      expanded: false,
    };
  },

  props: {
    item: {
      type: Object,
      default: () => ({
        avatar: undefined,
        group: undefined,
        title: undefined,
        children: [],
      }),
    },
    subGroup: {
      type: Boolean,
      default: false,
    },
    text: {
      type: Boolean,
      default: false,
    },
    useTooltip: {
      type: Boolean,
      default: false,
    },
  },

  computed: {
    children() {
      return this.item.children.map((item) => ({
        ...item,
        to: !item.to ? undefined : `${this.item.group}/${item.to}`,
      }));
    },
    computedText() {
      if (!this.item || !this.item.title) {
        return '';
      }

      let text = '';

      this.item.title.split(' ').forEach((val) => {
        text += val.substring(0, 1);
      });

      return text;
    },
    group() {
      return this.genGroup(this.item.children);
    },
  },

  methods: {
    genGroup(children) {
      return children
          .filter((item) => item.to)
          .map((item) => {
            const parent = item.group || this.item.group;
            let group = `${parent}/${kebabCase(item.to)}`;

            if (item.children) {
              group = `${group}|${this.genGroup(item.children)}`;
            }

            return group;
          }).join('|');
    },
    enableTooltip(item) {
      // v-tooltip has not a prop to disable, have to be manually
      if (this.useTooltip && item.tooltip) {
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

<style lang="scss">
.BaseItemGroup {
  .v-list-group__activator p {
    margin-bottom: 0;
  }

  .item-arrow {
    // p-absolute not working for the icon
    position: absolute;
    // had to decrease the default size of the icon to match with the expanded one
    width: 15px !important;
    height: 15px !important;
  }
}
</style>
