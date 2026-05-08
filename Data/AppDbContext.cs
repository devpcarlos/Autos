using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        // Recibe la configuración por inyección de dependencias desde Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Representa la tabla "Autos" en SQL Server
        // EF Core creará esta tabla cuando ejecutes la migración
        public DbSet<Auto> Autos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly
                );
        }

    }
}
