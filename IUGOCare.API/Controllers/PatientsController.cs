using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IUGOCare.Application.Patients.Commands.PatientAcceptsToS;
using IUGOCare.Application.Patients.Commands.EnableMarketingEmails;
using IUGOCare.Application.Patients.Queries.GetPatients;
using IUGOCare.Application.Patients.Queries.GetPatient;
using IUGOCare.Application.Patients.Commands.UpdatePatientPreferences;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.API.Models;
using IUGOCare.Application.Patients.Commands.UpdateEmailAddress;
using FluentValidation;
using IUGOCare.Application.Patients.Queries.GetEmailToken;
using IUGOCare.Application.Patients.Queries.GetProfileInformation;
using IUGOCare.Application.Patients.Commands.UpdatePassword;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using IUGOCare.Application.Common.Models;
using IUGOCare.Application.Common.Exceptions;
using System.Linq;

namespace IUGOCare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : BaseController
    {
        private readonly IIdentityService _identityService;

        public PatientsController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [Route("patients/{filter}")]
        public async Task<PatientsVm> GetPatientsAsync(PatientQueryFilter filter)
        {
            return await Mediator.Send(new GetPatientsQuery { Filter = filter });
        }

        [Route("patient")]
        public async Task<PatientVm> GetPatientAsync()
        {
            return await Mediator.Send(new GetPatientQuery());
        }

        [HttpPost]
        [Route("activate")]
        public async Task<IActionResult> ActivatePatient(PatientAcceptsToSCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Route("enableMarketingEmails")]
        public async Task<IActionResult> EnableMarketingEmails(EnableMarketingEmailsCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Route("updatePatientPreferences")]
        [Authorize]
        public async Task<IActionResult> UpdatePatientPreferences(UpdatePatientPreferencesCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Route("updateEmail")]
        public async Task<Result> UpdateEmail(PatientRequestUpdateEmailCommand command)
        {
            var patientId = await _identityService.GetCurrentPatientId();
            command.PatientId = patientId;
            try
            {
                await Mediator.Send(command);
                return Result.Success();
            }
            catch (FluentValidation.ValidationException vex)
            {
                return Result.Failure(vex);
            }
        }

        [HttpGet]
        [Route("updateEmail/{token}")]
        public async Task<EmailTokenVm> GetEmailToken(string token)
        {
            var query = new GetEmailToken() { Token = token };
            return await Mediator.Send(query);
        }

        [HttpPut]
        [Route("updateEmail/{token}")]
        public async Task<IActionResult> UseEmailToken(string token)
        {
            var query = new GetEmailToken() { Token = token };
            var vm = await Mediator.Send(query);

            var command = new UpdateEmailAddressCommand { PatientId = vm.PatientId, EmailAddress = vm.NewEmailAddress };
            await Mediator.Send(command);

            return NoContent();
        }
        
        [Route("patient/profile")]
        [Authorize]
        public async Task<PatientProfileVm> GetPatientProfile()
        {
            return await Mediator.Send(new GetProfileInformationQuery());
        }

        [HttpPost]
        [Route("updatePassword")]
        [Authorize]
        public async Task<Result> UpdatePassword(UpdatePasswordCommand command)
        {
            try
            {
                var patientId = await _identityService.GetCurrentPatientId();
                command.PatientId = patientId;
                await Mediator.Send(command);                
                return Result.Success();
            }
            catch (PasswordTooWeakException ex)
            {
                var errors = new List<string>(ex.Errors.Values.FirstOrDefault().AsEnumerable());                

                var result = Result.Failure(errors);               
                
                return result;
            }
        }
    }
}
