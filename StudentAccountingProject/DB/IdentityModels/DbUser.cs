using Microsoft.AspNetCore.Identity;
using StudentAccountingProject.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.DB.IdentityModels
{
    public class DbUser : IdentityUser
    {
        public virtual BaseProfile BaseProfile { get; set; }
    }
}
