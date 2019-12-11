using SmearAdmin.Data;
using SmearAdmin.Interface;
using SmearAdmin.Persistence;

namespace SmearAdmin.Repository
{
    public class CommonResourseRepository : Repositories<ChemistStockistResourse>, ICommonResourseRepository
    {
        public CommonResourseRepository(SmearAdminDbContext context) : base(context) { }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;
    }
}
