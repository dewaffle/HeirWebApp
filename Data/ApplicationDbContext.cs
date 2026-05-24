using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HeirWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<HeirWebApp.Models.Member> Member { get; set; } = default!;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
