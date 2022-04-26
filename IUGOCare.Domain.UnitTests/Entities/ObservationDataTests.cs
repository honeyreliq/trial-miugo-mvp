using System;
using System.Collections.Generic;
using System.Text;
using IUGOCare.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace IUGOCare.Domain.UnitTests.Entities
{
    [TestFixture]
    public class ObservationDataTests
    {
        [Test]
        public void ShouldReturnFalse_WhenNullIsProvided()
        {
            var od = new ObservationData();
            Assert.IsFalse(od.SetObservationChange(null));
        }

        [Test]
        public void ShouldReturnFalse_WhenCodesDoNotMatch()
        {
            var current = new ObservationData { ObservationCode = "aaa" };
            var previous = new ObservationData { ObservationCode = "bbb" };
            Assert.IsFalse(current.SetObservationChange(previous));
        }


        [TestCase(1, ObservationChange.Rising, true)]
        [TestCase(4, ObservationChange.Rising, true)]
        [TestCase(5, ObservationChange.Steady, false)]
        [TestCase(6, ObservationChange.Falling, true)]
        [TestCase(10, ObservationChange.Falling, true)]
        public void ShouldSetThePropertyProperly(int oldValue, ObservationChange expectedChange, bool itChanged)
        {
            var current = new ObservationData { ObservationCode = "aaa", Value = 5 };
            var previous = new ObservationData { ObservationCode = "aaa", Value = oldValue };

            bool changed = current.SetObservationChange(previous);
            Assert.AreEqual(expectedChange, current.Change);
            Assert.AreEqual(changed, itChanged);
        }
    }
}
