import {mount} from '../../../testHelpers/iugo-mounter';
// @ts-ignore
import BaseButtonGroup from '../BaseButtonGroup.vue';

const props = {
  value: 'my-value',
  color: 'green',
};

describe('BaseButtonGroup.vue', () => {
  let wrapper: any;
  beforeEach(() => {
    wrapper = mount((BaseButtonGroup as any), {value: props.value, color: props.color}, {});
  });

  it('should exists', () => {
    expect(wrapper.vm).toBeTruthy();
  });

  it('should contain the given "value" prop', () => {
    expect(wrapper.vm.currentValue).toBe(props.value);
  });

  it('should contain the given "color" prop', () => {
    expect(wrapper.vm.color).toBe(props.color);
  });

});
