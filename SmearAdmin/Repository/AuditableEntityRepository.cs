using SmearAdmin.Data;
using SmearAdmin.Interface;
using SmearAdmin.Persistence;

namespace SmearAdmin.Repository
{
    public class AuditableEntityRepository : Repositories<AuditableEntity>, IAuditableEntityRepository
    {
        public AuditableEntityRepository(SmearAdminDbContext context) : base(context) { }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;
    }
}
