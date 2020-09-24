using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.DTO
{
    public class GetAllStudentFilter
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Age { get; set; }
    }

    public class GetAllStudentsDTO
    {
        public GetAllStudentFilter Filter { get; set; }
        public int CurrentPage { get; set; }
        public int Rows { get; set; }
    }
}
