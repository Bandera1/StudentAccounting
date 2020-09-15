using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccountingProject.MediatR.Student.Queries;

namespace StudentAccountingProject.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
    public class StudentControlController : ApiController
    {
        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var result = await Mediator.Send(new GetAllStudentsQuery());

            return Ok(result);
        }

        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent()
        {
            return Ok();
        }

        [HttpPost("EditStudent")]
        public async Task<IActionResult> EditStudent()
        {
            return Ok();
        }

        [HttpPost("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent()
        {
            return Ok();
        }
    }
}