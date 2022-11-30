namespace ShoppingLikeFlies.Api.Contracts.Incoming;

public record UpdateCaffRequest(string caption, List<string> tags);
