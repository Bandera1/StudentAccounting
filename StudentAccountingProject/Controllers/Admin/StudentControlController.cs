using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccountingProject.MediatR.Student.Commands;
using StudentAccountingProject.MediatR.Student.DTO;
using StudentAccountingProject.MediatR.Student.Queries;

namespace StudentAccountingProject.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class StudentControlController : ApiController
    {
        [HttpGet("GetStudentsCount")]
        public async Task<IActionResult> GetStudentsCount()
        {
            var result = await Mediator.Send(new GetStudentsCountQuery());

            return Ok(result);
        }

        [HttpPost("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents([FromBody]GetAllStudentsQuery query) // ASK
        {
            var result = await Mediator.Send(query);

            if (!String.IsNullOrEmpty(result.ErrorMessage))
                return BadRequest(result);

            return Ok(result);        
        }

        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent([FromBody]CreateStudentCommand command)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await Mediator.Send(command);

            if (result.Status) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("EditStudent")]
        public async Task<IActionResult> EditStudent([FromBody]EditStudentCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await Mediator.Send(command);

            if (result.Status) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent([FromBody]DeleteStudentCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await Mediator.Send(command);

            if (result.Status) return Ok(result);
            return BadRequest(result);
        }

    }
}