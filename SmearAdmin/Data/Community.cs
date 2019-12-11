using System;
using System.Collections.Generic;

namespace SmearAdmin.Data
{
    public partial class Community
    {
        public int Id { get; set; }
        public string CommunityName { get; set; }
        public int? IsActive { get; set; }
    }
}
