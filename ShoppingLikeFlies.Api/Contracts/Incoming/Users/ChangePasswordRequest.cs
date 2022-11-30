namespace ShoppingLikeFlies.Api.Contracts.Incoming.Users;

public record ChangePasswordRequest(string oldPassword, string newPassword);
