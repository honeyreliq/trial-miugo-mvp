import Vue from 'vue';
import {mount} from '../../../../testHelpers/iugo-mounter';
// @ts-ignore
import UpdateEmail from '../UpdateEmail.vue';
import {patients, PatientsState} from '../../../../store/patients';
import { Wrapper } from '@vue/test-utils';

interface ComponentType {
  vm: Vue;
  element: any;
}

describe('UpdateEmail.vue', () => {
  const component: ComponentType = {
    vm: null,
    element: null,
  };
  let wrapper: Wrapper<Vue> = null;

  beforeEach(() => {
    (patients.state as PatientsState).currentPatient = {
      emailAddress: 'billy.dee@example.com',
      givenName: 'Billy',
      middleName: 'Dee',
      familyName: 'Williams',
      birthDate: new Date(1937, 3, 6),
    };

    wrapper = mount((UpdateEmail as any), {}, {modules: {patients}});
    component.vm = wrapper.vm;
    component.element = wrapper;
  });

  test('should exists', () => {
    expect(component.vm).toBeTruthy();
  });

  test('should contain "Save" button', () => {
    expect(component.element.text().includes('Save')).toBeTruthy();
  });

  test('should contain "Update Email" modal', () => {
    const toolbar = wrapper.find('basetoolbar-stub');
    expect(toolbar.html().includes('Update Email')).toBeTruthy();
  });

  test('should contain "Email Updated" modal', () => {
    expect(component.element.text().includes('Email Sent!')).toBeTruthy();
  });

  describe('validators', () => {
    let validators: any;

    beforeEach(() => {
      validators = component.vm.$data.validators;
    });

    test('should have "requiredEmailRules" method for fields', () => {
      expect(validators.requiredEmailRules).toBeDefined();
    });

    test('should have "passwordRequired" method for fields', () => {
      expect(validators.passwordRequired).toBeDefined();
    });

    test('should have "matchEmail" method for fields', () => {
      expect(validators.matchEmail).toBeDefined();
    });

  });

  describe('data', () => {
    let data: any;

    beforeEach(() => {
      data = component.vm.$data;
    });

    test('should have "component" object to manage the component', () => {
      expect(data.component.valid).toBeDefined();
      expect(data.component.disabled).toBeDefined();
      expect(data.component.showDialog).toBeDefined();
    });

    test('should have "data" object to confirm the email', () => {
      expect(data.data.email).toBeDefined();
      expect(data.data.confirm).toBeDefined();
      expect(data.data.pwd).toBeDefined();
    });

    test('should have "error" object to manage errors', () => {
      expect(data.error.show).toBeDefined();
      expect(data.error.messages).toBeDefined();
    });

    test('should have "error" object to manage errors', () => {
      expect(data.success.show).toBeDefined();
    });

  });

});
