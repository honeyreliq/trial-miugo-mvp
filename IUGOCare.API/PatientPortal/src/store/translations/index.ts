import { Module } from 'vuex';
import { RootState } from '../types';
import { TranslationsClient, ITranslationDto, ITranslationVm } from '../../IUGOCare-api';
import axiosClient from '@/auth/axiosInstance';

interface TranslationsState {
  error: boolean;
  errorMessage: string;
  loadingElements: boolean;
  translationElements: ITranslationDto[];
  translations: ITranslationVm[];
}

const state: TranslationsState = {
  error: false,
  errorMessage: '',
  loadingElements: false,
  translationElements: [],
  translations: [],
};

const client = new TranslationsClient(process.env.VUE_APP_API_URL, axiosClient);

const namespaced = true;

export const translations: Module<TranslationsState, RootState> = {
  namespaced,
  state,
  actions: {
    async fetchTranslationElements(context) {
      context.commit('setLoading', true);
      try {
        context.commit('setTranslationElements', []);
        const response = await client.getTranslationElements();
        context.commit('setTranslationElements', response.translations || []);
      } catch (e) {
        context.commit('setError', `Error while loading translation elements: ${e.message}`);
      }
      context.commit('setLoading', false);
    },
    async fetchTranslationByElementByLanguage(context, { elementName, language }) {
      try {
        await client.getTranslationByElementByLanguage(elementName, language)
          .then((translation) => context.commit('setTranslationByElementByLanguage', translation));
      } catch (e) {
        context.commit('setError', `Error while loading translation elements: ${e.message}`);
      }
    },
  },
  mutations: {
    setError(localState, error: string) {
      localState.error = true;
      localState.errorMessage = error;
    },
    setTranslationElements(localState, translationDtos: ITranslationDto[]) {
      localState.translationElements = translationDtos;
    },
    setTranslationByElementByLanguage(localState, translation: ITranslationVm) {
      const index = localState.translations.findIndex((item: ITranslationVm) =>
        item.elementName === translation.elementName && item.language === translation.language);
      if (index !== -1) {
        localState.translations.splice(index, 1);
      }
      localState.translations.push(translation);
    },
    setLoading(localState, payload: boolean) {
      localState.loadingElements = payload;
      localState.error = false;
      localState.errorMessage = '';
    },
  },
};
