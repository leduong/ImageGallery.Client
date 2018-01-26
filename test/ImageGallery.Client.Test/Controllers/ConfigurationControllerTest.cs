using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ImageGallery.Client.Configuration;
using ImageGallery.Client.Test.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace ImageGallery.Client.Test.Controllers
{
    public class ConfigurationControllerTest
    {
        //https://andrewlock.net/introduction-to-integration-testing-with-xunit-and-testserver-in-asp-net-core/

        private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public ConfigurationControllerTest(ITestOutputHelper output)
        {
            var applicationPath = WebTestHelpers.GetWebApplicationPath();
            var builder = new WebHostBuilder()
                .UseContentRoot(applicationPath)
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .UseApplicationInsights();

            _server = new TestServer(builder);
            _client = _server.CreateClient();
            _output = output;
        }

        [Fact]
        [Trait("Category", "Intergration")]
        public async Task Get_Client_Configuration()
        {
            var response = await _client.GetAsync("/api/ClientAppSettings");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var configurationOptions = JsonConvert.DeserializeObject<ConfigurationOptions>(responseString);

            Console.WriteLine(responseString);
            _output.WriteLine(responseString);

            Assert.NotNull(configurationOptions);
            Assert.IsType<ConfigurationOptions>(configurationOptions);
        }
    }
}
