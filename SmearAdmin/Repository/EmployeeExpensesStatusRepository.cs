using SmearAdmin.Data;
using SmearAdmin.Interface;
using SmearAdmin.Persistence;

namespace SmearAdmin.Repository
{
    public class EmployeeExpensesStatusRepository : Repositories<ExpensesStatus>, IEmployeeExpensesStatusRepository
    {
        public EmployeeExpensesStatusRepository(SmearAdminDbContext context) : base(context) { }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;
    }
}
