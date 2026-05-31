using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RezerwacjeKortow.Data;
using System.Diagnostics;
using RezerwacjeKortow.Models;

namespace RezerwacjeKortow.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Korty.ToListAsync());
        }
      public async Task<IActionResult> OpisKortow()
        {
            return View(await _context.Korty.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}