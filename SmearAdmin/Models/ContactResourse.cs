using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SmearAdmin.Models
{
    public partial class ContactResourse
    {
        public long ID { get; set; }
        public string RefTableId { get; set; }
        public string RefTableName { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string ResidenceNumber { get; set; }
        public string OfficeNumber { get; set; }
    }

    public partial class RolesDetails
    {
        public string RoleName { get; set; }
        public string[] RoleClaims { get; set; }
        public IEnumerable<string> UserClaimsOnRoles { get; set; } //To Fetch Data 
    }
}
