using System.Threading.Tasks;
using FluentAssertions;
using Neutralize.Emails;
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
        [Fact(DisplayName = "Send e-mail with fail because the api key is wrong")]
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