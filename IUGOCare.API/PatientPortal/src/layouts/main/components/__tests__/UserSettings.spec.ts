import {mount} from '../../../../testHelpers/iugo-mounter';
// @ts-ignore
import UserSettings from '../UserSettings.vue';
import { patients, PatientsState } from '../../../../store/patients';

describe('UserSettings.vue', () => {
  beforeEach(() => {
    (patients.state as PatientsState).currentPatient = {
      emailAddress: 'billy.dee@example.com',
      givenName: 'Billy',
      middleName: 'Dee',
      familyName: 'Williams',
      birthDate: new Date(1937, 3, 6),
    };
  });

  test('Should render the user\'s email address', () => {
    const msg = /billy.dee@example.com/;
    const wrapper = mount((UserSettings as any), { selectedOption: 'account'}, { modules: { patients } });
    expect(wrapper.text()).toMatch(msg);
  });

  test('Should render the user\'s name', () => {
    const msg = /Billy Dee Williams/;
    const wrapper = mount((UserSettings as any), { selectedOption: 'profile'}, { modules: { patients } });

    const input = wrapper.find('v-text-field-stub[name="fullName"]');
    expect(input.props().value).toMatch(msg);
  });

  test('Should render the birthday', () => {
    const msg = /April 6, 1937/;
    const wrapper = mount((UserSettings as any), { selectedOption: 'profile'}, { modules: { patients } });

    const input = wrapper.find('v-text-field-stub[name="birthDate"]');
    expect(input.props().value).toMatch(msg);
  });
});
