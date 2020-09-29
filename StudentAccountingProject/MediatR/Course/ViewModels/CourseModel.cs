using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.DTO
{
    public class CourseItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AuthorModel Author { get; set; }
        public string PhotoPath { get; set; }
        public int Rating { get; set; }
        public string DateOfStart { get; set; }
        public string DateOfEnd { get; set; }
    }

    public class CourseModel
    {
        public string ErrorMessage { get; set; }
        public ICollection<CourseItem> Courses { get; set; }
    }

}
