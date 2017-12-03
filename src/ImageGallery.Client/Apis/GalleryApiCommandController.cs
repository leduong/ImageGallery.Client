using System;
using System.Net.Http;
using System.Threading.Tasks;
using ImageGallery.Client.Configuration;
using ImageGallery.Client.Services;
using ImageGallery.Client.ViewModels;
using ImageGallery.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ImageGallery.Client.Apis
{
    [Route("api/images")]
    public class GalleryApiCommandController : Controller
    {
        private readonly IImageGalleryHttpClient _imageGalleryHttpClient;

        private const string InternalImagesRoute = "api/images";

        private ConfigurationOptions ApplicationSettings { get; }

        public GalleryApiCommandController(IOptions<ConfigurationOptions> settings,
            IImageGalleryHttpClient imageGalleryHttpClient)
        {
            ApplicationSettings = settings.Value;

            _imageGalleryHttpClient = imageGalleryHttpClient;
        }

        [HttpPost]
        public async Task<IActionResult> EditImage([FromBody] EditImageViewModel editImageViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // create an ImageForUpdate instance
            var imageForUpdate = new ImageForUpdate
            {
                Title = editImageViewModel.Title,
                Category = editImageViewModel.Category,
            };

            // serialize it
            var serializedImageForUpdate = JsonConvert.SerializeObject(imageForUpdate);

            // call the API
            var httpClient = await _imageGalleryHttpClient.GetClient();

            var response = await httpClient.PutAsync(
                    $"{InternalImagesRoute}/{editImageViewModel.Id}",
                    new StringContent(serializedImageForUpdate, System.Text.Encoding.Unicode, "application/json"))
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                return Ok();

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }
    }
}