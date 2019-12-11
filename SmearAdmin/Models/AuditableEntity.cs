using System;
using System.Collections.Generic;

namespace SmearAdmin.Models
{
    public partial class AuditableEntity
    {
        public long Id { get; set; }
        public long? RefTableId { get; set; }
        public string RefTableName { get; set; }
        public string FoundationDay { get; set; }
        public int? CommunityId { get; set; }
        public int? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
