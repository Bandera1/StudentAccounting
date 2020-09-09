using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccountingProject.Helpers;
using StudentAccountingProject.MediatR.Course.Queries;

namespace StudentAccountingProject.Controllers.Student
{
    [Authorize(Roles = "Student")]
    public class StudentController : ApiController
    {
        [HttpGet("GetAllCourses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var studentId = User.Claims.ToList()[0].Value;

            var result = await Mediator.Send(new GetAllCourseQuery
            {
                DTO = new MediatR.Course.DTO.GetAllCoursesDTO { StudentId = studentId }
            });
            return Ok(result);
        }

        [HttpPost("SubscribeToCourse")]
        public async Task<IActionResult> SubscribeToCourse()
        {
            return Ok();
        }
    }
}