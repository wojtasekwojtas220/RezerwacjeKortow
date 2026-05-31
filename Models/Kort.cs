using System.ComponentModel.DataAnnotations;

namespace RezerwacjeKortow.Models
{
    public class Kort
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa kortu jest wymagana")]
        [Display(Name = "Nazwa kortu")]
        public string Nazwa { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Opis nawierzchni (np. Mączka, Trawiasty)")]
        public string Opis { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Cena za godzinę (PLN)")]
        public decimal CenaZaGodzine { get; set; }

        // Relacja: Jeden kort może mieć wiele rezerwacji
        public List<RezerwacjaKortu> Rezerwacje { get; set; } = new List<RezerwacjaKortu>();
    }
}