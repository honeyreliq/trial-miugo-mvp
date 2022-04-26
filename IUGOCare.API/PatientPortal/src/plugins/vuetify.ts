import Vue from 'vue';
import Vuetify from 'vuetify/lib';
import i18n from '@/i18n';
import '@/sass/overrides.sass';
import { LangTranslator } from 'vuetify/types/services/lang';

Vue.use(Vuetify);

const baseTheme = {
  // Colors
  primary: '#007EC7',
  secondary: '#7558B3',
  tertiary: '#007EC7',
  accent: '#FFC0CB',
  error: '#DA4250',
  info: '#007EC7',
  success: '#2EA44F',
  warning: '#EEC138',
  // Greys
  lighterGrey: '#F5F5F5',
  lightGrey: '#EEEEEE',
  naturalGrey: '#424242',
  darkGrey: '#363636',
  darkerGrey: '#060606',
};

const lightTheme = {
  appbar: baseTheme.lighterGrey,
  background: baseTheme.lighterGrey,
  backgroundAlt: baseTheme.lighterGrey,
  bottomNav: baseTheme.primary,
};

const darkTheme = {
  appbar: baseTheme.darkerGrey,
  background: baseTheme.darkerGrey,
  backgroundAlt: baseTheme.darkGrey,
  bottomNav: baseTheme.primary,
};

const pageTheme = {
  colorObservations: baseTheme.secondary,
  colorCareplans: baseTheme.primary,
  colorMedications: baseTheme.success,
  colorSurveys: baseTheme.tertiary,
  colorFiles: baseTheme.warning,
  colorEducation: baseTheme.primary,
  colorMessages: baseTheme.primary,
};

const langTranslator: LangTranslator = (key, ...params) => i18n.t(key, params) as string;

export default new Vuetify({
  lang: {
    t: langTranslator,
  },
  theme: {
    dark: false,
    default: 'light',
    themes: {
      dark: {
        ...baseTheme,
        ...darkTheme,
        ...pageTheme,
      },
      light: {
        ...baseTheme,
        ...lightTheme,
        ...pageTheme,
      },
    },
    options: {
      customProperties: true,
    },
  },
});
