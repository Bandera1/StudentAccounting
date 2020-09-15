using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.ViewModels
{
    public class BaseViewModel
    {
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
