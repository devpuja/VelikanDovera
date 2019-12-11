using Microsoft.AspNetCore.Hosting;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System.Threading.Tasks;

namespace SmearAdmin.Interface
{
    public interface IStockistRepository : IRepositories<Stockist>
    {
        Task<PagingResult<StockistViewModel>> GetAllStockistAsync(int pageIndex, int pageSize);
        Task<PagingResult<StockistViewModel>> GetAllStockistByUserAsync(int pageIndex, int pageSize, string userName);
        Task<StockistViewModel> GetStockistByIDAsync(string ID);
        Task<ExportExcel> ExportStockistsDataAsync(IHostingEnvironment _host);
        Task<ExportExcel> ExportStockistsDataByUserAsync(IHostingEnvironment _host, string userName);
    }
}
