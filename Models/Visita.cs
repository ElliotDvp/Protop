using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PortalInmobiliario.Models
{
    public class Visita
    {
        public int Id { get; set; }
        
        [Required]
        public int InmuebleId { get; set; }  // ← "InmuebleId" correcto
        
        [Required]
        public string? UsuarioId { get; set; }
        
        [Required]
        public DateTime FechaInicio { get; set; }
        
        [Required]
        public DateTime FechaFin { get; set; }
        
        [Required]
        public string Estado { get; set; } = "Solicitada";
        
        [MaxLength(500)]
        public string? Notas { get; set; }

        // Relaciones CORREGIDAS
        public Inmueble? Inmueble { get; set; }  // ← "Inmueble" no "Immueble"
        public IdentityUser? Usuario { get; set; }  // ← "IdentityUser" en lugar de "ApplicationUser"
    }
}