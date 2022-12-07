namespace ShoppingLikeFlies.Api.Contracts.Response;

public class CaffResponse//(int id, string caption, List<string> tags, List<CommentResponse> comments, string creator, string previewUrl)
{
    public int Id { get; set; }
    public string Caption { get; set; }
    public List<string> Tags { get; set; }
    public List<CommentResponse> Comments { get; set; }
    public string Creator { get; set; }
    public string PreviewUrl { get; set; }
}

