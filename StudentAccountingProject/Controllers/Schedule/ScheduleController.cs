using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccountingProject.MediatR.Schedule.Queries;

namespace StudentAccountingProject.Controllers.Schedule
{
    [Authorize(Roles = "Student")]
    public class ScheduleController : ApiController
    {
        [HttpGet("GetSchedule")]
        public async Task<IActionResult> GetSchedule()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var query = new GetSheduleQuery 
            { 
                StudentId = User.Identities.First().Claims.First().Value
            };

            var result = await Mediator.Send(query);

            return Ok(result);
        }
    }
}