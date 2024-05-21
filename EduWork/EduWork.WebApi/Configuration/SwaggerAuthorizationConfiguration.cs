namespace EduWork.WebApi.Configuration
{
    public class SwaggerAuthorizationConfiguration
    {
        public const string Section = "SwaggerAuthorization";

        public string ClientId { get; set; }

        public string TokenUrl { get; set; }

        public string AuthorizationUrl { get; set; }

        public string Scope { get; set; }
    }
}
