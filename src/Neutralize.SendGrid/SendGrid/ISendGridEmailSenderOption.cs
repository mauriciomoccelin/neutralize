using SendGrid.Helpers.Mail;

namespace Neutralize.SendGrid
{
    public interface ISendGridEmailSenderOption
    {
        string ApiKey { get; }
        EmailAddress EmailAddressFrom { get; }
        
        public string GetApiKey();
        public ISendGridEmailSenderOption SetApiKey(string value);
        
        public EmailAddress GetEmailFrom();
        public ISendGridEmailSenderOption SetEmailFrom(string email, string name);
    }
}