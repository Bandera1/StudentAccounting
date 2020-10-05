using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.DB.IdentityModels;
using StudentAccountingProject.MediatR.Account.DTO;
using StudentAccountingProject.MediatR.Account.ViewModels;
using StudentAccountingProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Account.Commands
{
    public class SendConfirmEmailCommand : IRequest<SendConfirmEmailViewModel>
    {
        public SendConfirmEmailDTO DTO { get; set; }

        public class SendConfirmEmailCommandHandler : BaseMediator, IRequestHandler<SendConfirmEmailCommand, SendConfirmEmailViewModel>
        {

            public SendConfirmEmailCommandHandler(EFDbContext context) : base(context)
            {
            }

            public async Task<SendConfirmEmailViewModel> Handle(SendConfirmEmailCommand request, CancellationToken cancellationToken)
            {
                var user = Context.Users.FirstOrDefault(x => x.Id == request.DTO.UserId);
                if (user == null)
                {
                    return new SendConfirmEmailViewModel { Status = false, ErrorMessage = "User doesn`t exist" };
                }

                if (user.EmailConfirmed)
                {
                    return new SendConfirmEmailViewModel { Status = false, ErrorMessage = "Email alredy confirmed" };
                }

                EmailService emailService = new EmailService();
                string url = "https://localhost:44310/#/confirm/email" + "/" + "ID=" + user.Id;
                string emailHTML = $@"<p>&nbsp;</p>
<!-- HIDDEN PREHEADER TEXT -->
<div style=""display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;"">We're thrilled to have you here! Get ready to dive into your new account.</div>
<table border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0""><!-- LOGO -->
<tbody>
<tr>
<td align=""center"" bgcolor=""#FFA73B"">
<table style=""max-width: 600px;"" border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
<tbody>
<tr>
<td style=""padding: 40px 10px 40px 10px;"" align=""center"" valign=""top"">&nbsp;</td>
</tr>
</tbody>
</table>
</td>
</tr>
<tr>
<td style=""padding: 0px 10px 0px 10px;"" align=""center"" bgcolor=""#FFA73B"">
<table style=""max-width: 600px;"" border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
<tbody>
<tr>
<td style=""padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;"" align=""center"" valign=""top"" bgcolor=""#ffffff"">
<h1 style=""font-size: 48px; font-weight: 400; margin: 2;"">Confirm email</h1>
<img style=""display: block; border: 0px;"" src="" https://img.icons8.com/clouds/100/000000/handshake.png"" width=""125"" height=""120"" /></td>
</tr>
</tbody>
</table>
</td>
</tr>
<tr>
<td style=""padding: 0px 10px 0px 10px;"" align=""center"" bgcolor=""#f4f4f4"">
<table style=""max-width: 600px;"" border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
<tbody>
<tr>
<td align=""left"" bgcolor=""#ffffff"">
<table border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
<tbody>
<tr style=""height: 39px;"">
<td style=""padding: 20px 30px 60px; height: 39px;"" align=""center"" bgcolor=""#ffffff"">
<table border=""0"" cellspacing=""0"" cellpadding=""0"">
<tbody>
<tr>
<td style=""border-radius: 3px;"" align=""center"" bgcolor=""#FFA73B""><a style=""font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #FFA73B; display: inline-block;"" href=""{url}"" target=""_blank"" rel=""noopener"">Confirm Account</a></td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
<tr>
<td style=""padding: 30px 10px 0px 10px;"" align=""center"" bgcolor=""#f4f4f4"">&nbsp;</td>
</tr>
<tr>
<td style=""padding: 0px 10px 0px 10px;"" align=""center"" bgcolor=""#f4f4f4"">&nbsp;</td>
</tr>
</tbody>
</table>";
                await emailService.SendEmailAsync(user.Email, emailHTML, "Email confirm");

                return new SendConfirmEmailViewModel { Status = true };
            }
        }
    }
}

