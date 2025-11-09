#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using AutoServis.Models;
using AutoServis.Data;

namespace AutoServis.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly AutoServisContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            AutoServisContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Ime je obvezno")]
            [Display(Name = "Ime")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Priimek je obvezen")]
            [Display(Name = "Priimek")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Email je obvezen")]
            [EmailAddress(ErrorMessage = "Neveljaven email naslov")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Izberite vlogo")]
            [Display(Name = "Vloga")]
            public string Vloga { get; set; }

            [Required(ErrorMessage = "Geslo je obvezno")]
            [StringLength(100, ErrorMessage = "Geslo mora biti dolgo vsaj {2} znakov.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Geslo")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potrdi geslo")]
            [Compare("Password", ErrorMessage = "Gesli se ne ujemata.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.Vloga = Input.Vloga;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Uporabnik je ustvaril nov račun.");

                    // Ustvarjanje ustreznega profila glede na vlogo
                    if (Input.Vloga == "Stranka")
                    {
                        var stranka = new Stranka
                        {
                            Ime = Input.FirstName,
                            Priimek = Input.LastName,
                            Email = Input.Email,
                            DatumRegistracije = DateTime.Now
                        };
                        
                        _context.Stranke.Add(stranka);
                        await _context.SaveChangesAsync();
                        
                        user.StrankaID = stranka.ID;
                        await _userManager.UpdateAsync(user);
                        
                        // Dodelitev vloge Stranka
                        await _userManager.AddToRoleAsync(user, "Stranka");
                    }
                    else if (Input.Vloga == "Mehanik")
                    {
                        var mehanik = new Mehanik
                        {
                            Ime = Input.FirstName,
                            Priimek = Input.LastName,
                            Email = Input.Email,
                            DatumZaposlitve = DateTime.Now,
                            Specializacija = "Mehanik",
                            Telefon = ""
                        };
                        
                        _context.Mehaniki.Add(mehanik);
                        await _context.SaveChangesAsync();
                        
                        user.MehanikID = mehanik.MehanikID;
                        await _userManager.UpdateAsync(user);
                        
                        // Dodelitev vloge Mehanik
                        await _userManager.AddToRoleAsync(user, "Mehanik");
                    }

                    // Email confirmation (opcijsko)
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Potrdite svoj email",
                        $"Prosimo potrdite svoj račun z <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikom tukaj</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}