namespace ShoppingLikeFlies.Api.Contracts.Response;

public record CaffResponse(Guid id, string caption, List<string> tags, List<CommentResponse> comments, string creator, string previewUrl);
