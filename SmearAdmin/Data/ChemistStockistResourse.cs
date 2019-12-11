using System;
using System.Collections.Generic;

namespace SmearAdmin.Data
{
    public partial class ChemistStockistResourse
    {
        public string Id { get; set; }
        public string RefTableId { get; set; }
        public string RefTableName { get; set; }
        public string DrugLicenseNo { get; set; }
        public string FoodLicenseNo { get; set; }
        public string Gstno { get; set; }
        public string BestTimeToMeet { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonMobileNumber { get; set; }
        public DateTime? ContactPersonDateOfBirth { get; set; }
        public DateTime? ContactPersonDateOfAnniversary { get; set; }
    }
}
