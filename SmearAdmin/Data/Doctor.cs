using System;
using System.Collections.Generic;

namespace SmearAdmin.Data
{
    public partial class Doctor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string RegistrationNo { get; set; }
        public string Speciality { get; set; }
        public string Gender { get; set; }
        public string VisitFrequency { get; set; }
        public string VisitPlan { get; set; }
        public string BestDayToMeet { get; set; }
        public string BestTimeToMeet { get; set; }
        public string Brand { get; set; }
        public string Class { get; set; }
    }
}
