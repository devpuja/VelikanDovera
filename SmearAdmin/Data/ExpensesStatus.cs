using System;
using System.Collections.Generic;

namespace SmearAdmin.Data
{
    public partial class ExpensesStatus
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string ExpenseMonth { get; set; }
        public int? Status { get; set; }
    }
}
