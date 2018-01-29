using ImageGallery.Client.Configuration;
using Loggly;
using Loggly.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace ImageGallery.Client.Controllers
{
    [Route("api/[controller]")]
    public class ClientAppSettingsController : Controller
    {
        private readonly ApplicationOptions _configurationOptions;

        private readonly ILogglyClient _logglyClient;

        private readonly LogglyEvent _logglyEvent;

        public ClientAppSettingsController(IOptions<ApplicationOptions> options, ILogglyClient logglyClient)
        {
            _configurationOptions = options.Value;
            _logglyClient = logglyClient;
            _logglyEvent = new LogglyEvent();
            _logglyEvent.Options.Tags.Add(new SimpleTag { Value = "ClientAppSettings" });
        }

        [HttpGet]
        public IActionResult Get()
        {
            Log.Information("ConfigurationOptions:{@configurationOptions}", _configurationOptions.ToString());

            _logglyEvent.Data.Add("ClientAppSettings:", "{0}", _configurationOptions.ToString());
            _logglyClient.Log(_logglyEvent);

            return Ok(new
            {
                _configurationOptions.OpenIdConnectConfiguration,
                _configurationOptions.LogglyClientConfiguration,
            });
        }
    }
}