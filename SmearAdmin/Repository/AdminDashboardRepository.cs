using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Interface;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using static SmearAdmin.Helpers.Constants;

namespace SmearAdmin.Repository
{
    public class AdminDashboardRepository : Repositories<ExpensesStatus>, IAdminDashboardRepository
    {
        public AdminDashboardRepository(SmearAdminDbContext context) : base(context) { }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;

        public async Task<IEnumerable<EmployeeExpensesStatusViewModel>> GetAllSubmitNotification()
        {
            var dataUsers = await (from u in _appDbContext.Users
                             join es in _appDbContext.ExpensesStatus
                             on u.UserName equals es.UserName
                             where es.Status.Equals((int)EmployeeExpensesStatus.Submitted)
                             select new EmployeeExpensesStatusViewModel
                             {
                                 ID = es.Id,
                                 UserName = es.UserName,
                                 ExpenseMonth = es.ExpenseMonth,
                                 Status = es.Status,
                                 FullName = $"{u.FirstName} {u.MiddleName} {u.LastName}",
                             })
                             .ToListAsync().ConfigureAwait(false);

            return await Task.FromResult(dataUsers.AsEnumerable());
        }

        public async Task<IEnumerable<EmployeeExpensesStatusViewModel>> GetAllUserName()
        {
            var dataUsers = await (from u in _appDbContext.Users
                             where u.IsEnabled == true
                             select new EmployeeExpensesStatusViewModel
                             {
                                 UserName = u.UserName,
                                 FullName = $"{u.FirstName} {u.MiddleName} {u.LastName}",
                             })
                             .ToListAsync().ConfigureAwait(false);

            return await Task.FromResult(dataUsers.AsEnumerable().OrderBy(f => f.FullName));
        }

        public async Task<IEnumerable<EmployeeExpensesViewModel>> GetEmployeeExpensesInMonth(string UserName, string MonthYear)
        {
            var dataUsers = await (from e in _appDbContext.Expenses
                                   where e.UserName.Equals(UserName) && e.ExpenseMonth.Equals(MonthYear)
                                   select new EmployeeExpensesViewModel
                                   {
                                       ID = e.Id,
                                       UserName = e.UserName,
                                       PresentType = e.PresentType,
                                       ExpenseMonth = e.ExpenseMonth,
                                       Date = e.Date,
                                       HQ = e.Hq,
                                       HQName = (from m in _appDbContext.MasterKeyValue
                                                 where m.Id == e.Hq
                                                 select m.Value).FirstOrDefault(),
                                       Region = e.Region,
                                       BikeAllowance = e.BikeAllowance,
                                       DailyAllowance = e.DailyAllowance,
                                       OtherAmount = e.OtherAmount,
                                       ClaimAmount = e.ClaimAmount,
                                       EmployeeRemark = e.EmployeeRemark,
                                       ApprovedAmount = e.ApprovedAmount,
                                       ApprovedBy = e.ApprovedBy,
                                       ApproverRemark = e.ApproverRemark
                                   })
                             .ToListAsync().ConfigureAwait(false);

            return await Task.FromResult(dataUsers.AsEnumerable().OrderBy(f => f.Date));
        }

        public async Task<string> GetEmployeeExpenseForPDF(string UserName, string MonthYear)
        {
            int dataExpCount = _appDbContext.Expenses.Where(e => e.UserName.Equals(UserName) && e.ExpenseMonth.Equals(MonthYear)).ToList().Count();

            var dataExpenses = await (from e in _appDbContext.Expenses
                                      join es in _appDbContext.ExpensesStatus
                                      on e.ExpenseMonth equals es.ExpenseMonth
                                      where e.UserName.Equals(es.UserName) && e.ExpenseMonth.Equals(es.ExpenseMonth)
                                      && e.UserName.Equals(UserName) && e.ExpenseMonth.Equals(MonthYear)
                                      select new EmployeeExpensesViewModel
                                      {
                                          ID = e.Id,
                                          UserName = e.UserName,
                                          PresentType = e.PresentType,
                                          ExpenseMonth = e.ExpenseMonth,
                                          Date = e.Date,
                                          HQ = e.Hq,
                                          HQName = (from m in _appDbContext.MasterKeyValue
                                                    where m.Id == e.Hq
                                                    select m.Value).FirstOrDefault(),
                                          Region = e.Region,
                                          BikeAllowance = e.BikeAllowance,
                                          DailyAllowance = e.DailyAllowance,
                                          OtherAmount = e.OtherAmount,
                                          ClaimAmount = e.ClaimAmount,
                                          EmployeeRemark = e.EmployeeRemark,
                                          ApprovedAmount = e.ApprovedAmount,
                                          ApprovedBy = e.ApprovedBy,
                                          ApproverRemark = e.ApproverRemark,
                                          Status = es.Status
                                      }).OrderBy(x => x.Date).ToListAsync().ConfigureAwait(false);

            //dataExpenses.OrderBy(f => f.Date);

            var dataUsers = await (from u in _appDbContext.Users
                             where u.UserName.Equals(UserName)
                             select new RegistrationViewModel
                             {
                                 ID = u.Id,
                                 FirstName = u.FirstName,
                                 MiddleName = u.MiddleName,
                                 LastName = u.LastName,

                                 Department = u.Department,
                                 DepartmentName = (from depm in _appDbContext.MasterKeyValue
                                                   where depm.Id == u.Department
                                                   select depm.Value).FirstOrDefault(),

                                 Desigination = u.Desigination,
                                 DesiginationName = (from desm in _appDbContext.MasterKeyValue
                                                     where desm.Id == u.Desigination
                                                     select desm.Value).FirstOrDefault(),
                             }).ToListAsync().ConfigureAwait(false);

            var dataMobile = await (from mst in _appDbContext.MasterKeyValue
                                    where mst.Type.Equals(EmployeeConstant.Mobile)
                                    select Convert.ToInt32(mst.Value)).FirstOrDefaultAsync().ConfigureAwait(false);

            var dataFare = await (from mst in _appDbContext.MasterKeyValue
                                  where mst.Type.Equals(EmployeeConstant.Fare)
                                  select Convert.ToInt32(mst.Value)).FirstOrDefaultAsync().ConfigureAwait(false);

            var dataStationery = await (from mst in _appDbContext.MasterKeyValue
                                        where mst.Type.Equals(EmployeeConstant.Stationery)
                                        select Convert.ToInt32(mst.Value)).FirstOrDefaultAsync().ConfigureAwait(false);

            var dataCyber = await (from mst in _appDbContext.MasterKeyValue
                                   where mst.Type.Equals(EmployeeConstant.Cyber)
                                   select Convert.ToInt32(mst.Value)).FirstOrDefaultAsync().ConfigureAwait(false);

            if (dataExpCount < 15)
            {
                dataMobile /= 2;
                dataFare /= 2;
                dataStationery /= 2;
                dataCyber /= 2;
            }

            return GenerateHTML.GetHTMLEmployeeExpense(dataUsers.ToList()[0], dataExpenses.ToList(), dataMobile, dataFare, dataStationery, dataCyber);
        }
    }
}
