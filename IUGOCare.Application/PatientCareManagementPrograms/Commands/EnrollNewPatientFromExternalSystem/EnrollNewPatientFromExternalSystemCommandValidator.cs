using FluentValidation;

namespace IUGOCare.Application.PatientCareManagementPrograms.Commands.EnrollNewPatientFromExternalSystem
{
    public class EnrollNewPatientFromExternalSystemCommandValidator : AbstractValidator<EnrollNewPatientFromExternalSystemCommand>
    {
        public EnrollNewPatientFromExternalSystemCommandValidator()
        {
            RuleFor(e => e.ClinicPatientId).NotEmpty();
            RuleFor(e => e.CarePrograms).NotNull();
        }
    }
}
