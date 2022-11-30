namespace ShoppingLikeFlies.Api.Contracts.Response;

public record LoginResponse(Guid id, string username, string firstname, string lastname, bool isAdmin, string accessToken);
