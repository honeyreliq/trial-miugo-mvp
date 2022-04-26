using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;

namespace IUGOCare.Application.Organizations.Commands
{
    public class CreateOrganizationCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string AddressLines { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }

    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateOrganizationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var organization = new Organization
            {
                Id = request.Id,
                Name = request.Name,
                Phone = request.Phone,
                Address = new Address()
                {
                    AddressLines = request.AddressLines,
                    City = request.City,
                    State = request.State,
                    Country = request.Country,
                    ZipCode = request.ZipCode
                }
            };

            await _context.Organizations.AddAsync(organization);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
