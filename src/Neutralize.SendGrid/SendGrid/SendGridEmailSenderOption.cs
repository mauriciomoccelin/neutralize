namespace Neutralize.SendGrid
{
    public class SendGridEmailSenderOption : ISendGridEmailSenderOption
    {
        public string apiKey;

        public string GetApiKey() => apiKey;
        public void SetApiKey(string value) => apiKey = value;
    }
}