using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Models;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;

namespace SmearAdmin.Interface
{
    public interface IEmployeeRepository : IRepositories<AppUser>
    {
        Task<PagingResult<RegistrationViewModel>> GetAllEmployeeAsync(int pageIndex, int pageSize);
        //Task<IEnumerable<RegistrationViewModel>> GetEmployeeByIDAsync(string ID);
        Task<(RegistrationViewModel, string, string, string, string, string, string)> GetEmployeeByIDAsync(string ID);

        Task<List<SwapEmployeeViewModel>> GetSwapEmployeeListAsync();
        Task<string> SwapEmployeeUserNamesAsync(string userNameFrom, string userNameTo);
    }
}
