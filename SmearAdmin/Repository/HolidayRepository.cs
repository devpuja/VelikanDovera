using Microsoft.EntityFrameworkCore;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Interface;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmearAdmin.Helpers.Constants;

namespace SmearAdmin.Repository
{
    public class HolidayRepository : Repositories<HolidayList>, IHolidayRepository
    {
        public HolidayRepository(SmearAdminDbContext context) : base(context) { }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;

        public async Task<PagingResult<HolidayViewModel>> GetAllHolidayAsync(int pageIndex, int pageSize)
        {
            int totalCount = 0, totalPage = 0;
            pageIndex += 1;

            totalCount = _appDbContext.HolidayList.Count();
            totalPage = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

            var dataUsers = await (from h in _appDbContext.HolidayList
                             select new HolidayViewModel
                             {
                                 ID = h.Id,
                                 FestivalName = h.FestivalName,
                                 FestivalDate = (DateTime)h.FestivalDate,
                                 FestivalDay = h.FestivalDay,
                                 FestivalDescription = h.FestivalDescription,
                                 IsNationalFestival = (int)h.IsNationalFestival,
                                 BelongToCommunity = (int)h.BelongToCommunity,
                                 CommunityName = (from c in _appDbContext.MasterKeyValue
                                              where c.Id == h.BelongToCommunity && c.Type.Equals(EmployeeConstant.Community)
                                              select c.Value).FirstOrDefault(),
                             })
                             .Skip((pageIndex - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync().ConfigureAwait(false);

            var pagingData = new PagingResult<HolidayViewModel>
            {
                Items = dataUsers.AsEnumerable().OrderBy(f => f.FestivalDate),
                TotalCount = totalCount,
                TotalPage = totalPage
            };

            return await Task.FromResult(pagingData);
        }

        public async Task<HolidayViewModel> GetHolidayByIDAsync(string ID)
        {
            var dataHoliday = await (from h in _appDbContext.HolidayList
                                      select new HolidayViewModel
                                      {
                                          ID = h.Id,
                                          FestivalName = h.FestivalName,
                                          FestivalDate = (DateTime)h.FestivalDate,
                                          FestivalDay = h.FestivalDay,
                                          FestivalDescription = h.FestivalDescription,
                                          IsNationalFestival = (int)h.IsNationalFestival,
                                          BelongToCommunity = (int)h.BelongToCommunity
                                      })
                             .Where(d => d.ID.Equals(Convert.ToInt32(ID))).FirstOrDefaultAsync().ConfigureAwait(false);

            return dataHoliday;
        }
    }
}
