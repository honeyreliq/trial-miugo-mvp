using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Observations.Commands.CreateManualObservation;
using IUGOCare.Application.Observations.Commands.CreateObservation;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Entities;
using MediatR;
using Moq;
using NUnit.Framework;

namespace IUGOCare.Application.UnitTests.Observations.Commands
{
    public class ObservationCreatedTests : CommandTestBase
    {
        [Test]
        public async Task ShouldFireObservationCreatedEvent()
        {
            var mediatorMock = new Mock<IMediator>();
            var currentUserMock = new Mock<ICurrentUserService>();

            var o = new Observation() { Id = TestConstants.AllOnesGuid };
            var command = new CreateManualObservationCommand();
            var handler = new CreateObservationManuallyHandler(Context, mediatorMock.Object, currentUserMock.Object);

            await handler.Handle(command, CancellationToken.None);

            mediatorMock.Verify(m => m.Publish(It.Is<ObservationCreated>(ob => true), It.IsAny<CancellationToken>()));
        }
    }
}
