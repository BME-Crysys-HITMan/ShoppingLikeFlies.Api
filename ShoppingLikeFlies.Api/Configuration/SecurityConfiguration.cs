namespace ShoppingLikeFlies.Api.Configuration
{
    public class SecurityConfiguration
    {
        public const string SectionName = "Security";

        public string ConnectionString { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Duration { get; set; }
        public string Key { get; set; }
    }
}
