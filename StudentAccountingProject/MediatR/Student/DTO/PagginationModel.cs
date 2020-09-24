using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.DTO
{
    public class StudentFilter 
    {
        public string SearchCriteria { get; set; }
    }

    public class PagginationModel  
    {
        public StudentFilter Filter { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
