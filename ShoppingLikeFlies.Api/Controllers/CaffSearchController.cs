
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFiles.DomainServices.DTOs;

namespace ShoppingLikeFlies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaffSearchController : ControllerBase
    {
        
        private readonly IDataService dataService;
        private readonly Serilog.ILogger logger;
        private readonly IMapper mapper;

        public CaffSearchController(ICaffService caffService, IDataService dataService, Serilog.ILogger logger, IMapper mapper)
        {
            this.dataService = dataService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<List<CaffAllResponse>> SearchAsync(CaffSearchDTO caffSearchDTO)
        {
            logger.Debug("Method {method} called with params: {username}", nameof(SearchAsync), caffSearchDTO);
            var models = await dataService.SearchCaffAsync(caffSearchDTO);
            var respone = mapper.Map<List<CaffAllResponse>>(models);
            return respone;
        }
    }
}
