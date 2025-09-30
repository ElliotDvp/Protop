using System.ComponentModel.DataAnnotations;

namespace PortalInmobiliario.Models
{
    public class Inmueble
    {
        public int Id { get; set; }
        
        [Required, MaxLength(20)]
        public string Codigo { get; set; } = string.Empty;  // ← Inicializar
        
        [Required, MaxLength(100)]
        public string Titulo { get; set; } = string.Empty;
        
        public string? Imagen { get; set; }  // ← Esta sí puede ser null
        
        [Required]
        public string Tipo { get; set; } = string.Empty;  // ← Inicializar
        
        [Required, MaxLength(50)]
        public string Ciudad { get; set; } = string.Empty;  // ← Inicializar
        
        [Required, MaxLength(200)]
        public string Direccion { get; set; } = string.Empty;  // ← Inicializar
        
        [Range(0, 10)]
        public int Dormitorios { get; set; }
        
        [Range(0, 10)]
        public int Banos { get; set; }
        
        [Range(0.1, double.MaxValue)]
        public decimal MetrosCuadrados { get; set; }
        
        [Range(0.01, double.MaxValue)]
        public decimal Precio { get; set; }
        
        public bool Activo { get; set; } = true;

        // Relaciones
        public ICollection<Visita> Visitas { get; set; } = new List<Visita>();
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}