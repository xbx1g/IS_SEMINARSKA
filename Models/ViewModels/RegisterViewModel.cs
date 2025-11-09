using System.ComponentModel.DataAnnotations;

namespace AutoServis.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ime je obvezno")]
        [Display(Name = "Ime")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Priimek je obvezen")]
        [Display(Name = "Priimek")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email je obvezen")]
        [EmailAddress(ErrorMessage = "Neveljaven email naslov")]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Izberite vlogo")]
        [Display(Name = "Vloga")]
        public required string Vloga { get; set; }

        [Required(ErrorMessage = "Geslo je obvezno")]
        [StringLength(100, ErrorMessage = "Geslo mora biti dolgo vsaj {2} znakov.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Geslo")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potrdi geslo")]
        [Compare("Password", ErrorMessage = "Gesli se ne ujemata.")]
        public required string ConfirmPassword { get; set; }
    }
}