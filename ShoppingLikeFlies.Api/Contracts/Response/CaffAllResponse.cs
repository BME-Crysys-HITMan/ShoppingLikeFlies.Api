namespace ShoppingLikeFlies.Api.Contracts.Response
{
    public class CaffAllResponse
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public List<string> Tags { get; set; }
        public List<CommentResponse> Comments { get; set; }
        public string Creator { get; set; }
        public string PreviewUrl { get; set; }
    }
}
