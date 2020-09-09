using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.DTO
{
    public class SubscribeToCourseDTO
    {
        public string CourseId { get; set; }
        public string StudentId { get; set; }
    }
}
