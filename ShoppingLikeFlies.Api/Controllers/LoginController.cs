using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFlies.Api.Security.DAL;
using ShoppingLikeFlies.Api.Services;

namespace ShoppingLikeFlies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<LoginController> logger;
        private readonly ITokenGenerator token;
        private readonly ITokenCache cache;

        public LoginController(UserManager<ApplicationUser> userManager, ITokenGenerator token, ITokenCache cache, ILogger<LoginController> logger)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.logger = logger;
            this.token = token ?? throw new ArgumentNullException(nameof(token)); ;
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache)); ;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponse>> OnPostAsync
            (
                [FromBody] LoginRequest contract
            )
        {
            logger.LogInformation("Method {method} called with params: {username}", nameof(OnPostAsync), contract.username);
            var user = await userManager.FindByNameAsync(contract.username);
            if (user == null)
            {
                return Unauthorized();
            }

            var isCorrect = await userManager.CheckPasswordAsync(user, contract.password);

            if (isCorrect)
            {
                var jwt = await token.GenerateToken(user);
                return new LoginResponse(Guid.Parse(user.Id), user.UserName, user.FirstName, user.LastName, await userManager.IsInRoleAsync(user, "Admin") ,jwt);
            }

            return Unauthorized();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult OnDelete()
        {
            logger.LogInformation("Method {method} called", nameof(OnDelete));
            var tokenId = User.Claims.FirstOrDefault(x => x.Type == "Id");
            var userId = User.Claims.FirstOrDefault(x => x.Type == "uuid");

            if(tokenId is null || userId is null)
            {
                return Unauthorized();
            }

            cache.InvalidateToken(Guid.Parse(tokenId.Value), userId.Value);

            return NoContent();
        }
    }
}
