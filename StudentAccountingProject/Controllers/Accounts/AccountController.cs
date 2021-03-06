﻿using System;
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
            
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
          
            var result = await Mediator.Send(command);
            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
           
        }

        [HttpPost("sendConfirmEmail")]
        public async Task<IActionResult> sendConfirmEmail([FromBody]SendConfirmEmailCommand command)
        {
            var result = await Mediator.Send(command);

            if(result.Status) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> confirmEmail([FromBody]ConfirmEmailCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        [HttpPost("facebookLogin")]
        public async Task<IActionResult> facebookLogin([FromBody]FacebookLoginCommand command)
        {
            var result = await Mediator.Send(command);

            if (result.Status) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("changeImage")]
        public async Task<IActionResult> changeImage([FromBody]ChangeUserImageCommand command)
        {
            var studentId = User.Identities.First().Claims.First().Value;
            command.Model.UserId = studentId;

            var result = await Mediator.Send(command);
            return Ok(result);
        }

    }
}