import * as textHelpers from '../text-helpers';

describe('Text Helpers', () => {
  describe('kebabToCamelCase', () => {
    test('should return a a camel case string', () => {
      const actual = ['identity', 'meal-code-fasting', 'alreadyCamelCase'];
      const expected = ['identity', 'mealCodeFasting', 'alreadyCamelCase'];
      actual.forEach((s, i) => {
        const response = textHelpers.kebabToCamelCase(s);
        expect(response).toEqual(expected[i]);
      });
    });
  });
});
