using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.ViewModels
{
    public class StudentInfoModel
    {
        public string PhotoPath { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Age { get; set; }
        public bool IsFacebookAccount { get; set; }
    }
}
