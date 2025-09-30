using Microsoft.EntityFrameworkCore;
using PortalInmobiliario.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PortalInmobiliario.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Verificar si ya hay datos
                if (context.Inmuebles.Any())
                {
                    return; // La BD ya tiene datos
                }

                // Agregar inmuebles de ejemplo
                context.Inmuebles.AddRange(
                    new Inmueble
                    {
                        Codigo = "DEP-001",
                        Titulo = "Departamento Moderno en Centro",
                        Imagen = "/img/departamento1.jpg",
                        Tipo = "Departamento",
                        Ciudad = "Lima",
                        Direccion = "Av. Arequipa 123",
                        Dormitorios = 2,
                        Banos = 2,
                        MetrosCuadrados = 80,
                        Precio = 150000,
                        Activo = true
                    },
                    new Inmueble
                    {
                        Codigo = "CASA-001",
                        Titulo = "Casa Familiar en Surco",
                        Imagen = "/img/casa1.jpg",
                        Tipo = "Casa",
                        Ciudad = "Lima",
                        Direccion = "Calle Los Pinos 456",
                        Dormitorios = 4,
                        Banos = 3,
                        MetrosCuadrados = 200,
                        Precio = 450000,
                        Activo = true
                    },
                    new Inmueble
                    {
                        Codigo = "OFI-001",
                        Titulo = "Oficina en Centro Empresarial",
                        Imagen = "/img/oficina1.jpg",
                        Tipo = "Oficina",
                        Ciudad = "Lima",
                        Direccion = "Av. Javier Prado 789",
                        Dormitorios = 0,
                        Banos = 2,
                        MetrosCuadrados = 60,
                        Precio = 120000,
                        Activo = true
                    }
                );

                context.SaveChanges();
            }
        }
    }
}