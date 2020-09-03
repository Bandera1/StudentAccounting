using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentAccountingProject.MediatR.Account.Commands;
using StudentAccountingProject.MediatR.Account.ViewModels;

namespace StudentAccountingProject.Controllers.Accounts
{
    public class RegistrationController : ApiController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Registration([FromBody]RegistrationCommand command)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var res = await Mediator.Send(command);
                if (res.Status)
                {
                    return Ok(res.Token);
                }
                else
                {
                    return BadRequest(res);
                }
            }
            catch (Exception e)
            {
                var res = new RegistrationViewModel { Status = false, ErrorMessage = e.Message };
                return BadRequest(res);
            }
        }
    }
}