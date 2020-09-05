using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccountingProject.MediatR.Account.Commands;
using StudentAccountingProject.MediatR.Account.ViewModels;

namespace StudentAccountingProject.Controllers.Accounts
{
    public class AccountController : ApiController
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
    
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           //try
            //}
                var result = await Mediator.Send(command);
                if (result.Status)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            //}
            //catch (Exception e)
            //{
            //    var res = new RegistrationViewModel { Status = false, ErrorMessage = e.Message };
            //    return BadRequest(res);
            //}
        }
    }
}