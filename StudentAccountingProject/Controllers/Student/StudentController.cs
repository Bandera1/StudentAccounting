using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccountingProject.Helpers;
using StudentAccountingProject.MediatR.Course.Commands;
using StudentAccountingProject.MediatR.Course.Queries;
using System.Security.Claims;

namespace StudentAccountingProject.Controllers.Student
{
    //[Authorize(Roles = "Student")]
    public class StudentController : ApiController
    {
        [HttpGet("GetAllCourses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var studentId = User.Identities.First().Claims.First().Value;

            var result = await Mediator.Send(new GetAllCourseQuery
            {
                DTO = new MediatR.Course.DTO.GetAllCoursesDTO { StudentId = studentId }
            });
            return Ok(result);
        }

        [HttpPost("SubscribeToCourse")]
        public async Task<IActionResult> SubscribeToCourse([FromBody]SubscribeToCourseCommand command)
        {
            var studentId = User.Identities.First().Claims.First().Value;

            command.DTO.StudentId = studentId;
            var result = await Mediator.Send(command);

            if (result.Status) return Ok(result);
            return BadRequest(result);
        }
    }
}