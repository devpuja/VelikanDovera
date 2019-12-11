using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System.Threading.Tasks;

namespace SmearAdmin.Interface
{
    public interface ISendSMSRepository: IRepositories<Smslogger>
    {
        Task<PagingResult<DoctorViewModel>> GetAllDoctorSendSMSAsync(int pageIndex, int pageSize);
        Task<AdminDashboardViewModelDTO> GetSendSMSCountAsync();
    }
}
