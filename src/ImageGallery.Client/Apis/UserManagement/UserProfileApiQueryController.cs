using System;
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
    public class UserProfileApiQueryController : Controller
    {
        private const string InternalUserProfileRoute = "api/UserProfile";

        private readonly IJwtHttpServiceClient _jwtHttpServiceClient;
        private readonly ILogger<UserProfileApiQueryController> _logger;

        public UserProfileApiQueryController(
            IJwtHttpServiceClient jwtHttpServiceClient,
            ILogger<UserProfileApiQueryController> logger)
        {
            _jwtHttpServiceClient = jwtHttpServiceClient;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserProfileViewModel), 200)]
        public async Task<IActionResult> Get()
        {
            var response = await _jwtHttpServiceClient
                .GetAsync<UserProfileViewModel>(InternalUserProfileRoute);

            _logger.LogInformation($"Call {InternalUserProfileRoute} return {response.StatusCode}.");

            if (response.IsSuccessStatusCode)
                return Ok(response.Value);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return Unauthorized();

                case HttpStatusCode.Forbidden:
                    return Forbid();
            }

            throw new Exception($"A problem happened while calling the API: {response.Message}");
        }
    }
}