using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Account.ViewModels
{
    public class SendConfirmEmailViewModel
    {
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
