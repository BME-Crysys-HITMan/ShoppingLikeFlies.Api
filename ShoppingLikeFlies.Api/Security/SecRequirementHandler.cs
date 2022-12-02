using ShoppingLikeFlies.Api.Services;

namespace ShoppingLikeFlies.Api.Security
{
    public class SecRequirementHandler : AuthorizationHandler<SecRequirement>
    {
        private readonly ITokenCache cache;

        public SecRequirementHandler(ITokenCache cache)
        {
            this.cache = cache;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SecRequirement requirement)
        {
            var id = context.User.Claims.First(c => c.Type == "Id").Value;
            var userId = context.User.Claims.First(c => c.Type == "uuid").Value;

            Guid guid = Guid.Parse(id);

            if (cache.ValidateToken(guid, userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
