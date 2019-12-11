using SmearAdmin.Interface;
using System;
using System.Threading.Tasks;

namespace SmearAdmin.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IMasterKeyValueRepository MasterKeyValues { get; }
        ICommonResourseRepository CommonResourse { get; }
        IAuditableEntityRepository AuditableEntity { get; }
        IEmployeeRepository Employee { get; }
        IContactResourseRepository ContactResource { get; }        
        IHQRegionRepository HQRegion { get; }
        IEmployeeExpensesRepository EmployeeExpenses { get; }
        IEmployeeExpensesStatusRepository EmployeeExpensesStatus { get; }
        IAdminDashboardRepository AdminDashboard { get; }
        IDoctorRepository Doctors { get; }
        IChemistRepository Chemist { get; }
        IStockistRepository Stockist { get; }
        IHolidayRepository Holiday { get; }
        ISendSMSRepository SendSMS { get; }
        Task CompleteAsync();
    }
}
