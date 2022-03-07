using System.Threading.Tasks;

namespace Neutralize.Emails
{
    public interface IEmailSender
    {
        Task<EmailSenderResponse> Send(EmailSenderInput input);
    }
}