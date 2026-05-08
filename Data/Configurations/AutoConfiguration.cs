using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.Data.Configurations
{
    public class AutoConfiguration : IEntityTypeConfiguration<Auto>
    {
            public void Configure(EntityTypeBuilder<Auto> builder)
            {
                // Configuración de la tabla
                builder.ToTable("Autos");
                // Configuración de la clave primaria
                builder.HasKey(a => a.Id);
                // Configuración de las propiedades
                builder.Property(a => a.Marca)
                    .IsRequired()
                    .HasMaxLength(16);
                builder.Property(a => a.Modelo)
                    .IsRequired()
                    .HasMaxLength(10);
                builder.Property(a => a.Año)
                    .IsRequired();
                builder.Property(a => a.Precio)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
            // Configuración de los campos internos
            builder.Property(a => a.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");
                builder.Property(a => a.Activo)
                    .HasDefaultValue(true);
            // LLAVE FORÁNEA — referencia al Cliente
            builder.Property(a => a.ClienteId)
                   .IsRequired();
        }
        }
    }

