﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Account.DTO
{
    public class NewPhotoModel
    {
        public string UserId { get; set; }
        public string ImageBase64 { get; set; }
    }
}
