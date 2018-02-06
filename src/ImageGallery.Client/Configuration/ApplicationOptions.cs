namespace ImageGallery.Client.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public class ApplicationOptions
    {
        /// <summary>
        ///
        /// </summary>
        public string ApiUri { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ImagesUri { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Dataprotection Dataprotection { get; set; }

        /// <summary>
        ///
        /// </summary>
        public OpenIdConnectConfiguration OpenIdConnectConfiguration { get; set; }

        /// <summary>
        ///
        /// </summary>
        public LogglyClientConfiguration LogglyClientConfiguration { get; set; }

        /// <summary>
        ///
        /// </summary>
        public LoggingConfiguration LoggingConfiguration { get; set; }

        /// <summary>
        ///
        /// </summary>
        public SwaggerUiConfiguration SwaggerUiConfiguration { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ApiUri:{ApiUri}|ImagesUri{ImagesUri}";
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class Dataprotection
    {
        public string RedisConnection { get; set; }

        public string RedisKey { get; set; }

        public bool Enabled { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class OpenIdConnectConfiguration
    {
        public string Authority { get; set; }

        public string ClientSecret { get; set; }

        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public string ResponseType { get; set; }

        public string Scope { get; set; }

        public string PostLogoutRedirectUri { get; set; }
    }

    /// <summary>
    /// Swagger Ui OAuth Configuration
    /// </summary>
    public class SwaggerUiConfiguration
    {
        /// <summary>
        ///
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Realm { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Enabled { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class LoggingConfiguration
    {
        public string RollingFilePath { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class LogglyClientConfiguration
    {
        public string LogglyKey { get; set; }
    }
}