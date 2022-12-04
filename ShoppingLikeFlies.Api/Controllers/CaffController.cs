
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFiles.DomainServices.DTOs;

namespace ShoppingLikeFlies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaffController : ControllerBase
    {
        private readonly ICaffService caffService;
        private readonly IDataService dataService;
        private readonly Serilog.ILogger logger;
        private readonly IMapper mapper;

        public CaffController(ICaffService caffService, IDataService dataService, Serilog.ILogger logger, IMapper mapper)
        {
            this.caffService = caffService;
            this.dataService = dataService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CaffAllResponse>>> GetAllAsync()
        {
            var models = await dataService.GetAllAsync();

            var respone = mapper.Map<List<CaffAllResponse>>(models);

            return respone;
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public Task<ActionResult> CreateAsync()
        {
            // helper: https://learn.microsoft.com/en-us/aspnet/web-api/overview/advanced/sending-html-form-data-part-2
            throw new NotImplementedException();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UploadAsync(CaffUploadRequest contract)
        {
            if (contract.file == null) 
            {
                return BadRequest();
            }

            logger.Debug("Uploaded file data: name={name}, length={length}", contract.file.Name, contract.file.Length);

            return Ok();
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CaffResponse>> GetOne(
            [FromRoute] int id
        )
        {
            var model = await dataService.GetCaffAsync(id);

            var response = mapper.Map<CaffResponse>(model);

            return response;
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CaffResponse>> UpdateAsync(
            [FromRoute] int id,
            [FromBody] UpdateCaffRequest contract
        )
        {
            var model = mapper.Map<CaffDTO>(contract);

            model.Id = id;

            await dataService.UpdateCaffAsync(model);
            var data = await dataService.GetCaffAsync(id);

            var response = mapper.Map<CaffResponse>(data);

            return response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id
        )
        {
            await dataService.DeleteCaffAsync(id).ConfigureAwait(false);

            return NoContent();
        }

        [HttpGet]
        [Route("{id:int}/download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<ActionResult<DownloadResponse>> DownloadAsync(
            [FromRoute] int id
        )
        {
            throw new NotImplementedException();
        }
    }
}
