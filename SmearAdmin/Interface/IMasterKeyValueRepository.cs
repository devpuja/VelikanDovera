using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System.Threading.Tasks;

namespace SmearAdmin.Interface
{
    public interface IMasterKeyValueRepository : IRepositories<MasterKeyValue>
    {
        Task<PagingResult<MasterKeyValue>> GetAllMastersAsync(string masterFor, int pageIndex, int pageSize);
        //Task<(List<MasterKeyValue> dep, List<MasterKeyValue> des, List<MasterKeyValue> grade, List<MasterKeyValue> region)> GetMastersForEmployeeAsync();
        Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForEmployeeAsync();
        Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForDoctorAsync();
        Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForChemistAsync();
        Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForStockistAsync();
        Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForHolidayAsync();
        
    }
}
