using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System.Threading.Tasks;

namespace SmearAdmin.Interface
{
    public interface ISendSMSRepository : IRepositories<Smslogger>
    {
        Task<PagingResult<DoctorViewModel>> GetAllDoctorSendSMSAsync(int pageIndex, int pageSize);
        Task<PagingResult<DoctorViewModel>> GetAllDoctorSendSMSByUserAsync(int pageIndex, int pageSize, string userName);
        Task<PagingResult<DoctorViewModel>> GetAllDoctorSendSMSBySearchAsync(int pageIndex, int pageSize, string searchValue);
        Task<AdminDashboardViewModelDTO> GetSendSMSCountAsync();
    }
}
