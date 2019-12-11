using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmearAdmin.Models
{
    public class EmployeeExpensesStatus
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string ExpenseMonth { get; set; }
        public int? Status { get; set; }
    }
}
