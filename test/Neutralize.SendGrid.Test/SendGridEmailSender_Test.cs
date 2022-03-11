using System.Threading.Tasks;
using FluentAssertions;
using Neutralize.Emails;
using SendGrid.Helpers.Mail;
using Xunit;

namespace Neutralize.SendGrid.Test
{
    [Collection(nameof(SendGridEmailSenderCollection))]
    public class SendGridEmailSender_Test
    {
        private readonly IEmailSender emailSender;
        private readonly SendGridEmailSenderFixture fixture;
        
        public SendGridEmailSender_Test(SendGridEmailSenderFixture fixture)
        {
            this.fixture = fixture;
            emailSender = fixture.GenereteDefaultEmailSender();
        }
        
        [Trait("EmailSend", "SendGrid")]
        [Fact(DisplayName = "Send e-mail default with fail because the api key is wrong")]
        public async Task SendEmailWithFail()
        {
            // Arrange
            fixture.Mocker
                .GetMock<ISendGridEmailSenderOption>()
                .Setup(x => x.GetApiKey())
                .Returns(fixture.GenereteApiKey);

            var input = fixture.GenereteEmailSenderInput();
            
            // Act
            var response = await emailSender.Send(input);

            // Assert
            response.Success.Should().BeFalse();
            response.Result.Should().Contain("authorization grant", "API_KEY is wrong");
        }
        
        [Trait("EmailSend", "SendGrid")]
        [Fact(DisplayName = "Send e-mail for email option from with fail because the api key is wrong")]
        public async Task SendEmailWithFailForConfigurationOptions()
        {
            // Arrange
            var sendGridEmailSenderOption = fixture.Mocker.GetMock<ISendGridEmailSenderOption>();

            sendGridEmailSenderOption
                .Setup(x => x.GetApiKey())
                .Returns(fixture.GenereteApiKey);
            
            sendGridEmailSenderOption
                .Setup(x => x.GetEmailFrom())
                .Returns(() => new EmailAddress(fixture.GenereteEmail(), fixture.GenereteName()));

            var input = fixture.GenereteEmailSenderInput(true);
            
            // Act
            var response = await emailSender.Send(input);

            // Assert
            response.Success.Should().BeFalse();
            response.Result.Should().Contain("authorization grant", "API_KEY is wrong");
        }
        
        [Trait("EmailSend", "SendGrid")]
        [Fact(DisplayName = "Ignore sending e-mail because was ignored by configuration")]
        public async Task IgnoreSendEmailWithFailForConfigurationOptions()
        {
            // Arrange
            var sendGridEmailSenderOption = fixture.Mocker.GetMock<ISendGridEmailSenderOption>();

            sendGridEmailSenderOption
                .Setup(x => x.GetIgnoreEmailSending())
                .Returns(true);

            var input = fixture.GenereteEmailSenderInput(true);
            
            // Act
            var response = await emailSender.Send(input);

            // Assert
            response.Success.Should().BeFalse();
            response.Result.Should().Contain("The email sending was ignored.");
        }
        
        [Trait("EmailSend", "SendGrid")]
        [Fact(DisplayName = "Send e-mail with sucess", Skip = "Needs API_KEY")]
        public async Task SendEmailWithSuccess()
        {
            // Arrange
            fixture.Mocker
                .GetMock<ISendGridEmailSenderOption>()
                .Setup(x => x.GetApiKey())
                .Returns(fixture.GenereteApiKey);

            var input = fixture.GenereteEmailSenderInput();
            
            // Act
            var response = await emailSender.Send(input);

            // Assert
            response.Success.Should().BeTrue();
            response.Result.Should().BeEmpty();
        }
    }
}