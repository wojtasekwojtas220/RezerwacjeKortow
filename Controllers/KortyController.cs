using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RezerwacjeKortow.Data;
using RezerwacjeKortow.Models;

namespace RezerwacjeKortow.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KortyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KortyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Korty.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kort kort)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kort);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kort);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var kort = await _context.Korty.FindAsync(id);
            if (kort == null) return NotFound();
            return View(kort);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kort kort)
        {
            if (id != kort.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(kort);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kort);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var kort = await _context.Korty.FirstOrDefaultAsync(m => m.Id == id);
            if (kort == null) return NotFound();
            return View(kort);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kort = await _context.Korty.FindAsync(id);
            if (kort != null)
            {
                _context.Korty.Remove(kort);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}