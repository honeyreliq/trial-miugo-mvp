<template>
  <v-dialog v-model="privacy" width="600px" @keydown.esc="privacy = false">
    <template v-slot:activator="{ on, attrs }">
      <v-list-item @click="aboutModal = true;" v-bind="attrs" v-on="on">
      <v-list-item-icon class="mr-4 ">
        <fa-icon :icon="['fad', 'user-lock']" class="fa-fw" />
      </v-list-item-icon>
      <v-list-item-content>
        <v-list-item-title>{{ $t('privacyPolicy')}}</v-list-item-title>
      </v-list-item-content>
      </v-list-item>
    </template>
    <v-card>
      <BaseToolbar :title="$t('privacyPolicy')">
        <v-spacer></v-spacer>
        <BaseButton icon dark @click="privacy = false">
          <fa-icon :icon="['fal', 'times']" class="fa-fw" />
        </BaseButton>
      </BaseToolbar>
      <v-card-text v-html="getTranslationContent()" />
      <v-card-actions>
        <v-spacer></v-spacer>
        <BaseButton color="primary" block @click="privacy = false">{{ $t('close')}}</BaseButton>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script lang="ts">
  import Vue from 'vue';
  import { mapState } from 'vuex';
  import { ITranslationVm } from '@/IUGOCare-api';

  export default Vue.extend({
    name: 'PrivacyDialog',
    data: () => ({
      privacy: false,
      elementName: 'privacyPolicy',
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
      privacy() {
        this.$root.$emit('closeMenu');
        if (this.privacy) {
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
