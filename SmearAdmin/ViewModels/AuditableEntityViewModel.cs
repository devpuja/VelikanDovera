using System;

namespace SmearAdmin.ViewModels
{
    public class AuditableEntityViewModel
    {
        public string ID { get; set; }
        public string RefTableId { get; set; }
        public string RefTableName { get; set; }
        public bool IsActive { get; set; }
        public int CommunityID { get; set; }
        public string CommunityName { get; set; }
        public DateTime? FoundationDay { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
