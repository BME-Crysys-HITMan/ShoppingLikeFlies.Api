using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFlies.Api.Contracts.Incoming.Users;
using ShoppingLikeFlies.Api.Security.DAL;

namespace ShoppingLikeFlies.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly Serilog.ILogger logger;
    private readonly UserManager<ApplicationUser> userManager;

    public UsersController(Serilog.ILogger logger, UserManager<ApplicationUser> userManager, IValidator<RegisterRequest> validator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserResponse>>> OnGetAsync
        ()
    {
        logger.Debug("Method {method} called", nameof(OnGetAsync));
        var list = userManager.Users.ToList().Select(async x =>
            new UserResponse(Guid.Parse(x.Id), x.UserName, x.FirstName, x.LastName, await userManager.IsInRoleAsync(x, "Admin"))
            ).ToList();

        return Ok(list);

    }

    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> OnGetAsync
        (
            [FromRoute] Guid id
        )
    {
        logger.Debug("Method {method} called with params: {id}", nameof(OnGetAsync), id);
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return NotFound();
        }
        return Ok(new UserResponse(Guid.Parse(user.Id), user.UserName, user.FirstName, user.LastName, await isAdminOrSelfAsync(id)));
    }

    [HttpPut]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UserResponse>> OnUpdateAsync
        (
            [FromRoute] Guid id,
            [FromBody] UpdateUserRequest contract
        )
    {
        logger.Debug("Method {method} called with params: {id}", nameof(OnUpdateAsync), id);


        var user = await userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return NotFound();
        }

        var principal = (await userManager.GetUserAsync(User));
        if (principal == null || principal.Id != id.ToString())
        {
            return Unauthorized();
        }

        var updatedUser = new ApplicationUser
        {
            Email = contract.username,
            EmailConfirmed = true,
            UserName = contract.username,
            LastName = contract.lastname,
            FirstName = contract.firstname,
        };
        var identityResult = await userManager.UpdateAsync(updatedUser);

        if (!identityResult.Succeeded)
        {
            return BadRequest(identityResult.Errors);
        }

        return Ok(new UserResponse(Guid.Parse(user.Id), user.UserName, user.FirstName, user.LastName, await isAdminOrSelfAsync(id)));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> OnDeleteAsync
        (
            [FromRoute] Guid id
        )
    {
        logger.Debug("Method {method} called with params: {id}", nameof(OnDeleteAsync), id);
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }
        var identityResult = await userManager.DeleteAsync(user);
        if (!identityResult.Succeeded)
        {
            return BadRequest(identityResult.Errors);
        }

        return Ok();
    }

    [HttpPut]
    [Route("{id:guid}/pwreset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> OnPwResetAsync
        (
            [FromRoute] Guid id,
            [FromBody] ChangePasswordRequest contract
        )
    {
        logger.Debug("Method {method} called with params: {id}", nameof(OnPwResetAsync), id);
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        if (!await userManager.CheckPasswordAsync(user, contract.oldPassword))
        {
            return Unauthorized();
        }

        var identityResult = await userManager.ChangePasswordAsync(user, contract.oldPassword, contract.newPassword);

        if (!identityResult.Succeeded)
        {
            return BadRequest(identityResult.Errors);
        }

        identityResult = await userManager.UpdateAsync(user);

        if (!identityResult.Succeeded)
        {
            return BadRequest(identityResult.Errors);
        }

        return Ok();
    }

    [HttpPut]
    [Route("{id:guid}/admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> OnAdminFlipAsync
        (
            [FromRoute] Guid id
        )
    {
        logger.Debug("Method {method} called with params: {id}", nameof(OnAdminFlipAsync), id);
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }
        var role = await isAdminOrSelfAsync(id);
        IdentityResult identityResult;
        if (role)
            identityResult = await userManager.RemoveFromRoleAsync(user, "Admin");
        else
            identityResult = await userManager.AddToRoleAsync(user, "Admin");

        var ir = await userManager.UpdateAsync(user);

        return Ok();
    }

    private async Task<bool> isAdminOrSelfAsync(Guid userId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return false;
        }
        var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
        return isAdmin || userId.ToString() == user.Id;

    }
}
