using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.DB.Entities
{
    public class AdminProfile
    {
        [Key]
        public string Id { get; set; }

        public bool IsSuperAdmin { get; set; }
    }
}
