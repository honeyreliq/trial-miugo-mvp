using FluentValidation;

namespace IUGOCare.Application.PatientCareManagementPrograms.Commands.SetPatientCareManagementProgramEnrollment
{
    public class SetPatientCareManagementProgramEnrollmentCommandValidator : AbstractValidator<SetPatientCareManagementProgramEnrollmentCommand>
    {
        public SetPatientCareManagementProgramEnrollmentCommandValidator()
        {
            RuleFor(p => p.ClinicPatientId)
                .NotEmpty().WithMessage("The clinic patient Id is required.");

            RuleFor(p => p.CareProgramShortName)
                .NotEmpty().WithMessage("The care program short name is required.");
        }
    }
}
