using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalInmobiliario.Models;
using PortalInmobiliario.Data;

namespace PortalInmobiliario.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InmueblesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inmuebles
        public async Task<IActionResult> Index(InmuebleFilterViewModel? filtros = null)
        {
            // Inicializar filtros si es null
            filtros ??= new InmuebleFilterViewModel();
            
            // Consulta base - solo inmuebles activos
            IQueryable<Inmueble> query = _context.Inmuebles.Where(i => i.Activo);

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filtros.Ciudad))
                query = query.Where(i => i.Ciudad != null && i.Ciudad.Contains(filtros.Ciudad));

            if (!string.IsNullOrEmpty(filtros.Tipo))
                query = query.Where(i => i.Tipo != null && i.Tipo == filtros.Tipo);

            if (filtros.PrecioMin.HasValue)
                query = query.Where(i => i.Precio >= filtros.PrecioMin.Value);

            if (filtros.PrecioMax.HasValue)
                query = query.Where(i => i.Precio <= filtros.PrecioMax.Value);

            if (filtros.Dormitorios.HasValue)
                query = query.Where(i => i.Dormitorios >= filtros.Dormitorios.Value);

            // Validaciones server-side
            if (filtros.PrecioMin.HasValue && filtros.PrecioMax.HasValue &&
                filtros.PrecioMin > filtros.PrecioMax)
            {
                ModelState.AddModelError("PrecioMax", "El precio máximo debe ser mayor o igual al precio mínimo");
            }

            // Contar total antes de paginar
            filtros.TotalInmuebles = await query.CountAsync();

            // **SOLUCIÓN: Obtener datos SIN ORDER BY en la consulta SQL**
            var inmueblesSinOrdenar = await query
                .Skip((filtros.Pagina - 1) * filtros.TamanoPagina)
                .Take(filtros.TamanoPagina)
                .ToListAsync();  // ← Los datos vienen de la BD sin ordenar

            // **ORDENAR EN MEMORIA (client-side) después de obtener los datos**
            var inmuebles = inmueblesSinOrdenar
                .OrderBy(i => i.Precio)
                .ToList();

            filtros.Inmuebles = inmuebles;

            // Llenar listas para dropdowns
            filtros.Ciudades = await _context.Inmuebles
                .Where(i => i.Activo && i.Ciudad != null)
                .Select(i => i.Ciudad!)
                .Distinct()
                .ToListAsync();

            filtros.Tipos = await _context.Inmuebles
                .Where(i => i.Activo && i.Tipo != null)
                .Select(i => i.Tipo!)
                .Distinct()
                .ToListAsync();

            return View(filtros);
        }

        // GET: Inmuebles/Details/5
        public async Task<IActionResult> Details(int? id)
        {   
            if (id == null)
            {
                return NotFound();
        }

        var inmueble = await _context.Inmuebles
        .FirstOrDefaultAsync(m => m.Id == id && m.Activo);

        if (inmueble == null)
            {
                return NotFound();
            }

    // Verificar si tiene reserva activa
        var tieneReservaActiva = await _context.Reservas
        .AnyAsync(r => r.InmuebleId == id && r.FechaExpiracion > DateTime.Now);

        ViewBag.TieneReservaActiva = tieneReservaActiva;

        return View(inmueble);
        }
    }
}