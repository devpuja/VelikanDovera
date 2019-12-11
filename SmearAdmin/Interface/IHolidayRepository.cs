using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmearAdmin.Interface
{
    public interface IHolidayRepository : IRepositories<HolidayList>
    {
        Task<PagingResult<HolidayViewModel>> GetAllHolidayAsync(int pageIndex, int pageSize);
        Task<HolidayViewModel> GetHolidayByIDAsync(string ID);
    }
}
