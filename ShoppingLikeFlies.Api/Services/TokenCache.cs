using Microsoft.IdentityModel.Tokens;
using Serilog.Context;

namespace ShoppingLikeFlies.Api.Services;

public class TokenCache : ITokenCache
{
    private readonly Dictionary<Guid, string> _tokenCache = new Dictionary<Guid, string>();
    private readonly ILogger logger;

    public TokenCache(ILogger logger)
    {
        this.logger = logger.ForContext<TokenCache>();
    }

    public void AddToken(SecurityTokenDescriptor handler)
    {
        
        using (LogContext.PushProperty("method", nameof(AddToken)))
        {
            logger.Verbose("Method {method} called", nameof(AddToken));
            var id = handler.Subject.Claims.First(x => x.Type == "Id").Value;
            var userId = handler.Subject.Claims.First(x => x.Type == "uuid").Value;
            var guid = Guid.Parse(id);
            if (_tokenCache.ContainsKey(guid))
            {
                logger.Debug("Already containing token");
                _tokenCache.Remove(guid);
            }


            if (_tokenCache.ContainsValue(userId))
            {
                logger.Debug("User is already logged in");
                var item = _tokenCache.First(x => x.Value == userId);

                _tokenCache.Remove(item.Key);
            }


            _tokenCache.Add(guid, userId);
            logger.Debug("Token added to cache");
        }
    }

    public void InvalidateToken(Guid id, string userId)
    {
        if(_tokenCache.ContainsKey(id))
        {
            if(_tokenCache[id] == userId)
            {
                _tokenCache.Remove(id);
            }
        }
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
