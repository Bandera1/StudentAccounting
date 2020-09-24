using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.ViewModels
{
    public class StudentItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Age { get; set; }
        public string Email { get; set; }
        public string RegisterDate { get; set; }
        public string DateOfCourseStart { get; set; }
    }


    public class StudentsModel
    {
        public string ErrorMessage { get; set; }
        public ICollection<StudentItem> Students { get; set; }
    }
}
