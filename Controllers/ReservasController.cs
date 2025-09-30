using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortalInmobiliario.Models;
using PortalInmobiliario.Data;
using System.Security.Claims;

namespace PortalInmobiliario.Controllers
{
    [Authorize] // Solo usuarios autenticados
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int inmuebleId)
        {
            var inmueble = await _context.Inmuebles
                .FirstOrDefaultAsync(i => i.Id == inmuebleId && i.Activo);

            if (inmueble == null)
            {
                return NotFound();
            }

            // Validación: No reserva activa para el inmueble
            var reservaActiva = await _context.Reservas
                .AnyAsync(r => r.InmuebleId == inmuebleId && r.FechaExpiracion > DateTime.Now);

            if (reservaActiva)
            {
                TempData["ErrorMessage"] = "El inmueble ya tiene una reserva activa";
                return RedirectToAction("Details", "Inmuebles", new { id = inmuebleId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var reserva = new Reserva
            {
                InmuebleId = inmuebleId,
                UsuarioId = userId!,
                FechaCreacion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddHours(48) // Reserva por 48 horas
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Inmueble reservado por 48 horas. Tienes hasta " + reserva.FechaExpiracion.ToString("f") + " para completar la documentación.";
            return RedirectToAction("Details", "Inmuebles", new { id = inmuebleId });
        }
    }
}