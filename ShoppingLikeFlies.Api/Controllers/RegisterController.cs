using Microsoft.AspNetCore.Mvc;

namespace ShoppingLikeFlies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        [HttpPost]
        public Task<ActionResult> OnPostAsync()
        {
            throw new NotImplementedException();
        }
    }
}
