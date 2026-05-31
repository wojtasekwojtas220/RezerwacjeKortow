using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RezerwacjeKortow.Data;
using RezerwacjeKortow.Models;

namespace RezerwacjeKortow.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ZarzadzanieRezerwacjamiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZarzadzanieRezerwacjamiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rezerwacje = await _context.Rezerwacje
                .Include(r => r.Kort)
                .Include(r => r.Uzytkownik)
                .OrderByDescending(r => r.DataRozpoczecia)
                .ToListAsync();

            ViewBag.Oczekujace = rezerwacje.Count(r => r.Status == StatusRezerwacji.Oczekujaca);
            ViewBag.Zatwierdzone = rezerwacje.Count(r => r.Status == StatusRezerwacji.Zatwierdzona);
            ViewBag.Dzisiejsze = rezerwacje.Count(r => r.DataRozpoczecia.Date == DateTime.Today);

            return View(rezerwacje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ZmienStatus(int id, StatusRezerwacji nowyStatus)
        {
            var rezerwacja = await _context.Rezerwacje.FindAsync(id);
            if (rezerwacja != null)
            {
                rezerwacja.Status = nowyStatus;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}