using System.Threading.Tasks;
using Neutralize.Emails;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Neutralize.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly ISendGridEmailSenderOption option;

        public SendGridEmailSender(ISendGridEmailSenderOption option)
        {
            this.option = option;
        }

        public async Task<EmailSenderResponse> Send(EmailSenderInput input)
        {
            if (option.GetIgnoreEmailSending())
                return new EmailSenderResponse
                {
                    Success = false,
                    Result = "The email sending was ignored."
                };

            var client = new SendGridClient(option.GetApiKey());
            
            var toEmailAddress = new EmailAddress(input.EmailTo, input.NameEmailTo);
            var fromEmailAddress = input.SendForEmailOnOptions
                ? option.GetEmailFrom()
                : new EmailAddress(input.EmailFrom, input.NameEmailFrom);
            
            var mensagem = MailHelper.CreateSingleEmail(
                fromEmailAddress,
                toEmailAddress,
                input.Subject,
                input.PlainTextContent,
                input.HtmlContent
            );

            var response = await client.SendEmailAsync(mensagem);

            return new EmailSenderResponse
            {
                Success = response.IsSuccessStatusCode,
                Result = await response.Body.ReadAsStringAsync()
            };
        }
    }
}