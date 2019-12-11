using System;
using System.Collections.Generic;

namespace SmearAdmin.Data
{
    public partial class Hqregion
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? Hqid { get; set; }
        public int? RegionId { get; set; }
    }
}
