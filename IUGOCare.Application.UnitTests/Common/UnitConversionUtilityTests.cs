using IUGOCare.Domain.Common;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.UnitTests.Common
{
    public class UnitConversionUtilityTests
    {
        [TestCase("mg/dL", 102, "mmol/L", 5.7)]
        [TestCase("°F", 102, "°C", 38.9)]
        [TestCase("lbs", 153, "kg", 69.4)]
        [TestCase("mi", 20, "km", 32.2)]
        [TestCase("ft", 10, "m", 3)]
        [TestCase("oz", 60, "mL", 1774.4)]
        public void ShouldConvertToMetric(string originUnit, decimal value, string expectedUnit, decimal expectedValue)
        {
            var conversion = UnitConversionUtility.ConvertToMetricUnit(originUnit, value);

            Assert.AreEqual(expectedUnit, conversion.DestinationUnit);
            Assert.AreEqual(expectedValue, conversion.ConvertedValue);
        }
    }
}
