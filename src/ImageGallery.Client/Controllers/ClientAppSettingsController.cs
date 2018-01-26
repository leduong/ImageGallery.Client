﻿using ImageGallery.Client.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ImageGallery.Client.Controllers
{
    [Route("api/[controller]")]
    public class ClientAppSettingsController : Controller
    {
        private readonly ConfigurationOptions _configurationOptions;

        public ClientAppSettingsController(IOptions<ConfigurationOptions> options)
        {
            _configurationOptions = options.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                _configurationOptions.OpenIdConnectConfiguration,
                _configurationOptions.LogglyClientConfiguration,
            });
        }
    }
}