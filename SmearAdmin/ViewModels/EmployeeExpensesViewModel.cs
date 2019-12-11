using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmearAdmin.Data;

namespace SmearAdmin.ViewModels
{
    public class EmployeeExpensesViewModel
    {
        public long ID { get; set; }
        public string UserName { get; set; }
        public string PresentType { get; set; }
        public string ExpenseMonth { get; set; }
        public DateTime? Date { get; set; }
        public int? HQ { get; set; }
        public string HQName { get; set; }
        public string Region { get; set; }
        public int? BikeAllowance { get; set; }
        public int? DailyAllowance { get; set; }
        public int? OtherAmount { get; set; }
        public int? ClaimAmount { get; set; }
        public string EmployeeRemark { get; set; }
        public int? ApprovedAmount { get; set; }
        public string ApprovedBy { get; set; }
        public string ApproverRemark { get; set; }
        public int? Status { get; set; }

    }

    public class EmployeeExpensesStatusViewModel
    {
        public long ID { get; set; }
        public string UserName { get; set; }
        public string ExpenseMonth { get; set; }
        public int? Status { get; set; }
        public string FullName { get; set; }
    }

    //public class MasterKeyValueExpensesResult<T>
    //{
    //    public IEnumerable<T> ItemsPresentType { get; set; }
    //    public MasterKeyValue ItemsDaily { get; set; }
    //    public MasterKeyValue ItemsBike { get; set; } 
    //    public IEnumerable<T> ItemsRegion { get; set; }
    //    public MasterKeyValue HQ { get; set; }
    //    public IEnumerable<string> ItemsRegionName { get; set; }
    //}

    public class PDFViewModel
    {
        public byte[] File { get; set; }
        public string FullFilePath { get; set; }
        public string FileName { get; set; }
    }
}
