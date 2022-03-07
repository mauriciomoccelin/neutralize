using System;
using Microsoft.Extensions.DependencyInjection;
using Neutralize.Emails;
using SendGrid;

namespace Neutralize.SendGrid
{
    public static class Abstraction
    {
        public static IServiceCollection AddSendGridEmailSender(
            IServiceCollection service,
            Action<SendGridEmailSenderOption> sendGridOption
        )
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (sendGridOption == null) throw new ArgumentNullException(nameof(sendGridOption));

            var options = new SendGridEmailSenderOption();
            
            sendGridOption.Invoke(options);
            
            service.AddSingleton<ISendGridEmailSenderOption>(options);
            service.AddSingleton<IEmailSender, SendGridEmailSender>();

            return service;
        }
    }
}