namespace ShoppingLikeFlies.Api.Contracts.Incoming;

public record RegisterRequest(string username, string firstname, string lastname, string password, string passwordConfirm);
