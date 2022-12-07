namespace ShoppingLikeFlies.Api.Contracts.Incoming
{
    public class UploadRequest
    {
        public string Description { get; set; }
        public IFormFile Caff { get; set; }
    }
}
