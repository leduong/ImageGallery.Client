using ImageGallery.Client.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace ImageGallery.Client.Controllers
{
    [Route("api/[controller]")]
    public class ClientAppSettingsController : Controller
    {
        private readonly ApplicationOptions _configurationOptions;

        public ClientAppSettingsController(IOptions<ApplicationOptions> options)
        {
            _configurationOptions = options.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Log.Information("ConfigurationOptions:{@configurationOptions}", _configurationOptions.ToString());

            return Ok(new
            {
                _configurationOptions.OpenIdConnectConfiguration,
                _configurationOptions.LogglyClientConfiguration,
            });
        }
    }
}