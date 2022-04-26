using System;
using System.Threading.Tasks;
using IUGOCare.Application.Observations.Commands.CreateManualObservation;
using IUGOCare.Application.Observations.Queries.GetPatientObservations;
using IUGOCare.Application.Observations.Queries.GetRecentPatientObservationTypes;
using IUGOCare.Application.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Shared.Protocol;
using IUGOCare.Domain.Common.Constants;

namespace IUGOCare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationsController : BaseController
    {
        [HttpGet("filter")]
        [Authorize]
        public async Task<ActionResult<PatientObservationsVm>> GetPatientObservations(
            [FromQuery(Name="clinicPatientId")]Guid clinicPatientId,
            [FromQuery(Name="observationCodes")]string[] observationCodes,
            [FromQuery(Name="effectiveDateStart")]DateTimeOffset effectiveDateStart,
            [FromQuery(Name="effectiveDateEnd")]DateTimeOffset effectiveDateEnd)
        {
            return await Mediator.Send(new GetPatientObservationsQuery {
                ClinicPatientId = clinicPatientId,
                EffectiveDateStart = effectiveDateStart,
                EffectiveDateEnd = effectiveDateEnd,
                ObservationCodes = observationCodes,
                UnitSystem = UnitSystems.ImperialSystem
            });
        }

        [HttpPost]
        [Route("create")]
        [Authorize]
        public async Task<IActionResult> CreateManualObservation(CreateManualObservationCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        [Route("recentPatientObservationTypes")]
        [Authorize]
        public async Task<ActionResult<RecentPatientObservationTypesVm>> GetRecentPatientObservationTypes()
        {
            return await Mediator.Send(new GetRecentPatientObservationTypesQuery());
        }
    }
}
