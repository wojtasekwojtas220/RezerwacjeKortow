using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace RezerwacjeKortow.Models
{
    public class RezerwacjaKortu
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Wybierz miasto")]
        [Display(Name = "Miasto")]
        public string Miasto { get; set; } = string.Empty;

        public DateTime DataRozpoczecia { get; set; }

        [Required]
        [Range(1, 4)]
        [Display(Name = "Czas trwania (w godzinach)")]
        public int CzasTrwania { get; set; }

        public StatusRezerwacji Status { get; set; } = StatusRezerwacji.Oczekujaca;

        public int KortId { get; set; }
        public Kort? Kort { get; set; }

        public string? UzytkownikId { get; set; }
        public IdentityUser? Uzytkownik { get; set; }
        
        [NotMapped]
        [Required(ErrorMessage = "Wybierz datę gry")]
        [DataType(DataType.Date)]
        [Display(Name = "Data gry")]
        public DateTime DataGry { get; set; } = DateTime.Today;

        [NotMapped]
        [Required(ErrorMessage = "Wybierz godzinę gry")]
        [Display(Name = "Godzina gry")]
        public string GodzinaGry { get; set; } = "17:00";
    }
}