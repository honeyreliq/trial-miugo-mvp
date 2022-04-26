import * as conversionHelpers from '../conversion-helpers';

describe('Conversion Helpers', () => {
  test('convertToF should convert', () => {
    const f = conversionHelpers.convertToF(18, '°C');
    expect(f).toEqual(64.4);
  });

  test('convertToF should not convert', () => {
    const f = conversionHelpers.convertToF(18, '°F');
    expect(f).toEqual(18);
  });

  describe('convertToFt', () => {
    test('should do nothing if the units are already feet', () => {
      const ft = Math.random();
      expect(conversionHelpers.convertToFt(ft, 'ft')).toEqual(ft);
    });

    test('should convert from metres', () => {
      const ft = conversionHelpers.convertToFt(1.76, 'mt');
      expect(ft).toEqual(6);
    });

    test('should convert from kilometres', () => {
      const ft = conversionHelpers.convertToFt(1.76, 'km');
      expect(ft).toEqual(5774);
    });

    test('should convert from miles', () => {
      const ft = conversionHelpers.convertToFt(1.76, 'mi');
      expect(ft).toEqual(9293);
    });
  });

  describe('convertToMgdl', () => {
    test('should do nothing for mg/dL', () => {
      const mgdl = Math.random();
      expect(conversionHelpers.convertToMgdl(mgdl, 'mg/dL')).toEqual(mgdl);
    });

    test('should convert from mmol/L', () => {
      expect(conversionHelpers.convertToMgdl(25, 'mmol/L')).toEqual(450);
    });
  });

  describe('secondsToHms', () => {
    test('should convert', () => {
      expect(conversionHelpers.secondsToHms(86402)).toEqual('24:00:02');
    });
  });
});
