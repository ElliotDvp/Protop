using System.ComponentModel.DataAnnotations;

namespace PortalInmobiliario.Models
{
    public class AgendarVisitaViewModel
    {
        public int InmuebleId { get; set; }
        public string? InmuebleTitulo { get; set; }
        
        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        [Display(Name = "Fecha y Hora de Inicio")]
        public DateTime FechaInicio { get; set; } = DateTime.Now.AddDays(1).Date.AddHours(10); // Mañana a las 10 AM
        
        [Required(ErrorMessage = "La fecha de fin es requerida")]
        [Display(Name = "Fecha y Hora de Fin")]
        public DateTime FechaFin { get; set; } = DateTime.Now.AddDays(1).Date.AddHours(11); // Mañana a las 11 AM
        
        [Display(Name = "Notas adicionales")]
        [MaxLength(500)]
        public string? Notas { get; set; }
    }
}