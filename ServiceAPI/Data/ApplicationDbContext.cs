using Microsoft.EntityFrameworkCore;

namespace ServiceAPI
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Service> Messages { get; set; } = null!;

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //    : base(options) { }

        public ApplicationDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=D:\Files\ServiceAPI\ServiceAPI\Data\Service.db");
        }
    }
}
