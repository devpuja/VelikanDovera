using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmearAdmin.Data;
using SmearAdmin.Interface;
using SmearAdmin.Repository;

namespace SmearAdmin.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmearAdminDbContext _appDbContext;

        ILoggerFactory _loggerFactory;
        IMasterKeyValueRepository _MasterKeyValues;
        ICommonResourseRepository _CommonResourse;
        IAuditableEntityRepository _AuditableEntity;
        IEmployeeRepository _Employee;
        IContactResourseRepository _ContactResource;
        IHQRegionRepository _HQRegion;
        IEmployeeExpensesRepository _EmployeeExpenses;
        IEmployeeExpensesStatusRepository _EmployeeExpensesStatus;
        IAdminDashboardRepository _AdminDashboard;
        IDoctorRepository _Doctors;
        IChemistRepository _Chemist;
        IStockistRepository _Stockist;
        IHolidayRepository _Holiday;
        ISendSMSRepository _SendSMS;

        public UnitOfWork(SmearAdminDbContext context, ILoggerFactory loggerFactory)
        {
            _appDbContext = context;
            _loggerFactory = loggerFactory;
        }

        public IMasterKeyValueRepository MasterKeyValues
        {
            get
            {
                if (_MasterKeyValues == null)
                    _MasterKeyValues = new MasterKeyValueRepository(_appDbContext);

                return _MasterKeyValues;
            }
        }

        public ICommonResourseRepository CommonResourse
        {
            get
            {
                if (_CommonResourse == null)
                    _CommonResourse = new CommonResourseRepository (_appDbContext);

                return _CommonResourse;
            }
        }

        public IAuditableEntityRepository AuditableEntity
        {
            get
            {
                if (_AuditableEntity == null)
                    _AuditableEntity = new AuditableEntityRepository(_appDbContext);

                return _AuditableEntity;
            }
        }

        public IEmployeeRepository Employee
        {
            get
            {
                if (_Employee == null)
                    _Employee = new EmployeeRepository(_appDbContext);

                return _Employee;
            }
        }

        public IContactResourseRepository ContactResource
        {
            get
            {
                if (_ContactResource == null)
                    _ContactResource = new ContactResourseRepository(_appDbContext);

                return _ContactResource;
            }
        }

        public IHQRegionRepository HQRegion
        {
            get
            {
                if (_HQRegion == null)
                    _HQRegion = new HQRegionRepository(_appDbContext);

                return _HQRegion;
            }
        }

        public IEmployeeExpensesRepository EmployeeExpenses
        {
            get
            {
                if (_EmployeeExpenses == null)
                    _EmployeeExpenses = new EmployeeExpensesRepository(_appDbContext, _loggerFactory);

                return _EmployeeExpenses;
            }
        }

        public IEmployeeExpensesStatusRepository EmployeeExpensesStatus
        {
            get
            {
                if (_EmployeeExpensesStatus == null)
                    _EmployeeExpensesStatus = new EmployeeExpensesStatusRepository(_appDbContext);

                return _EmployeeExpensesStatus;
            }
        }

        public IAdminDashboardRepository AdminDashboard
        {
            get
            {
                if (_AdminDashboard == null)
                    _AdminDashboard = new AdminDashboardRepository(_appDbContext);

                return _AdminDashboard;
            }
        }

        public IDoctorRepository Doctors
        {
            get
            {
                if (_Doctors == null)
                    _Doctors = new DoctorRepository(_appDbContext);

                return _Doctors;
            }
        }

        public IChemistRepository Chemist
        {
            get
            {
                if (_Chemist == null)
                    _Chemist = new ChemistRepository(_appDbContext);

                return _Chemist;
            }
        }

        public IStockistRepository Stockist
        {
            get
            {
                if (_Stockist == null)
                    _Stockist = new StockistRepository(_appDbContext);

                return _Stockist;
            }
        }

        public IHolidayRepository Holiday
        {
            get
            {
                if (_Holiday == null)
                    _Holiday = new HolidayRepository(_appDbContext);

                return _Holiday;
            }
        }

        public ISendSMSRepository SendSMS
        {
            get
            {
                if (_SendSMS == null)
                    _SendSMS = new SendSMSRepository(_appDbContext);

                return _SendSMS;
            }
        }

        public async Task CompleteAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
