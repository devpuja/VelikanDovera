using Microsoft.AspNetCore.Hosting;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System.Threading.Tasks;

namespace SmearAdmin.Interface
{
    public interface IChemistRepository : IRepositories<Chemist>
    {
        Task<PagingResult<ChemistViewModel>> GetAllChemistAsync(int pageIndex, int pageSize);
        Task<PagingResult<ChemistViewModel>> GetAllChemistByUserAsync(int pageIndex, int pageSize, string userName);
        Task<PagingResult<ChemistViewModel>> GetAllChemistBySearchAsync(int pageIndex, int pageSize, string searchValue);        
        Task<ChemistViewModel> GetChemistByIDAsync(string ID);
        Task<ExportExcel> ExportChemistsDataAsync(IHostingEnvironment _host);
        Task<ExportExcel> ExportChemistsDataByUserAsync(IHostingEnvironment _host, string userName);
    }
}
