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
        
        public string GenereteEmail()
        {
            return new Faker().Internet.Email();
        }
        
        public string GenereteName()
        {
            return new Faker().Name.FullName();
        }

        public EmailSenderInput GenereteEmailSenderInput(bool value = false)
        {
            var faker = new Faker<EmailSenderInput>()
                .CustomInstantiator(
                    fake => EmailSenderInput
                        .Create()
                        .SetHtmlContent(null)
                        .SetSubject(fake.Lorem.Word())
                        .SetSendForEmailOnOptions(value)
                        .SetPlainTextContent(fake.Lorem.Paragraph())
                        .SetEmailTo(fake.Internet.Email(), fake.Name.FullName())
                        .SetEmailFrom(fake.Internet.Email(), fake.Name.FullName())
                );

            return faker.Generate();
        }
    }
}