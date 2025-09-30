using Microsoft.EntityFrameworkCore;
using PortalInmobiliario.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PortalInmobiliario.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets para nuestros modelos
        public DbSet<Inmueble> Inmuebles { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configurar Inmueble
            builder.Entity<Inmueble>(entity =>
            {
                // Código único
                entity.HasIndex(i => i.Codigo).IsUnique();
                
                // Precio con precisión decimal
                entity.Property(i => i.Precio)
                      .HasPrecision(18, 2);
                
                // Metros cuadrados con precisión
                entity.Property(i => i.MetrosCuadrados)
                      .HasPrecision(18, 2);
            });

            // Configurar Visita
            builder.Entity<Visita>(entity =>
            {
                // FechaInicio debe ser menor que FechaFin
                entity.ToTable(tb => tb.HasCheckConstraint("CK_Visita_Fechas", "[FechaInicio] < [FechaFin]"));
            });

            // No se pueden crear restricciones directas para "no visitas solapadas" 
            // ni "solo una reserva activa" en SQLite, se validarán por código
        }
    }
}
