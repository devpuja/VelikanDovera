using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmearAdmin.ViewModels
{
    public class HQRegionViewModel
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int? HQID { get; set; }
        public int? RegionID { get; set; }
    }
}
