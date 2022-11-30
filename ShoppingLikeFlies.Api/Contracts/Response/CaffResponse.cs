namespace ShoppingLikeFlies.Api.Contracts.Response;

public record CaffResponse(Guid id, string caption, List<string> tags, string creator, string previewUrl);
