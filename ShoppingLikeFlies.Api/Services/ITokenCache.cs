using Microsoft.IdentityModel.Tokens;

namespace ShoppingLikeFlies.Api.Services;

public interface ITokenCache
{
    void AddToken(SecurityTokenDescriptor handler);
    bool ValidateToken(Guid id, string userId);
}
