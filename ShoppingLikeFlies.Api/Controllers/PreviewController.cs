﻿using Microsoft.AspNetCore.Mvc;

namespace ShoppingLikeFlies.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class PreviewController : ControllerBase
{
    private readonly IDataService dataService;
    private readonly ILogger<PreviewController> logger;

    public PreviewController(IDataService dataService, ILogger<PreviewController> logger)
    {
        this.dataService = dataService;
        this.logger = logger;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<ActionResult> OnGet([FromRoute] Guid id)
    {
        logger.LogInformation("Method {method} called with attributes: {id}", nameof(OnGet), id);
        var caff = await dataService.GetCaffAsync(id);
        return Content(caff.ThumbnailPath);
    }
}
