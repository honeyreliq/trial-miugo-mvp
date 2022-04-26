<template>
  <v-dialog v-model="about" width="800px" @keydown.esc="about = false">
    <template v-slot:activator="{ on, attrs }">
      <v-list-item @click="aboutModal = true;" v-bind="attrs" v-on="on">
      <v-list-item-icon class="mr-4 ">
        <BaseLinkedImage
          :src="require('@/assets/iUGO_Mobile_avatar.svg')"
          contain
          width="24px"
          position="center center"
        />
      </v-list-item-icon>
      <v-list-item-content>
        <v-list-item-title>{{ $t('about')}}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </template>
    <v-card dark color="primary">
      <BaseToolbar>
        <v-spacer></v-spacer>
        <BaseButton icon dark @click="about = false">
          <fa-icon :icon="['fal', 'times']" class="fa-fw" />
        </BaseButton>
      </BaseToolbar>
      <v-card-text dark class="about-modal" v-html="getTranslationContent()">
      </v-card-text>
    </v-card>
  </v-dialog>
</template>
<script lang="ts">
  import Vue from 'vue';
  import { mapState } from 'vuex';
  import { ITranslationVm } from '@/IUGOCare-api';

  export default Vue.extend({
    name: 'AboutDialog',
    data: () => ({
      about: false,
      elementName: 'about',
    }),
    methods: {
      getTranslation(): void {
        this.$store.dispatch('translations/fetchTranslationByElementByLanguage', {
          elementName: this.elementName,
          language: this.getCurrentLanguage,
        });
      },
      getTranslationContent(): string {
        return atob(this.translations.find((t: ITranslationVm) =>
          t.elementName === this.elementName && t.language === this.getCurrentLanguage)?.fileContent || '');
      },
    },
    computed: {
      ...mapState('translations', ['translations']),
      getCurrentLanguage(): string {
        return this.$i18n.locale;
      },
    },
    watch: {
      about() {
        if (this.about) {
          this.getTranslation();
        }
      },
    },
    created() {
      this.getTranslation();
    },
    mounted() {
      this.$watch('$i18n.locale', this.getTranslation);
    },
  });
</script>
