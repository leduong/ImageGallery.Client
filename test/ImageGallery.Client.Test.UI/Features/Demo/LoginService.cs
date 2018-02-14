using System;
using System.Collections.Generic;
using System.Text;
using ImageGallery.Client.Test.UI.Features.Demo.Helpers;

namespace ImageGallery.Client.Test.UI.Features.Demo
{
    public class LoginService
    {
        private readonly IDictionary<string, string> _users = new Dictionary<string, string>();

        public void AddUser(string userName, string password)
        {
            _users.Add(userName, password);
        }

        public LoginResult Login(LoginRequest loginRequest)
        {
            LongRunningOperationSimulator.Simulate();
            string password;
            return _users.TryGetValue(loginRequest.UserName, out password) && password == loginRequest.Password
                ? new LoginResult(true, $"Welcome {loginRequest.UserName}!")
                : new LoginResult(false, "Invalid user name or password.");
        }
    }
}
