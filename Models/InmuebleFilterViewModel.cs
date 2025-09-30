namespace PortalInmobiliario.Models
{
    public class InmuebleFilterViewModel
    {
        public string? Ciudad { get; set; }
        public string? Tipo { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public int? Dormitorios { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanoPagina { get; set; } = 6;
        
        // Para mostrar los resultados
        public List<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();
        public int TotalInmuebles { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalInmuebles / TamanoPagina);
        
        // Listas para los dropdowns - CON VERIFICACIÓN
        public List<string> Ciudades { get; set; } = new List<string>();
        public List<string> Tipos { get; set; } = new List<string>();
    }
}