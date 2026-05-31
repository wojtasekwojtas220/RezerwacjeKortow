using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RezerwacjeKortow.Data;
using RezerwacjeKortow.Models;

namespace RezerwacjeKortow.Controllers
{
    [Authorize]
    public class RezerwacjeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RezerwacjeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var rezerwacje = await _context.Rezerwacje
                .Include(r => r.Kort)
                .Where(r => r.UzytkownikId == userId)
                .OrderByDescending(r => r.DataRozpoczecia)
                .ToListAsync();
                
            return View(rezerwacje);
        }

        public async Task<IActionResult> Create(int? kortId)
        {
            if (kortId == null) return NotFound();
            var kort = await _context.Korty.FindAsync(kortId);
            if (kort == null) return NotFound();

            ViewBag.KortNazwa = kort.Nazwa;
            var rezerwacja = new RezerwacjaKortu 
            { 
                KortId = kort.Id,
                DataGry = DateTime.Today
            };
            return View(rezerwacja);
        }

        [HttpGet]
        public async Task<IActionResult> GetDostepneGodziny(int kortId, string dataGry, int czasTrwania, string miasto)
        {
            if (!DateTime.TryParse(dataGry, out DateTime wybranaData) || string.IsNullOrEmpty(miasto))
                return Json(new List<string>());

            var rezerwacjeDnia = await _context.Rezerwacje
                .Where(r => r.KortId == kortId && 
                            r.Miasto.ToLower() == miasto.ToLower() && 
                            r.DataRozpoczecia.Date == wybranaData.Date && 
                            r.Status != StatusRezerwacji.Odrzucona && 
                            r.Status != StatusRezerwacji.Anulowana)
                .ToListAsync();

            var dostepneGodziny = new List<string>();
            
            for (int h = 6; h <= 20; h++)
            {
                var minuty = (h == 20) ? new[] { "00" } : new[] { "00", "30" };

                foreach (var m in minuty)
                {
                    var czasRozpoczecia = wybranaData.Date.Add(new TimeSpan(h, int.Parse(m), 0));
                    
                    if (czasRozpoczecia <= DateTime.Now) continue;
                    var czasZakonczenia = czasRozpoczecia.AddHours(czasTrwania);

                    bool isConflict = rezerwacjeDnia.Any(r => 
                        r.DataRozpoczecia < czasZakonczenia && 
                        r.DataRozpoczecia.AddHours(r.CzasTrwania) > czasRozpoczecia);

                    if (!isConflict)
                    {
                        dostepneGodziny.Add($"{h:00}:{m}");
                    }
                }
            }
            return Json(dostepneGodziny);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RezerwacjaKortu rezerwacja)
        {
            if (TimeSpan.TryParse(rezerwacja.GodzinaGry, out TimeSpan godzina))
            {
                rezerwacja.DataRozpoczecia = rezerwacja.DataGry.Date + godzina;
            }

            if (rezerwacja.DataRozpoczecia < DateTime.Now)
            {
                ModelState.AddModelError("DataGry", "Nie możesz zarezerwować terminu w przeszłości.");
            }

            var koniecNowej = rezerwacja.DataRozpoczecia.AddHours(rezerwacja.CzasTrwania);
            bool isConflict = await _context.Rezerwacje.AnyAsync(r => 
                r.KortId == rezerwacja.KortId && 
                r.Miasto == rezerwacja.Miasto &&
                r.Status != StatusRezerwacji.Odrzucona && 
                r.Status != StatusRezerwacji.Anulowana &&
                r.DataRozpoczecia < koniecNowej && 
                r.DataRozpoczecia.AddHours(r.CzasTrwania) > rezerwacja.DataRozpoczecia);

            if (isConflict)
            {
                ModelState.AddModelError("", "Ten kort jest już zajęty w wybranym mieście i czasie");
            }

            rezerwacja.UzytkownikId = _userManager.GetUserId(User);
            rezerwacja.Status = StatusRezerwacji.Oczekujaca;

            if (ModelState.IsValid)
            {
                _context.Add(rezerwacja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var kort = await _context.Korty.FindAsync(rezerwacja.KortId);
            ViewBag.KortNazwa = kort?.Nazwa;
            return View(rezerwacja);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anuluj(int id)
        {
            var userId = _userManager.GetUserId(User);
            var rezerwacja = await _context.Rezerwacje
                .FirstOrDefaultAsync(r => r.Id == id && r.UzytkownikId == userId);

            if (rezerwacja != null && rezerwacja.DataRozpoczecia > DateTime.Now)
            {
                rezerwacja.Status = StatusRezerwacji.Anulowana;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}