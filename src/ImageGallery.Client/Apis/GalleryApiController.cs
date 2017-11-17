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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;

namespace ImageGallery.Client.Apis
{
    [Route("api/images")]
    public class GalleryApiController : Controller
    {
        private readonly IImageGalleryHttpClient _imageGalleryHttpClient;

        private ConfigurationOptions ApplicationSettings { get; }

        public GalleryApiController(IOptions<ConfigurationOptions> settings,
            IImageGalleryHttpClient imageGalleryHttpClient)
        {
            ApplicationSettings = settings.Value;
            _imageGalleryHttpClient = imageGalleryHttpClient;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GalleryIndexViewModel), 200)]
        public async Task<ActionResult> GalleryIndexViewModel()
        {
            await WriteOutIdentityInformation();

            // call the API
            var httpClient = await _imageGalleryHttpClient.GetClient();

            var response = await httpClient.GetAsync("api/images").ConfigureAwait(false);

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
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                     response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
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