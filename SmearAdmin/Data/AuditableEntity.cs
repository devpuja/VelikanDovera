using System;
using System.Collections.Generic;

namespace SmearAdmin.Data
{
    public partial class AuditableEntity
    {
        public string Id { get; set; }
        public string RefTableId { get; set; }
        public string RefTableName { get; set; }
        public DateTime? FoundationDay { get; set; }
        public int? CommunityId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
