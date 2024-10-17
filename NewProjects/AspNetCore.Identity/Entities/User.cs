using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.Entities
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }

    }
}