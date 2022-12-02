using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFlies.Api.Security.DAL;

namespace ShoppingLikeFlies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Serilog.ILogger logger;

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponse>> OnPostAsync
            (
                [FromBody] LoginRequest contract
            )
        {
            var user = await userManager.FindByNameAsync(contract.username);
            if (user == null)
            {
                return Unauthorized();
            }

            var isCorrect = await userManager.CheckPasswordAsync(user, contract.password);

            if (isCorrect)
            {

            }

            return Unauthorized();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> OnDeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
