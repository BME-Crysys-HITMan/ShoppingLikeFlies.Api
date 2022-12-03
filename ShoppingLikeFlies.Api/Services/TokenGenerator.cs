using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ShoppingLikeFlies.Api.Configuration;
using ShoppingLikeFlies.Api.Security.DAL;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingLikeFlies.Api.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IOptions<SecurityConfiguration> options;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenCache cache;
        private readonly ILogger logger;
        public TokenGenerator(IOptions<SecurityConfiguration> options, UserManager<ApplicationUser> userManager, ITokenCache cache, ILogger logger)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            logger.Verbose("Method {method} called with user:{user}", nameof(GenerateToken), user.UserName);
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var isAdminTask = userManager.IsInRoleAsync(user, "Admin");

            var cfg = options.Value;

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg.Key));
            List<Claim> claims = new()
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uuid", user.Id),
            };

            bool isAdmin = await isAdminTask;

            if (isAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(cfg.Duration),
                Issuer = cfg.Issuer,
                Audience = cfg.Audience,
                SigningCredentials = new SigningCredentials(
                    signingKey,
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            cache.AddToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
