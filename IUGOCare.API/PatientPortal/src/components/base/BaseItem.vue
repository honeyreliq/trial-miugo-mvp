<template>
  <v-list-item
    :href="href"
    :rel="href && href !== '#' ? 'noopener' : undefined"
    :target="href && href !== '#' ? '_blank' : undefined"
    :to="item.to"
    :active-class="`primary white--text`"
    :class="item.color"
  >
    <v-list-item-icon
      v-if="item.icon"
     >
      <fa-icon :icon="['fad', item.icon]" class="fa-fw" />

    </v-list-item-icon>
    <v-list-item-icon
      v-else
      class="v-list-item__icon--text text-subtitle-2"
      v-text="computedText"
    />
    <v-list-item-content v-if="item.title">
      <v-list-item-title v-text="item.title" class="text-subtitle-1" />
    </v-list-item-content>
  </v-list-item>
</template>

<script>
  import Themeable from 'vuetify/lib/mixins/themeable';

  export default {
    name: 'BaseItem',

    mixins: [Themeable],

    props: {
      item: {
        type: Object,
        default: () => ({
          href: undefined,
          icon: undefined,
          title: undefined,
          to: undefined,
        }),
      },
      text: {
        type: Boolean,
        default: false,
      },
    },

    computed: {
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
      href() {
        return this.item.href || (!this.item.to ? '#' : undefined);
      },
    },
  };
</script>
