using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ImageGallery.Client.Configuration;
using ImageGallery.Client.Services;
using ImageGallery.Client.ViewModels;
using ImageGallery.Model;
using Microsoft.AspNetCore.Authorization;
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
        [Route("edit")]
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

        [HttpDelete]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            // call the API
            var httpClient = await _imageGalleryHttpClient.GetClient();

            var response = await httpClient.DeleteAsync($"{InternalImagesRoute}/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                return Ok();

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return Unauthorized();

                case HttpStatusCode.Forbidden:
                    return new ForbidResult();
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpPost]
        [Route("add")]
        //[Authorize(Policy = "CanOrderFrame")]
        public async Task<IActionResult> AddImage(AddImageViewModel addImageViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // create an ImageForCreation instance
            var imageForCreation = new ImageForCreation
            {
                Title = addImageViewModel.Title,
                Category = addImageViewModel.Category,
            };

            // take the first (only) file in the Files list
            var imageFile = addImageViewModel.File;

            if (imageFile.Length > 0)
            {
                using (var fileStream = imageFile.OpenReadStream())
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    imageForCreation.Bytes = ms.ToArray();
                }
            }

            // serialize it
            var serializedImageForCreation = JsonConvert.SerializeObject(imageForCreation);

            // call the API
            var httpClient = await _imageGalleryHttpClient.GetClient();

            var response = await httpClient.PostAsync(
                    InternalImagesRoute,
                    new StringContent(serializedImageForCreation, Encoding.Unicode, "application/json"))
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                return Ok();

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }
    }
}