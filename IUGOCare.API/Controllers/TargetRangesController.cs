using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IUGOCare.Application.TargetRanges.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IUGOCare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetRangesController : BaseController
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PatientTargetRangesVm>> GetPatientTargetRanges()
        {
            return await Mediator.Send(new GetPatientTargetRangesQuery());
        }
    }
}
