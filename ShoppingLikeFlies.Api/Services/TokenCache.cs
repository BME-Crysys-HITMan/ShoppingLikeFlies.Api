using Microsoft.IdentityModel.Tokens;

namespace ShoppingLikeFlies.Api.Services;

public class TokenCache : ITokenCache
{
    private readonly Dictionary<Guid, string> _tokenCache = new Dictionary<Guid, string>();

    public void AddToken(SecurityTokenDescriptor handler)
    {
        var id = (string)handler.Claims["Id"];
        var userId = (string)handler.Claims["uuid"];
        var guid = Guid.Parse(id);
        if (_tokenCache.ContainsKey(guid))
        {
            _tokenCache.Remove(guid);
        }


        if (_tokenCache.ContainsValue(userId))
        {
            var item = _tokenCache.First(x => x.Value == userId);

            _tokenCache.Remove(item.Key);
        }

        _tokenCache.Add(guid, userId);
    }

    public bool ValidateToken(Guid id, string userId)
    {
        if (userId is null)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        if (_tokenCache.ContainsKey(id))
        {
            var item = _tokenCache[id];
            if (item == userId)
                return true;
        }

        return false;
    }
}
