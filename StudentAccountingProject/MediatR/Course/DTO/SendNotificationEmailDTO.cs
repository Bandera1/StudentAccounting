using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.DTO
{
    public class SendNotificationEmailDTO
    {
        public string UserEmail { get; set; }
        public string CourseName { get; set; }
        public int DaysCount { get; set; }
    }
}
