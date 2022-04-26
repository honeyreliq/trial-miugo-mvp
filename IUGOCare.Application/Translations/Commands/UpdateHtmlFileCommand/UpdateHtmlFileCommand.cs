using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Translations.Commands.UpdateHtmlFileCommand
{
    public class UpdateHtmlFileCommand : IRequest
    {
        public string ElementName { get; set; }
        public string Language { get; set; }
        public byte[] FileContent { get; set; }
    }

    public class UpdateHtmlFileCommandHandler : IRequestHandler<UpdateHtmlFileCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateHtmlFileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateHtmlFileCommand request, CancellationToken cancellationToken)
        {
            var translation = await _context.Translations
                .FirstOrDefaultAsync(t => t.ElementName.Equals(request.ElementName) &&
                t.Language.Equals(request.Language));

            if(translation is null)
            {
                throw new NotFoundException(nameof(Translation), $"{request.ElementName}/{request.Language}");
            }

            translation.FileContent = request.FileContent;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
