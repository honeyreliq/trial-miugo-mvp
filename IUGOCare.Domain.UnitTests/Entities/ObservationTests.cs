using System;
using System.Collections.Generic;
using System.Text;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Domain.UnitTests.Entities
{
    [TestFixture]
    public class ObservationTests
    {
        [Test]
        public void ShouldReturnAnEmptyList_WhenNullIsProvided()
        {
            var o = new Observation();
            var list = o.SetObservationChange(null);
            Assert.AreEqual(0, list.Count);
        }

        public void ShouldReturnAllChangedItems()
        {
            var currentO = new Observation();
            var previousO = new Observation();
            var currentOD = new ObservationData { ObservationCode = "aaa", Value = 5 };
            var currentOD2 = new ObservationData { ObservationCode = "bbb", Value = 5 };
            var previousOD = new ObservationData { ObservationCode = "aaa", Value = 4 };

            currentO.ObservationsData.Add(currentOD);
            currentO.ObservationsData.Add(currentOD2);
            previousO.ObservationsData.Add(previousOD);

            var list = currentO.SetObservationChange(previousO);
            Assert.AreEqual(1, list.Count);
        }
    }
}
