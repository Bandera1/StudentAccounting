using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Schedule.ViewModels
{
    public class ScheduleItem
    {
        public string Title { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }

    public class ScheduleModel
    {
        public ICollection<ScheduleItem> Items { get; set; }
    }
}
