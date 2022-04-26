using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Commands.UpdatePatientInformation
{
    public class UpdatePatientInformationCommand : IRequest
    {
        public Guid ClinicPatientId { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }

    public class UpdatePatientInformationCommandHandler : IRequestHandler<UpdatePatientInformationCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePatientInformationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePatientInformationCommand command, CancellationToken cancellationToken)
        {
            var clinicPatient = await _context.ClinicPatients
                .FirstOrDefaultAsync(cp => cp.ClinicPatientId == command.ClinicPatientId);

            clinicPatient.GivenName = command.GivenName;
            clinicPatient.MiddleName = command.MiddleName;
            clinicPatient.FamilyName = command.FamilyName;
            clinicPatient.BirthDate = command.BirthDate;
            clinicPatient.Phone = command.Phone;
            clinicPatient.Address = new Address()
            {
                AddressLines = $"{command.AddressLine1} {command.AddressLine2}".Trim(),
                City = command.City,
                State = command.State,
                Country = command.Country,
                ZipCode = command.ZipCode,
            };

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
