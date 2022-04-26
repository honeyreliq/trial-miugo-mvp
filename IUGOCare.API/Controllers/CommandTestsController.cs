using System;
using System.Threading.Tasks;
using IUGOCare.Application.CareManagementPrograms.Queries;
using IUGOCare.Application.Observations.Commands.CreateObservation;
using IUGOCare.Application.Observations.Commands.UpdateObservation;
using IUGOCare.Application.Organizations.Commands;
using IUGOCare.Application.Organizations.Queries;
using IUGOCare.Application.PatientCareManagementPrograms.Commands.SetPatientCareManagementProgramEnrollment;
using IUGOCare.Application.Patients.Commands.DisableMarketingEmails;
using IUGOCare.Application.Patients.Commands.EnableMarketingEmails;
using IUGOCare.Application.Patients.Commands.PatientAcceptsToS;
using IUGOCare.Application.Patients.Commands.RegisterPatient;
using IUGOCare.Application.Patients.Commands.SendPatientOnboardingEmail;
using IUGOCare.Application.Patients.Commands.UpdateEmailAddress;
using IUGOCare.Application.Patients.Queries.GetPatients;
using IUGOCare.Application.Providers.Commands;
using IUGOCare.Application.Providers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IUGOCare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandTestsController : BaseController
    {
        [Route("patients/{filter}")]
        [Authorize]
        public async Task<PatientsVm> GetPatientsAsync(PatientQueryFilter filter)
        {
            return await Mediator.Send(new GetPatientsQuery { Filter = filter });
        }

        [HttpPost]
        [Route("PatientAcceptsToS")]
        [Authorize]
        public async Task<IActionResult> PatientAcceptsToS(PatientAcceptsToSCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Route("patient/register")]
        [Authorize]
        public async Task<IActionResult> RegisterPatient(RegisterPatientCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Route("SendPatientOnboardingEmail")]
        [Authorize]
        public async Task<IActionResult> SendPatientOnboardingEmail(SendPatientOnboardingEmailCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Route("EnableMarketingEmails")]
        [Authorize]
        public async Task<IActionResult> EnableMarketingEmails(EnableMarketingEmailsCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Route("DisableMarketingEmails")]
        [Authorize]
        public async Task<IActionResult> DisableMarketingEmails(DisableMarketingEmailsCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut]
        [Route("patient/updateemailaddress")]
        [Authorize]
        public async Task<ActionResult<Unit>> UpdateEmailAddress(UpdateEmailAddressCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        [Route("CreateObservation")]
        [Authorize]
        public async Task<ActionResult<Unit>> CreateObservation(CreateObservationCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut]
        [Route("updateObservation")]
        [Authorize]
        public async Task<ActionResult<Guid>> UpdateObservation(UpdateObservationCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        [Route("careManagementPrograms")]
        [Authorize]
        public async Task<ActionResult<CareManagementProgramsVm>> GetCareManagementPrograms()
        {
            return await Mediator.Send(new GetCareManagementProgramsQuery());
        }

        [HttpPost]
        [Route("createOrganization")]
        [Authorize]
        public async Task<IActionResult> CreateOrganization(CreateOrganizationCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        [Route("organizations")]
        [Authorize]
        public async Task<ActionResult<OrganizationsVm>> GetOrganizations()
        {
            return await Mediator.Send(new GetOrganizationsQuery());
        }

        [HttpPost]
        [Route("setPatientCareManagementProgramEnrollment")]
        [Authorize]
        public async Task<IActionResult> SetPatientCareMangementProgramEnrollment(SetPatientCareManagementProgramEnrollmentCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Route("createProvider")]
        [Authorize]
        public async Task<IActionResult> CreateProvider(CreateProviderCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        [Route("providers")]
        [Authorize]
        public async Task<ActionResult<ProvidersVm>> GetProviders()
        {
            return await Mediator.Send(new GetProvidersQuery());
        }
    }
}
