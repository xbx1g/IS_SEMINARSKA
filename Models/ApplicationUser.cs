using Microsoft.AspNetCore.Identity;

namespace AutoServis.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        
        public string? Vloga { get; set; } // "Stranka", "Mehanik", "Administrator"
        
        public int? StrankaID { get; set; }
        public virtual Stranka? Stranka { get; set; }
        
        public int? MehanikID { get; set; }
        public virtual Mehanik? Mehanik { get; set; }
    }
}