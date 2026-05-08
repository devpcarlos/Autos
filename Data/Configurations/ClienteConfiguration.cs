using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.Data.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            // Nombre de la tabla
            builder.ToTable("Clientes");

            // PRIMARY KEY
            builder.HasKey(c => c.Id);

            // Nombre — NOT NULL, máximo 100 caracteres
            builder.Property(c => c.Nombre)
                   .IsRequired()
                   .HasMaxLength(100);

            // Email — NOT NULL, máximo 100, UNIQUE
            builder.Property(c => c.Email)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasIndex(c => c.Email)
                   .IsUnique();

            // Contraseña — NOT NULL, máximo 256 caracteres
            // En producción siempre se guarda hasheada, nunca en texto plano
            builder.Property(c => c.Contrasena)
                   .IsRequired()
                   .HasMaxLength(256);

            // Teléfono — opcional, máximo 15 caracteres
            builder.Property(c => c.Telefono)
                   .HasMaxLength(15);

            // Activo — true por defecto
            builder.Property(c => c.Activo)
                   .HasDefaultValue(true);

            // RELACIÓN — un Cliente tiene muchos Autos
            // Si eliminas un Cliente, restringe — no borra los Autos
            builder.HasMany(c => c.Autos)
                   .WithOne(a => a.Cliente)
                   .HasForeignKey(a => a.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
