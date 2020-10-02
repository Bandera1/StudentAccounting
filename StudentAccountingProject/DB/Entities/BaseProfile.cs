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
        public BaseProfile()
        {
            IsDeleted = false;
        }

        [Key, ForeignKey("User")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhotoPath { get; set; }
        public string Age { get; set; }
        public DateTime RegisterDate { get; set; }
        public string UserId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFacebookAccount { get; set; }

        public virtual StudentProfile StudentProfile { get; set; }
        public virtual AdminProfile AdminProfile { get; set; }

        public virtual DbUser User { get; set; }
    }
}
