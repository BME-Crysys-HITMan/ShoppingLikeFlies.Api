namespace ShoppingLikeFlies.Api.Contracts.Response
{
    public record CaffAllResponse(int id, string caption, List<string> tags, string creator, string previewUrl);
}
