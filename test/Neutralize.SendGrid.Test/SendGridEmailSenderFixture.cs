using System;
using Bogus;
using Moq.AutoMock;
using Neutralize.Emails;
using Xunit;

namespace Neutralize.SendGrid.Test
{
    [CollectionDefinition(nameof(SendGridEmailSenderCollection))]
    public class SendGridEmailSenderCollection : ICollectionFixture<SendGridEmailSenderFixture> {}
    
    public class SendGridEmailSenderFixture : IDisposable
    {
        public AutoMocker Mocker;
        
        public void Dispose()
        {
        }
        
        public IEmailSender GenereteDefaultEmailSender()
        {
            Mocker = new AutoMocker();
            var emailSender = Mocker.CreateInstance<SendGridEmailSender>();
            return emailSender;
        }

        public string GenereteApiKey()
        {
            return new Faker().Internet.Password();
        }

        public EmailSenderInput GenereteEmailSenderInput()
        {
            var faker = new Faker<EmailSenderInput>()
                .CustomInstantiator(fake => new EmailSenderInput
                {
                    EmailFrom = fake.Internet.Email(),
                    NameEmailFrom = fake.Name.FullName(),
                    EmailTo = fake.Internet.Email(),
                    NameEmailTo = fake.Name.FullName(),
                    Subject = fake.Lorem.Word(),
                    HtmlContent = null,
                    PlainTextContent = fake.Lorem.Paragraph()
                });

            return faker.Generate();
        }
    }
}