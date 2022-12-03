using Microsoft.AspNetCore.Mvc;

namespace ShoppingLikeFlies.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class PreviewController : ControllerBase
{
    [HttpGet]
    [Route("{id:guid}")]
    public Task<ActionResult> OnGet([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }
}
