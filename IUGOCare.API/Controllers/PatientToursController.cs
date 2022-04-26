using System.Threading.Tasks;
using IUGOCare.Application.Patients.Commands.CompleteTour;
using IUGOCare.Application.Patients.Queries.ShouldStartTour;
using IUGOCare.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IUGOCare.API.Controllers
{
    [Route("api/patients/tours")]
    [ApiController]
    [Authorize]
    public class PatientToursController : BaseController
    {
        [Route("{tourKey}/skip")]
        [HttpPost]
        public async Task<IActionResult> SkipTour(string tourKey)
        {
            var ctc = new CompleteTourCommand { TourKey = tourKey, CompletionReason = CompletionReason.Skipped };
            await Mediator.Send(ctc);
            return NoContent();
        }

        [Route("{tourKey}/finish")]
        [HttpPost]
        public async Task<IActionResult> FinishTour(string tourKey)
        {
            var ctc = new CompleteTourCommand { TourKey = tourKey, CompletionReason = CompletionReason.Finished };
            await Mediator.Send(ctc);
            return NoContent();
        }

        [Route("{tourKey}")]
        [HttpGet]
        public async Task<bool> ShouldStartTour(string tourKey)
        {
            var query = new ShouldStartTourQuery { TourKey = tourKey };
            return await Mediator.Send(query);
        }
    }
}
