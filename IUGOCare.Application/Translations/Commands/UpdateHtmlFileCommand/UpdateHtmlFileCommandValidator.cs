using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using IUGOCare.Domain.Common.Constants;

namespace IUGOCare.Application.Translations.Commands.UpdateHtmlFileCommand
{
    public class UpdateHtmlFileCommandValidator : AbstractValidator<UpdateHtmlFileCommand>
    {
        public UpdateHtmlFileCommandValidator()
        {
            var validLanguages = new List<string> { Languages.EnglishLanguage, Languages.SpanishLanguage };

            RuleFor(v => v.ElementName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("The element is required.")
                .NotEmpty().WithMessage("The element is required.");

            RuleFor(v => v.Language)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("The language is required.")
                .NotEmpty().WithMessage("The language is required.")
                .Must(x => validLanguages.Contains(x.ToUpper())).WithMessage($"The language is incorrect, must be { string.Join(" or ", validLanguages)}.");

            RuleFor(v => v.FileContent)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("The file is required.")
                .NotEmpty().WithMessage("The file is required.")
                .Must(c => c.Length > 0).WithMessage("Please select a file whose size is greater than zero.")
                .Must(c => c.All(item => !string.IsNullOrWhiteSpace(item.ToString()))).WithMessage("Please select a file whose size is greater than zero.");
        }
    }
}
