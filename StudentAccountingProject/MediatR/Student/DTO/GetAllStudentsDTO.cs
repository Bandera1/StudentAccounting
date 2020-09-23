using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.DTO
{
    public class GetAllStudentsDTO
    {
        public int From { get; set; }
        public int To { get; set; }
    }
}
