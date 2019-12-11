using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmearAdmin.Models;

namespace SmearAdmin.Data
{
    public partial class SmearAdminDbContext : IdentityDbContext<AppUser>
    {
        public SmearAdminDbContext(DbContextOptions<SmearAdminDbContext> options) : base(options) { }

        //public virtual DbSet<AppUser> AppUsers { get; set; }

    }
}
