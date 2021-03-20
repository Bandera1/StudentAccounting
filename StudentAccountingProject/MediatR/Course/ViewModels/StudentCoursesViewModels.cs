using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.ViewModels
{
    public class StudentCoursesViewModels
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
        public string DateOfStart { get; set; }
        public string DateOfEnd { get; set; }
        public string PhotoPath { get; set; }
    }
}
