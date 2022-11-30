using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFlies.Api.Contracts.Incoming;
using ShoppingLikeFlies.Api.Contracts.Response;

namespace ShoppingLikeFlies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RegistrationErrorResponse))]
        public Task<IActionResult> OnPostAsync
            (
                [FromBody] RegisterRequest contract
            )
        {
            throw new NotImplementedException();
        }
    }
}
