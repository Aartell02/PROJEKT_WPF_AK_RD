using Microsoft.EntityFrameworkCore;
using PROJEKT_WPF_AK_RD.EntityFramework;

namespace PROJEKT_WPF_AK_RD.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ProjektWPF;Trusted_Connection=True;");
        }
    }
}
