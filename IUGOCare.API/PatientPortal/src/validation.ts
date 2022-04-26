import i18n from './i18n';
import {LocaleMessages} from 'vue-i18n';

export type ValidationFn = (v: string) => true | string | LocaleMessages;

const isTruthy = (v: string, key = 'invalidEntryEnterAValue') => !!v || i18n.t(key);

export const exists: ValidationFn = (v) => isTruthy(v, 'invalidEntryEnterAValue');
export const isBetween: (min: number, max: number) => ValidationFn =
  (min, max) =>
  (v) => !v  || (Number(v) >= min && Number(v) <= max) || i18n.t('outOfRange', { min, max });
export const isInteger: ValidationFn = (v) => !v || /^[0-9]*$/.test(v) || i18n.t('valueMustBeANumber');
export const isDecimal: (digits: number) => ValidationFn = (digits) => {
  return (v: string) => {
    const pattern = `^([0-9]+(\\.[0-9]{0,${digits.toString()}})?)$`;
    const regExp = new RegExp(pattern);
    return v !== '' && regExp.test(v) || i18n.t('invalidEntryEnterAValue');
  };
};
export const isSelected: ValidationFn = (v) => isTruthy(v, 'invalidEntrySelectAValue');
export const isFileSelected: ValidationFn = (v) => isTruthy(v, 'invalidEntrySelectFile');
export const isHtmlFile: ValidationFn = (v: any) => !v || v.type === 'text/html' || i18n.t('invalidEnrtyFileTypeHtml');
export const isValidFileSize: ValidationFn = (v: any) => !v || v.size > 0 || i18n.t('invalidEnrtyFileSize');
export const isValidPhoneNumberFormat: ValidationFn = (v: string) => !v || /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/.test(v) || i18n.t('invalidEntryPhoneNumberFormat');
export const isValidZipCodeFormat: ValidationFn = (v: string) => !v || /^\d{5}(-\d{4})?(?!-)$/.test(v) || i18n.t('invalidEntryZipCodeFormat');
