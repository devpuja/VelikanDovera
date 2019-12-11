using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;

namespace SmearAdmin.Interface
{
    public interface IEmployeeExpensesRepository : IRepositories<Expenses>
    {
        Task<MasterKeyValueResult<MasterKeyValue>> GetEmployeeAllowanceDetails(string ID);

        Task<IEnumerable<EmployeeExpensesViewModel>> GetAllEmployeeExpensesInMonth(string UserName, string MonthYear);

        Task<IEnumerable<EmployeeExpensesStatusViewModel>> GetAllNotification(string UserName);
    }
}
