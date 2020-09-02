﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace StudentAccountingProject.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string message, string subject)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("bonvoyagevirus@gmail.com");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = message;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("bonvoyagevirus@gmail.com", "bon123456-"),
                EnableSsl = true
            };
            client.Send(mail);
        }
    }
}
