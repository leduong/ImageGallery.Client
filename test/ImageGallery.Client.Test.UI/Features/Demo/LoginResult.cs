using System;
using System.Collections.Generic;
using System.Text;

namespace ImageGallery.Client.Test.UI.Features.Demo
{
    public class LoginResult
    {
        public bool IsSuccessful { get; }

        public string ResultMessage { get; }

        public LoginResult(bool isSuccessful, string resultMessage)
        {
            IsSuccessful = isSuccessful;
            ResultMessage = resultMessage;
        }
    }
}
