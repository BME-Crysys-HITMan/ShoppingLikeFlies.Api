using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFlies.Api.Contracts.Incoming.Users;
using ShoppingLikeFlies.Api.Contracts.Response;

namespace ShoppingLikeFlies.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ActionResult<List<UserResponse>>> OnGetAsync
        ()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ActionResult<UserResponse>> OnGetAsync
        (
            [FromRoute] Guid id
        )
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = "OnlySelf")]
    public Task<ActionResult<UserResponse>> OnUpdateAsync
        (
            [FromRoute] Guid id,
            [FromBody] UpdateUserRequest contract
        )
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = "SelfOrAdmin")]
    public Task<ActionResult> OnDeleteAsync
        (
            [FromRoute] Guid id
        )
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/pwreset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<IActionResult> OnPwResetAsync
        (
            [FromRoute] Guid id,
            [FromBody] ChangePasswordRequest contract
        )
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> OnAdminFlipAsync
        (
            [FromRoute] Guid id
        )
    {
        throw new NotImplementedException();
    }
}
