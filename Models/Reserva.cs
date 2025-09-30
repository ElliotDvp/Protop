using Microsoft.AspNetCore.Identity;

namespace PortalInmobiliario.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        
        public int InmuebleId { get; set; }
        
        public string? UsuarioId { get; set; }
        
        public DateTime FechaExpiracion { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relaciones CORREGIDAS
        public Inmueble? Inmueble { get; set; }  // ← "Inmueble" correcto
        public IdentityUser? Usuario { get; set; }  // ← "IdentityUser"
    }
}