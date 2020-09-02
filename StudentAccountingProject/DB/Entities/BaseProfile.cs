using StudentAccountingProject.DB.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.DB.Entities
{
    public class BaseProfile
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Photo { get; set; }

        public virtual StudentProfile ClientProfile { get; set; }
        public virtual AdminProfile ManagerProfile { get; set; }

        public virtual DbUser User { get; set; }
    }
}
