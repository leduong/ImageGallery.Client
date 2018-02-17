namespace ImageGallery.Client.Test.UI.Features.Demo
{
    public class LoginResult
    {
        public LoginResult(bool isSuccessful, string resultMessage)
        {
            IsSuccessful = isSuccessful;
            ResultMessage = resultMessage;
        }

        public bool IsSuccessful { get; }

        public string ResultMessage { get; }
    }
}
