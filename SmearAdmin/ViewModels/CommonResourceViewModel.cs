using System;

namespace SmearAdmin.ViewModels
{
    public class CommonResourceViewModel
    {
        public string ID { get; set; }
        public string RefTableId { get; set; }
        public string RefTableName { get; set; }
        public string DrugLicenseNo { get; set; }
        public string FoodLicenseNo { get; set; }
        public string GSTNo { get; set; }
        public string BestTimeToMeet { get; set; }       
        public string ContactPersonName { get; set; }
        public string ContactPersonMobileNumber { get; set; }
        public DateTime? ContactPersonDateOfBirth { get; set; }
        public DateTime? ContactPersonDateOfAnniversary { get; set; }
    }
}
