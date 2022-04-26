import {mount} from '../../../testHelpers/iugo-mounter';
// @ts-ignore
import Footer from '../Footer.vue';

describe('Footer.vue', () => {
  test('renders props.msg when passed', () => {
    const year = new Date().getFullYear();
    const msg = `© ${year} Reliq Health Technologies | All Rights Reserved`;
    const wrapper = mount(Footer);
    expect(wrapper.text()).toMatch(msg);
  });
});
