using System;
using System.Collections.Generic;

namespace SmearAdmin.ViewModels
{
    public class RegistrationViewModel
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; } //Not Required for registration
        public string Email { get; set; }
        public DateTime? DOJ { get; set; }
        public DateTime? DOB { get; set; }
        public string Password { get; set; } //First Auto Generate Then Update

        public int? Department { get; set; }
        public string DepartmentName { get; set; }
        public int? Desigination { get; set; }
        public string DesiginationName { get; set; }
        public int? Grade { get; set; }
        public string GradeName { get; set; }
        public int? HQ { get; set; }
        public string HQName { get; set; }
        public IEnumerable<int> Region { get; set; }
        public IEnumerable<string> RegionName { get; set; }
        public string Pancard { get; set; }
                
        public string CustomUserName { get; set; }
        public string PasswordRaw { get; set; }
        public bool IsEnabled { get; set; }
        public ContactResourseViewModel Contact { get; set; }
        public RolesViewModel Roles { get; set; }
    }
}
