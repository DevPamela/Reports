namespace Reports.Application.Models.Configuration
{
    public class OpenIddictConfig
    {
        public int AccessTokenLifetimeMinutes { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string LogoutEndpoint { get; set; }
    }
}
