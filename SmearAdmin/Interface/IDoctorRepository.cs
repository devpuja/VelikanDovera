using Microsoft.AspNetCore.Hosting;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmearAdmin.Interface
{
    public interface IDoctorRepository : IRepositories<Doctor>
    {
        Task<PagingResult<DoctorViewModel>> GetAllDoctorAsync(int pageIndex, int pageSize);
        Task<PagingResult<DoctorViewModel>> GetAllDoctorsByUserAsync(int pageIndex, int pageSize, string userName);
        Task<PagingResult<DoctorViewModel>> GetAllDoctorsBySearchAsync(int pageIndex, int pageSize, string searchValue);        
        Task<DoctorViewModel> GetDoctorByIDAsync(string ID);
        Task<ExportExcel> ExportDoctorsDataAsync(IHostingEnvironment _host);
        Task<ExportExcel> ExportDoctorsDataByUserAsync(IHostingEnvironment _host, string userName);
    }
}