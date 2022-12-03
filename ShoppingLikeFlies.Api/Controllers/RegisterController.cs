using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFlies.Api.Security.DAL;

namespace ShoppingLikeFlies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IValidator<RegisterRequest> validator;

        public RegisterController(ILogger<RegisterController> logger, UserManager<ApplicationUser> userManager, IValidator<RegisterRequest> validator)
        {
            logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RegistrationErrorResponse))]
        public async Task<IActionResult> OnPostAsync
            (
                [FromBody] RegisterRequest contract
            )
        {
            logger.LogInformation("Method {method} called", nameof(OnPostAsync));

            var result = await validator.ValidateAsync(contract);
            List<string> errors;
            RegistrationErrorResponse error;
            if (!result.IsValid)
            {
                errors = new List<string>();

                foreach (var item in result.Errors)
                {
                    errors.Add(item.ErrorMessage);
                }

                error = new RegistrationErrorResponse(errors);

                return BadRequest(error);
            }

            var user = new ApplicationUser
            {
                Email = contract.username,
                EmailConfirmed = true,
                UserName = contract.username,
                LastName = contract.lastname,
                FirstName = contract.firstname,
            };

            var identityResult = await userManager.CreateAsync(user, contract.password);

            if (identityResult.Succeeded)
            {
                return Created(nameof(OnPostAsync), null);
            }

            errors = new List<string>();

            foreach (var item in identityResult.Errors)
            {
                errors.Add(item.Description);
            }

            error = new RegistrationErrorResponse(errors);

            return BadRequest(error);
        }
    }
}
