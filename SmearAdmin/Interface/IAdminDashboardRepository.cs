using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmearAdmin.Data;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;

namespace SmearAdmin.Interface
{
    public interface IAdminDashboardRepository : IRepositories<ExpensesStatus>
    {
        Task<IEnumerable<EmployeeExpensesStatusViewModel>> GetAllSubmitNotification();
        Task<IEnumerable<EmployeeExpensesViewModel>> GetEmployeeExpensesInMonth(string UserName, string MonthYear);
        Task<IEnumerable<EmployeeExpensesStatusViewModel>> GetAllUserName();
        Task<string> GetEmployeeExpenseForPDF(string UserName, string MonthYear);
    }
}
