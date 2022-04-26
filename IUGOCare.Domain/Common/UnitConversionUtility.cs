using System;
using System.Collections.Generic;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Domain.Common
{
    public static class UnitConversionUtility
    {
        public static UnitConversion ConvertToMetricUnit(string originUnit, decimal value)
        {
            var unitConversion = new UnitConversion();
            decimal convertedValue;

            switch (originUnit)
            {
                case "mg/dL":
                    convertedValue = value / 18;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "mmol/L";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                case "°F":
                    convertedValue = (value - 32) / 1.8M;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "°C";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                case "lbs":
                    convertedValue = value / 2.2046M;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "kg";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                case "ft":
                    convertedValue = value * 0.3048M;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "m";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                case "mi":
                    convertedValue = value * 1.609M;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "km";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                case "oz":
                    convertedValue = value * 29.574M;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "mL";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                default:
                    unitConversion.DestinationUnit = originUnit;
                    unitConversion.ConvertedValue = value;
                    break;
            }
            return unitConversion;
        }

        public static UnitConversion ConvertToImperialUnit(string originUnit, decimal value)
        {
            var unitConversion = new UnitConversion();
            decimal convertedValue;

            switch (originUnit)
            {
                case "mmol/L":
                    convertedValue = value * 18;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "mg/dL";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                case "°C":
                    convertedValue = (value * 1.8M) + 32;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "°F";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                case "kg":
                    convertedValue = value * 2.2046M;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "lbs";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                case "m":
                    convertedValue = value / 0.3048M;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "ft";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                case "km":
                    convertedValue = value / 1.609M;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "mi";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                case "mL":
                    convertedValue = value / 29.574M;
                    convertedValue = Math.Round(convertedValue, 1);
                    unitConversion.DestinationUnit = "oz";
                    unitConversion.ConvertedValue = convertedValue;
                    break;
                default:
                    unitConversion.DestinationUnit = originUnit;
                    unitConversion.ConvertedValue = value;
                    break;
            }
            return unitConversion;
        }
    }
}
