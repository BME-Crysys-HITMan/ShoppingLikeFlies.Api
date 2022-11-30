using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFlies.Api.Contracts.Response;

namespace ShoppingLikeFlies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaffController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<ActionResult<List<CaffResponse>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public Task<ActionResult> CreateAsync()
        {
            // helper: https://learn.microsoft.com/en-us/aspnet/web-api/overview/advanced/sending-html-form-data-part-2
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<CaffResponse>> GetOne(
            [FromRoute] Guid id
        )
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<ActionResult> UpdateAsync(
            [FromRoute] Guid id
        )
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<ActionResult> DeleteAsync(
            [FromRoute] Guid id
        )
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id:guid}/download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<ActionResult<DownloadResponse>> DownloadAsync(
            [FromRoute] Guid id
        )
        {
            throw new NotImplementedException();
        }
    }
}
