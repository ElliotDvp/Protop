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
    public class VisitasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public VisitasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Visitas/Create
        public async Task<IActionResult> Create(int inmuebleId)
        {
            var inmueble = await _context.Inmuebles
                .FirstOrDefaultAsync(i => i.Id == inmuebleId && i.Activo);

            if (inmueble == null)
            {
                return NotFound();
            }

            var model = new AgendarVisitaViewModel
            {
                InmuebleId = inmuebleId,
                InmuebleTitulo = inmueble.Titulo,
                FechaInicio = DateTime.Now.AddDays(1).Date.AddHours(10), // Mañana 10 AM
                FechaFin = DateTime.Now.AddDays(1).Date.AddHours(11)     // Mañana 11 AM
            };

            return View(model);
        }

        // POST: Visitas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AgendarVisitaViewModel model)
        {
            var inmueble = await _context.Inmuebles
                .FirstOrDefaultAsync(i => i.Id == model.InmuebleId && i.Activo);

            if (inmueble == null)
            {
                ModelState.AddModelError("", "Inmueble no encontrado");
                return View(model);
            }

            // Validación 1: FechaInicio < FechaFin
            if (model.FechaInicio >= model.FechaFin)
            {
                ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la fecha de inicio");
            }

            // Validación 2: Horario laboral (08:00 - 19:00)
            if (model.FechaInicio.Hour < 8 || model.FechaFin.Hour > 19)
            {
                ModelState.AddModelError("FechaInicio", "Las visitas solo pueden agendarse entre 8:00 AM y 7:00 PM");
            }

            // Validación 3: No visitas solapadas
            var existeSolapamiento = await _context.Visitas
                .AnyAsync(v => v.InmuebleId == model.InmuebleId 
                            && v.Estado != "Cancelada"
                            && v.FechaInicio < model.FechaFin 
                            && v.FechaFin > model.FechaInicio);

            if (existeSolapamiento)
            {
                ModelState.AddModelError("FechaInicio", "Ya existe una visita agendada para este inmueble en el horario seleccionado");
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                var visita = new Visita
                {
                    InmuebleId = model.InmuebleId,
                    UsuarioId = userId!,
                    FechaInicio = model.FechaInicio,
                    FechaFin = model.FechaFin,
                    Notas = model.Notas,
                    Estado = "Solicitada"
                };

                _context.Visitas.Add(visita);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Visita agendada correctamente. Espera la confirmación del broker.";
                return RedirectToAction("Details", "Inmuebles", new { id = model.InmuebleId });
            }

            model.InmuebleTitulo = inmueble.Titulo;
            return View(model);
        }
    }
}