using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmearAdmin.Models
{
    public partial class EmployeeExpenses
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string PresentType { get; set; }
        public string ExpenseMonth { get; set; }
        public DateTime? Date { get; set; }
        public int? Hq { get; set; }
        public string Region { get; set; }
        public int? BikeAllowance { get; set; }
        public int? DailyAllowance { get; set; }
        public int? OtherAmount { get; set; }
        public int? ClaimAmount { get; set; }
        public string EmployeeRemark { get; set; }
        public int? ApprovedAmount { get; set; }
        public string ApprovedBy { get; set; }
        public string ApproverRemark { get; set; }
    }
}
