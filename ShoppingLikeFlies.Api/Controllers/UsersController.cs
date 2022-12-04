using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFlies.Api.Contracts.Incoming.Users;
using ShoppingLikeFlies.Api.Security.DAL;
using System.Collections;
using System.Diagnostics.Contracts;

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
        logger.Debug("Method {method} called" , nameof(OnGetAsync));
        var list = new List<UserResponse>();
        var l = userManager.Users.ToList();
        foreach (var x in l)
        {
            var role = await userManager.IsInRoleAsync(x, "Admin");
            list.Add(new UserResponse(Guid.Parse(x.Id), x.UserName, x.FirstName, x.LastName, role));
        }
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

        if(user == null)
        {
            return NotFound();
        }
        return Ok(new UserResponse(Guid.Parse(user.Id), user.UserName, user.FirstName, user.LastName, await userManager.IsInRoleAsync(user, "Admin")));
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
        var loginId = User.Claims.First(x => x.Type == "uuid");
        if  (loginId.Value != user.Id)
        {
            return Unauthorized();
        }

        user.Email = contract.username;
        user.UserName = contract.username;
        user.LastName = contract.lastname;
        user.FirstName = contract.firstname;

        var identityResult = await userManager.UpdateAsync(user);

        if(!identityResult.Succeeded)
        {
            return BadRequest(identityResult.Errors);
        }

        return Ok(new UserResponse(Guid.Parse(user.Id), user.UserName, user.FirstName, user.LastName, await userManager.IsInRoleAsync(user, "Admin")));
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
        var isValidPassword = await userManager.PasswordValidators[0].ValidateAsync(userManager, user, contract.oldPassword);
        if (!isValidPassword.Succeeded)
        {
            return Unauthorized();
        }
        
        var identityResult = await userManager.ChangePasswordAsync(user, contract.oldPassword, contract.newPassword);

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
        var user = await userManager.FindByIdAsync (id.ToString());
        if (user == null)
        {
            return NotFound();
        }
        var role = await userManager.IsInRoleAsync(user, "Admin");
        IdentityResult identityResult;

        if(role)
            identityResult = await userManager.RemoveFromRoleAsync(user, "Admin");
        else
            identityResult = await userManager.AddToRoleAsync(user, "Admin");

        return Ok();
    }
}
