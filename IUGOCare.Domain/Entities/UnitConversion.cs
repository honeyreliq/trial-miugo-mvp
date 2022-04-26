using System;
using System.Collections.Generic;
using System.Text;

namespace IUGOCare.Domain.Entities
{
    public class UnitConversion
    {
        public string DestinationUnit { set; get; }
        public decimal ConvertedValue { set; get; }
    }
}
