namespace ShoppingLikeFlies.Api.Contracts.Response
{
    public record UserResponse(Guid id, string username, string firstname, string lastname, bool isAdmin);
}
