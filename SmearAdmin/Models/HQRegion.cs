using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmearAdmin.Models
{
    public partial class HQRegion
    {
        //public int Id { get; set; }
        //public string UserId { get; set; }
        //public int? Hqid { get; set; }
        //public int? RegionId { get; set; }

        public int ID { get; set; }
        public string UserID { get; set; }
        public int? HQID { get; set; }
        public int? RegionID { get; set; }
    }
}
