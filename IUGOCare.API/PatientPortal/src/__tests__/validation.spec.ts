import * as v from '../validation';

const falseyValues: any[] = ['', null, undefined, 0, false];
const goodStrings: string[] = ['a', 'A longer string', '😬👋🏻👨‍👩‍👧‍👦🚵🏾‍♀️', 'undefined', 'null', 'הָיְתָהtestالصفحات التّحول'];

describe('Validation', () => {
  describe('exists', () => {
    test('should return an error for falsey values', () => {
      falseyValues.forEach((falsey) => {
        const response = v.exists(falsey);
        expect(response).toEqual('Invalid Entry! Please enter a value.');
      });
    });

    test('should true for truthy values', () => {
      goodStrings.forEach((truthy) => {
        const response = v.exists(truthy);
        expect(response).toEqual(true);
      });
    });
  });

  describe('isBetween', () => {
    test('should pass validation', () => {
      const fn = v.isBetween(6, 9);
      [6, 7, 8, 9].forEach((n) => expect(fn(n.toString())).toEqual(true));
    });

    test('should fail validation', () => {
      const fn = v.isBetween(6, 9);
      [4, 5, 10, 11].forEach((n) => expect(fn(n.toString())).toEqual('Invalid Entry! Valid range is from 6 to 9.'));
    });
  });

  describe('isInteger', () => {
    test('should pass validation', () => {
      ['6', '7', '8', '9'].forEach((n) => expect(v.isInteger(n.toString())).toEqual(true));
    });

    test('should fail validation', () => {
      ['asdf', '9.95'].forEach((n) => expect(v.isInteger(n.toString()))
        .toEqual('Invalid Entry! Please enter a number.'));
    });
  });

  describe('isSelected', () => {
    test('should return an error for falsey values', () => {
      falseyValues.forEach((falsey) => {
        const response = v.isSelected(falsey);
        expect(response).toEqual('Invalid Entry! Please select a value.');
      });
    });

    test('should true for truthy values', () => {
      goodStrings.forEach((truthy) => {
        const response = v.isSelected(truthy);
        expect(response).toEqual(true);
      });
    });
  });
});
