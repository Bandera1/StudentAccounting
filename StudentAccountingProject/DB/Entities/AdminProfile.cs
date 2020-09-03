using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.DB.Entities
{
    public class AdminProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string BaseProfileId { get; set; }

        public virtual BaseProfile BaseProfile { get; set; }
    }
}
