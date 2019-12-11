using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmearAdmin.ViewModels
{
    public class RolesViewModel
    {
        public string RoleName { get; set; }
        public string[] RoleClaims { get; set; }
        public IEnumerable<string> UserClaimsOnRoles { get; set; } //To Fetch Data 
    }
}
