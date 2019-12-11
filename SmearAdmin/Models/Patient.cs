using System;
using System.Collections.Generic;

namespace SmearAdmin.Models
{
    public partial class Patient
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string PatientName { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
    }
}
