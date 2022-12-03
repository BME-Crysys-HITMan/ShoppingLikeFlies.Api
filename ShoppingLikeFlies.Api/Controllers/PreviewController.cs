using Microsoft.AspNetCore.Mvc;

namespace ShoppingLikeFlies.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class PreviewController : ControllerBase
{
    private readonly IDataService dataService;

    public PreviewController(IDataService dataService)
    {
        this.dataService = dataService;
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult> OnGet([FromRoute] int id)
    {
        var caff = await dataService.GetCaffAsync(id);
        return Content(caff.ThumbnailPath);
    }
}
