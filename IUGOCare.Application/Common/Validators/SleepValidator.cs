using System;
using System.Linq;
using System.Collections.Generic;
using IUGOCare.Application.Observations.Commands.CreateObservation;

namespace IUGOCare.Application.Common.Behaviours
{
    public static class SleepValidator
    {
        public static bool SleepValidationTimesMustNotSurpass24Hours(string observationCode, IList<ObservationDataItem> observationDataList)
        {
            if (observationCode.Equals("sleep"))
            {
                decimal secondsInADay = 86400;
                decimal totalSeconds = 0;

                foreach (var dataItem in observationDataList)
                {
                    if (!dataItem.ObservationCode.Equals("total"))
                    {
                        totalSeconds += dataItem.Value;
                    }
                }

                if (totalSeconds > secondsInADay)
                {
                    return false;
                }

            }

            return true;
        }

        public static bool SleepValidationTimesMustMatch(string observationCode, IList<ObservationDataItem> observationDataList)
        {
            if (observationCode.Equals("sleep"))
            {
                decimal totalGeneralCategory = observationDataList.Single(oc => oc.ObservationCode.Equals("total")).Value;

                decimal totalOtherCategories = 0;

                foreach (var dataItem in observationDataList)
                {
                    if (!dataItem.ObservationCode.Equals("total"))
                    {
                        totalOtherCategories += dataItem.Value;
                    }
                }

                if (totalGeneralCategory < totalOtherCategories)
                {
                    return false;
                }

            }

            return true;
        }
    }
}
