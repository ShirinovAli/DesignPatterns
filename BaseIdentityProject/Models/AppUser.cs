using Microsoft.AspNetCore.Identity;

namespace BaseIdentityProject.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
