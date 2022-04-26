using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Patients.Commands.PatientAcceptsToS;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Entities;
using MediatR;
using Moq;
using NUnit.Framework;

namespace IUGOCare.Application.UnitTests.Patients.Commands
{
    public class PatientAcceptsToSTests : CommandTestBase
    {
        [Test]
        public async Task ShouldFirePatientActivatedEvent()
        {
            var mediatorMock = new Mock<IMediator>();
            var identityMock = new Mock<IIdentityService>();

            var p = new Patient() { Id = TestConstants.AllOnesGuid };
            var command = new PatientAcceptsToSCommand() { PatientId = p.Id };
            var handler = new PatientAcceptsToSCommandHandler(Context, mediatorMock.Object, identityMock.Object);

            await handler.Handle(command, CancellationToken.None);

            mediatorMock.Verify(m => m.Publish(It.Is<PatientActivated>(pa => pa.PatientId == p.Id), It.IsAny<CancellationToken>()));
        }

    }
}
