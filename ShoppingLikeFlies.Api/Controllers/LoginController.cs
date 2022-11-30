using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFlies.Api.Contracts.Response;

namespace ShoppingLikeFlies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public Task<ActionResult<LoginResponse>> OnPostAsync()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> OnDeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
