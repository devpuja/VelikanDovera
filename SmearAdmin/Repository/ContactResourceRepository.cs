using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmearAdmin.Data;
using SmearAdmin.Interface;
using SmearAdmin.Persistence;

namespace SmearAdmin.Repository
{
    public class ContactResourseRepository : Repositories<ContactResourse>, IContactResourseRepository
    {
        public ContactResourseRepository(SmearAdminDbContext context) : base(context) { }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;
    }
}
