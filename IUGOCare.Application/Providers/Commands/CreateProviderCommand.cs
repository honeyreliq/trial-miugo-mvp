using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;

namespace IUGOCare.Application.Providers.Commands
{
    public class CreateProviderCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Phone { get; set; }
        public string AddressLines { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public Guid OrganizationId { get; set; }
    }

    public class CreateProviderCommandHandler: IRequestHandler<CreateProviderCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateProviderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
        {
            var organization = await _context.Organizations.FindAsync(request.OrganizationId);
            if (organization is null)
            {
                throw new NotFoundException(nameof(Organization), request.OrganizationId);
            }
            

            var provider = new Provider
            {
                Id = request.Id,
                Name = request.Name,
                Type = request.Type,
                Phone = request.Phone,
                Address = new Address()
                {
                    AddressLines = request.AddressLines,
                    City = request.City,
                    State = request.State,
                    Country = request.Country,
                    ZipCode = request.ZipCode
                },
                OrganizationId = request.OrganizationId
            };

            await _context.Providers.AddAsync(provider);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
