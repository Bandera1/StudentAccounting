using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.DTO
{
    public class DelCourseItem
    {
        public string CourseId { get; set; }
    }
    public class DelCourseModel
    {
        public List<DelCourseItem> Courses { get; set; }
    }
}
