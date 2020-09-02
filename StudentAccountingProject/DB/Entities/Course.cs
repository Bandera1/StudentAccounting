using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.DB.Entities
{
    public class Course
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public string Photo { get; set; }
        public float Rating { get; set; }
        public string File { get; set; } 
        public DateTime DateOfStart { get; set; }
             
        public virtual BaseProfile Author { get; set; }
    }
}
