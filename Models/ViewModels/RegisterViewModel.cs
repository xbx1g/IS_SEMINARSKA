using System.ComponentModel.DataAnnotations;

namespace AutoServis.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Ime")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Priimek")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mesto")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Vloga")]
        public string Vloga { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Geslo mora biti dolgo vsaj {2} znakov.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Geslo")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potrdi geslo")]
        [Compare("Password", ErrorMessage = "Gesli se ne ujemata.")]
        public string ConfirmPassword { get; set; }
    }
}