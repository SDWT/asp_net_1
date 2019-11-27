using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public string Description { get; set; }
    }
}
