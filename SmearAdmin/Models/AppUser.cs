using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SmearAdmin.Models
{
    public class AppUser : IdentityUser
    {
        public long? FacebookId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public DateTime? DOJ { get; set; }
        public DateTime? DOB { get; set; }

        public int? Department { get; set; }
        public int? Desigination { get; set; }
        public int? Grade { get; set; }
        public int? HQ { get; set; }     
        public string Pancard { get; set; }
        public string PasswordRaw { get; set; }
        public bool IsEnabled { get; set; }
    }
}
