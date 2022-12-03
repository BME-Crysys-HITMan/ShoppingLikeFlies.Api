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
            var id = context.User.Claims.FirstOrDefault(c => c.Type == "Id");
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "uuid");

            if(id is null || userId is null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            Guid guid = Guid.Parse(id.Value);

            if (cache.ValidateToken(guid, userId.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
