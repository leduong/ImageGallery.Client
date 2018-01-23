using ImageGallery.Client.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ImageGallery.Client.Controllers
{
    [Route("api/[controller]")]
    public class ClientAppSettingsController : Controller
    {
        private readonly OpenIdConnectConfiguration _clientAppSettings;
        private readonly LogglyClientConfiguration _logglyClientConfiguration;

        public ClientAppSettingsController(IOptions<OpenIdConnectConfiguration> clientAppSettings, IOptions<LogglyClientConfiguration> logglyClientConfiguration)
        {
            _clientAppSettings = clientAppSettings.Value;
            _logglyClientConfiguration = logglyClientConfiguration.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { OpenIdConnectConfiguration = _clientAppSettings, LogglyClientConfiguration = _logglyClientConfiguration });
        }
    }
}