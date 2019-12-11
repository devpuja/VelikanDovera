using System.Collections.Generic;

namespace SmearAdmin.ViewModels
{
    public class DoctorViewModel
    {
        public string ID { get; set; }
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
        public List<string> BrandName { get; set; }
        public string Class { get; set; }
        public ContactResourseViewModel Contact { get; set; }
        public CommonResourceViewModel Common { get; set; }
        public AuditableEntityViewModel AuditableEntity { get; set; }
    }
}