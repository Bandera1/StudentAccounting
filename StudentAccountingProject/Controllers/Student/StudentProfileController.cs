using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccountingProject.MediatR.Student.Commands;
using StudentAccountingProject.MediatR.Student.Queries;

namespace StudentAccountingProject.Controllers.Student
{
    [Authorize(Roles = "Student")]
    public class StudentProfileController : ApiController
    { 
        [HttpGet("GetStudentInfo")]
        public async Task<IActionResult> GetStudentInfo()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var studentId = User.Identities.First().Claims.First().Value;
            var query = new GetStudentInfoQuery { StudentId = studentId };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent([FromBody]EditStudentCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var studentId = User.Identities.First().Claims.First().Value;
            command.DTO.Id = studentId;

            var result = await Mediator.Send(command);
            if(String.IsNullOrEmpty(result.ErrorMessage))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody]ChangePasswordCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var studentId = User.Identities.First().Claims.First().Value;
            command.Model.StudentId = studentId;

            var result = await Mediator.Send(command);
            if(String.IsNullOrEmpty(result.ErrorMessage))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}