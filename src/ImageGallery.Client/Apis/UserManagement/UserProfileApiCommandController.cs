using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using ImageGallery.Client.ViewModels.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NavigatorIdentity.HttpClient.Interfaces;

namespace ImageGallery.Client.Apis.UserManagement
{
    [Authorize]
    [Route(UserManagementRoutes.UserProfile)]
    public class UserProfileApiCommandController : Controller
    {
        private const string InternalUserProfileRoute = "api/UserProfile";

        private readonly IJwtHttpServiceClient _jwtHttpServiceClient;
        private readonly ILogger<UserProfileApiCommandController> _logger;

        public UserProfileApiCommandController(
            IJwtHttpServiceClient jwtHttpServiceClient,
            ILogger<UserProfileApiCommandController> logger)
        {
            _jwtHttpServiceClient = jwtHttpServiceClient;
            _logger = logger;
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserProfileUpdateViewModel), 200)]
        public async Task<IActionResult> Put([FromBody] [Required] UserProfileUpdateViewModel model)
        {
            var response = await _jwtHttpServiceClient
                .PutAsync<UserProfileUpdateViewModel, UserProfileUpdateViewModel>(InternalUserProfileRoute, model);

            _logger.LogInformation($"Call {InternalUserProfileRoute} return {response.StatusCode}.");

            if (response.IsSuccessStatusCode)
                return Ok(response.Value);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return Unauthorized();

                case HttpStatusCode.Forbidden:
                    return new ForbidResult();
            }

            throw new Exception($"A problem happened while calling the API: {response.Message}");
        }
    }
}