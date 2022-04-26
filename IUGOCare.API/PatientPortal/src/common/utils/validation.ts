import i18n from '../../i18n';
import {LocaleMessages} from 'vue-i18n';

const isTruthy = (v: string, key = 'invalidEntryEnterAValue') => !!v || i18n.t(key);

export type ValidationFn = (v: string) => true | string | LocaleMessages;

export const isBetween: (min: number, max: number) => ValidationFn =
  (min, max) =>
  (v) => !v  || (Number(v) >= min && Number(v) <= max) || i18n.t('outOfRange', { min, max });
export const isInteger: ValidationFn = (v) => !v || /^[0-9]*$/.test(v) || i18n.t('valueMustBeANumber');
export const required: ValidationFn = (v) => isTruthy(v, 'required');
export const exists: ValidationFn = (v) => isTruthy(v, 'invalidEntryEnterAValue');
export const isDecimal: (digits: number) => ValidationFn = (digits) => {
  return (v: string) => {
    const pattern = `^([0-9]+(\\.[0-9]{0,${digits.toString()}})?)$`;
    const regExp = new RegExp(pattern);
    return v !== '' && regExp.test(v) || i18n.t('invalidEntryEnterAValue');
  };
};
export const isSelected: ValidationFn = (v) => isTruthy(v, 'invalidEntrySelectAValue');