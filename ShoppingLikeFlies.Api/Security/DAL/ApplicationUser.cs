using Microsoft.AspNetCore.Identity;

namespace ShoppingLikeFlies.Api.Security.DAL;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
