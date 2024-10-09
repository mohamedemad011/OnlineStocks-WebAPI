using Microsoft.AspNetCore.Identity;

namespace FinShark.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}
