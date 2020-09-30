using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.DTO
{
    public class NewCourseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public string PhotoBase64 { get; set; }
        public string DateOfStart { get; set; }
        public string DateOfEnd { get; set; }
    }
}
