using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.DB.Entities
{
    public class StudentToCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string CourseId { get; set; }

        public virtual BaseProfile Student { get; set; }
        public virtual Course Course { get; set; }
    }
}
