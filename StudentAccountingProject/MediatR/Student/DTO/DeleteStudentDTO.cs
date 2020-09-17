using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.DTO
{
    public class DeleteStudentInner
    {
        public string StudentId { get; set; }
    }

    public class DeleteStudentDTO
    {
        public List<DeleteStudentInner> students { get; set; }
    }
}
