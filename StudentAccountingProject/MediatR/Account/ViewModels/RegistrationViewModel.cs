﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Account.ViewModels
{
    public class RegistrationViewModel
    {
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
    }
}
