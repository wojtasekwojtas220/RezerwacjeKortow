using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RezerwacjeKortow.Models;

namespace RezerwacjeKortow.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kort> Korty { get; set; }
        public DbSet<RezerwacjaKortu> Rezerwacje { get; set; }
    }
}