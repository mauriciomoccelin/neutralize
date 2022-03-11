using SendGrid.Helpers.Mail;

namespace Neutralize.SendGrid
{
    public class SendGridEmailSenderOption : ISendGridEmailSenderOption
    {
        public string ApiKey { get; set; }
        public bool IgnoreEmailSending { get; set; }
        public EmailAddress EmailAddressFrom { get; set; }

        public string GetApiKey() => ApiKey;
        public ISendGridEmailSenderOption SetApiKey(string value)
        {
            ApiKey = value;
            return this;
        }

        public EmailAddress GetEmailFrom() => EmailAddressFrom;
        public ISendGridEmailSenderOption SetEmailFrom(string email, string name)
        {
            EmailAddressFrom = new EmailAddress(email, name);
            return this;
        }

        public bool GetIgnoreEmailSending() => IgnoreEmailSending;
        public ISendGridEmailSenderOption SetIgnoreEmailSending(bool value)
        {
            IgnoreEmailSending = value;
            return this;
        }
    }
}