using ShoppingLikeFlies.Api.Security.DAL;

namespace ShoppingLikeFlies.Api.Services;

public interface ITokenGenerator
{
    /// <summary>
    /// Generates a JWT token.
    /// </summary>
    /// <param name="user">User to hand the token to.</param>
    /// <returns>Returns a string containing the token.</returns>
    Task<string> GenerateToken(ApplicationUser user);
}
