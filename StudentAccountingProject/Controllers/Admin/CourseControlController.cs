using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccountingProject.MediatR.Course.Commands;
using StudentAccountingProject.MediatR.Course.Queries;

namespace StudentAccountingProject.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class CourseControlController : ApiController
    {
        [HttpGet("GetCoursesCount")]
        public async Task<IActionResult> GetCoursesCount()
        {
            var result = await Mediator.Send(new GetCoursesCountQuery());

            return Ok(result);
        }

        [HttpGet("GetAuthors")]
        public async Task<IActionResult> GetAuthors()
        {
            var result = await Mediator.Send(new GetAuthorsQuery());

            return Ok(result);
        }

        [HttpPost("GetCourses")]
        public async Task<IActionResult> GetCourses([FromBody]GetCoursesQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await Mediator.Send(query);

            if (!String.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("CreateCourse")]
        public async Task<IActionResult> CreateStudent([FromBody]CreateCourseCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await Mediator.Send(command);

            if (String.IsNullOrEmpty(result.ErrorMessage))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("DeleteCourse")]
        public async Task<IActionResult> DeleteCourse([FromBody]DeleteCourseCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await Mediator.Send(command);

            if (String.IsNullOrEmpty(result.ErrorMessage))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    } 
}