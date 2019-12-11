using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmearAdmin.Data;
using SmearAdmin.Interface;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmearAdmin.Helpers.Constants;

namespace SmearAdmin.Repository
{
    public class EmployeeExpensesRepository : Repositories<Expenses>, IEmployeeExpensesRepository
    {
        ILoggerFactory LoggerFactory;
        public EmployeeExpensesRepository(SmearAdminDbContext context,ILoggerFactory loggerFactory) : base(context) {
            LoggerFactory = loggerFactory;
        }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;

        public async Task<IEnumerable<EmployeeExpensesViewModel>> GetAllEmployeeExpensesInMonth(string UserName, string MonthYear)
        {
            //int totalCount = _appDbContext.Expenses.Where(e => e.UserName.Equals(UserName) && e.ExpenseMonth.Equals(MonthYear)).ToList().Count();
            var dataUsers = await (from e in _appDbContext.Expenses
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
                             })
                             .ToListAsync();

            //start logging to the console
            //var logger = LoggerFactory.CreateLogger<ConsoleLogger>();
            //logger.LogTrace("sss", dataUsers.AsEnumerable());

            return await Task.FromResult(dataUsers.AsEnumerable().OrderBy(f => f.Date));
        }

        public async Task<MasterKeyValueResult<MasterKeyValue>> GetEmployeeAllowanceDetails(string ID)
        {
            //string ID = "d21db5cb-ddb0-4669-82ef-cb77f3baf035";           
            //var dataBike = await _appDbContext.MasterKeyValue
            //    .Where(g => g.Type.Equals(Constants.EmployeeConstant.Bike)).ToListAsync();

            //var dataHQ = await _appDbContext.MasterKeyValue
            //    .Where(g => g.Type.Equals(Constants.EmployeeConstant.HQ)).OrderBy(o => o.Value).ToListAsync();

            
            var dataPresentType = await _appDbContext.MasterKeyValue
                .Where(pt => pt.Type.Equals(EmployeeConstant.PresentType)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataDaily = await (from mst in _appDbContext.MasterKeyValue
                                   where mst.Type.Equals(EmployeeConstant.Daily)
                                   select mst).FirstOrDefaultAsync().ConfigureAwait(false);

            var dataBike = await (from mst in _appDbContext.MasterKeyValue
                                  where mst.Type.Equals(EmployeeConstant.Bike)
                                  select mst).FirstOrDefaultAsync().ConfigureAwait(false);

            var dataHQ = await (from u in _appDbContext.Users
                          join mst in _appDbContext.MasterKeyValue
                          on u.HQ equals mst.Id
                          where u.Id == ID
                          select mst).FirstOrDefaultAsync().ConfigureAwait(false);

            //var dataHQName = (from u in _appDbContext.Users
            //                  join mst in _appDbContext.MasterKeyValue
            //                  on u.HQ equals mst.Id
            //                  where u.Id == ID
            //                  select mst.Value).FirstOrDefault();

            var dataRegion = await _appDbContext.MasterKeyValue
                .Where(r => r.Type.Equals(EmployeeConstant.Region)).OrderBy(o => o.Value).ToListAsync().ConfigureAwait(false);

            var dataRegionName = await (from hqrgn in _appDbContext.Hqregion
                                  join mst in _appDbContext.MasterKeyValue
                                  on hqrgn.RegionId equals mst.Id
                                  join u in _appDbContext.Users
                                  on hqrgn.UserId equals u.Id
                                  where (hqrgn.Hqid.Equals(u.HQ) && hqrgn.UserId.Equals(ID))
                                  select mst.Value).ToListAsync().ConfigureAwait(false);

            return new MasterKeyValueResult<MasterKeyValue>
            {
                ItemsPresentType = dataPresentType,
                ItemsDaily = dataDaily,
                ItemsBike = dataBike,
                HQ = dataHQ,
                ItemsRegion = dataRegion,
                ItemsRegionName = dataRegionName
            };
        }

        public async Task<IEnumerable<EmployeeExpensesStatusViewModel>> GetAllNotification(string UserName)
        {
            var dataStatus = await (from es in _appDbContext.ExpensesStatus
                              where es.UserName.Equals(UserName) && !es.Status.Equals((int)EmployeeExpensesStatus.Submitted)
                              select new EmployeeExpensesStatusViewModel
                              {
                                  ID = es.Id,
                                  UserName = es.UserName,
                                  ExpenseMonth = es.ExpenseMonth,
                                  Status = es.Status
                              }).OrderByDescending(i => i.ID).Take(5).ToListAsync().ConfigureAwait(false);

            return await Task.FromResult(dataStatus.AsEnumerable());
        }
    }
}