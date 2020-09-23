using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccountingProject.MediatR.Student.Commands;
using StudentAccountingProject.MediatR.Student.Queries;

namespace StudentAccountingProject.Controllers.Admin
{
    //[Authorize(Roles ="Admin")]
    public class StudentControlController : ApiController
    {
        [HttpGet("GetStudentsCount")]
        public async Task<IActionResult> GetStudentsCount()
        {
            var result = await Mediator.Send(new GetStudentsCountQuery());

            return Ok(result);
        }

        [HttpGet("GetAllStudents/{from}/{to}")]
        public async Task<IActionResult> GetAllStudents(int from, int to)
        {
            var result = await Mediator.Send(new GetAllStudentsQuery 
            {
                DTO = new MediatR.Student.DTO.GetAllStudentsDTO
                {
                    From = from,
                    To = to
                }
            });

            if (result.Status) return Ok(result);
            return BadRequest(result);
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