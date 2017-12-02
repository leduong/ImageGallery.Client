using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Client.Configuration;
using ImageGallery.Client.Services;
using ImageGallery.Client.ViewModels;
using ImageGallery.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;

namespace ImageGallery.Client.Apis
{
    [Route("api/images")]
    public class GalleryApiController : Controller
    {
        private readonly IImageGalleryHttpClient _imageGalleryHttpClient;
        private readonly ILogger<GalleryApiController> _logger;

        private ConfigurationOptions ApplicationSettings { get; }

        public GalleryApiController(IOptions<ConfigurationOptions> settings,
            IImageGalleryHttpClient imageGalleryHttpClient, ILogger<GalleryApiController> logger)
        {
            _logger = logger;
            ApplicationSettings = settings.Value;
            _imageGalleryHttpClient = imageGalleryHttpClient;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GalleryIndexViewModel), 200)]
        public async Task<ActionResult> GalleryIndexViewModel()
        {
            await WriteOutIdentityInformation();

            // call the API
            var imagesRoute = "api/images";
            var httpClient = await _imageGalleryHttpClient.GetClient();

            var response = await httpClient.GetAsync(imagesRoute).ConfigureAwait(false);

            _logger.LogInformation($"Call {imagesRoute} return {response.StatusCode}.");

            if (response.IsSuccessStatusCode)
            {
                var imagesAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var galleryIndexViewModel = new GalleryIndexViewModel
                    (
                      JsonConvert.DeserializeObject<IList<Image>>(imagesAsString).ToList(),
                      ApplicationSettings.ImagesUri
                    );

                return Ok(galleryIndexViewModel);
            }

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                    return Unauthorized();

                case System.Net.HttpStatusCode.Forbidden:
                    return new ForbidResult();
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EditImage(Guid id)
        {
            // call the API
            var imagesRoute = $"api/images/{id}";
            var httpClient = await _imageGalleryHttpClient.GetClient();

            var response = await httpClient.GetAsync(imagesRoute).ConfigureAwait(false);

            _logger.LogInformation($"Call {imagesRoute} return {response.StatusCode}.");

            if (response.IsSuccessStatusCode)
            {
                var imageAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var deserializedImage = JsonConvert.DeserializeObject<Image>(imageAsString);

                var editImageViewModel = new EditImageViewModel
                {
                    Id = deserializedImage.Id,
                    Title = deserializedImage.Title,
                    Category = deserializedImage.Category,
                };

                return Ok(editImageViewModel);
            }

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                    return Unauthorized();

                case System.Net.HttpStatusCode.Forbidden:
                    return new ForbidResult();
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        public async Task WriteOutIdentityInformation()
        {
            // get the saved identity token
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            // write it out
            Debug.WriteLine($"Identity token: {identityToken}");

            // write out the user claims
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }
    }
}