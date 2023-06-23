using Microsoft.AspNetCore.Identity;

namespace TemplatePattern.WebApp.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhotoUrl { get; set; }
        public string Description { get; set; }
    }
}
