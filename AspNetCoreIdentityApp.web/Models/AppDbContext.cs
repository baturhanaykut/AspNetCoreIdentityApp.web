using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AspNetCoreIdentityApp.web.Areas.Admin.Models;

namespace AspNetCoreIdentityApp.web.Models
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AspNetCoreIdentityApp.web.Areas.Admin.Models.RoleUpdateViewModel> RoleUpdateViewModel { get; set; } = default!;
    }
}
