using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.DB.Entities
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        //public string Photo { get; set; }
        public int Rating { get; set; }
        //public string File { get; set; } 
        public DateTime DateOfStart { get; set; }
        public DateTime DateOfEnd { get; set; }

        public virtual BaseProfile Author { get; set; }
        //public virtual ICollection<BaseProfile> Subscribers { get; set; }
    }
}
