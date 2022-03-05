using System;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Neutralize.Identity;
using Xunit;

namespace Neutralize.Core.Test
{
    [Collection(nameof(NeutralizeCoreCollection))]
    public class AspNetUser_Tests
    {
        private readonly IAspNetUser aspNetUser;
        private readonly NeutralizeCoreFixture fixture;
        
        public AspNetUser_Tests(NeutralizeCoreFixture fixture)
        {
            this.fixture = fixture;
            aspNetUser = fixture.GenereteDefaultNeutralizeAspNetUser();
        }
        
        [Trait("Category", "Core - AspNetUser")]
        [Fact(DisplayName = "Check if current user in request is autenticated")]
        public void IAspNetUser_CheckIfIsAutenticated_WithSuccess()
        {
            // Arrange
            var authenticatedFlag = fixture.Faker.Random.Bool();
            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Setup(h => h.HttpContext.User.Identity.IsAuthenticated)
                .Returns(authenticatedFlag);

            // Act
            var isAutenticated = aspNetUser.IsAutenticated();

            // Assert
            Assert.Equal(isAutenticated, authenticatedFlag);

            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Verify(x => x.HttpContext.User.Identity.IsAuthenticated, Times.Once);
        }
        
        [Trait("Category", "Core - AspNetUser")]
        [Fact(DisplayName = "Check if current user in request your claims")]
        public void IAspNetUser_GetUserClaims_WithSuccess()
        {
            // Arrange
            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Setup(h => h.HttpContext.User.Claims)
                .Returns(fixture.GeneretUserClaims());

            // Act
            var claims = aspNetUser.GetUserClaims();

            // Assert
            Assert.NotEmpty(claims);

            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Verify(x => x.HttpContext.User.Claims, Times.Once);
        }
        
        [Trait("Category", "Core - AspNetUser")]
        [Fact(DisplayName = "Check if current user in request specific claim value")]
        public void IAspNetUser_GetUserSpecificClaimValue_WithSuccess()
        {
            // Arrange
            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Setup(h => h.HttpContext.User.Claims)
                .Returns(fixture.GeneretUserClaims());

            // Act
            var claim = aspNetUser.GetClaim(ClaimTypes.Email);

            // Assert
            Assert.NotNull(claim);

            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Verify(x => x.HttpContext.User.Claims, Times.Once);
        }
        
        [Trait("Category", "Core - AspNetUser")]
        [Fact(DisplayName = "Try check if current user in request specific claim value")]
        public void IAspNetUser_TryGetUserSpecificClaimValue_WithSuccess()
        {
            // Arrange
            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Setup(h => h.HttpContext.User.Claims)
                .Returns(fixture.GeneretUserClaims());

            // Act
            var success = aspNetUser.TryGetClaim(ClaimTypes.Email, out var claim);

            // Assert
            success.Should().BeTrue();
            claim.Should().NotBeNull();

            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Verify(x => x.HttpContext.User.Claims, Times.Once);
        }
        
        [Trait("Category", "Core - AspNetUser")]
        [Fact(DisplayName = "Check if current user in request specific claim value")]
        public void IAspNetUser_GetUserSpecificClaimValues_WithFail()
        {
            // Arrange
            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Setup(h => h.HttpContext.User.Claims)
                .Returns(fixture.GeneretUserClaims());

            // Act
            var func = aspNetUser.Invoking(x => x.GetClaim(string.Empty));;

            // Assert
            func.Should().Throw<InvalidOperationException>();

            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Verify(x => x.HttpContext.User.Claims, Times.Once);
        }
        
        [Trait("Category", "Core - AspNetUser")]
        [Fact(DisplayName = "Try check if current user in request specific claim value")]
        public void IAspNetUser_TryGetUserSpecificClaimValues_WithFail()
        {
            // Arrange
            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Setup(h => h.HttpContext.User.Claims)
                .Returns(fixture.GeneretUserClaims());

            // Act
            var success = aspNetUser.TryGetClaim("non-existent claim", out var claim);

            // Assert
            success.Should().BeFalse();
            claim.Should().BeNull();

            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Verify(x => x.HttpContext.User.Claims, Times.Once);
        }
        
        [Trait("Category", "Core - AspNetUser")]
        [Fact(DisplayName = "Get in current user of request your id value (Claim: NameIdentifier)")]
        public void IAspNetUser_GetUserId_WithSuccess()
        {
            // Arrange
            var httpContextAccessor = fixture.Mocker
                .GetMock<IHttpContextAccessor>();

            httpContextAccessor
                .Setup(h => h.HttpContext.User.Claims)
                .Returns(fixture.GeneretUserClaims());
            
            httpContextAccessor
                .Setup(h => h.HttpContext.User.Identity.IsAuthenticated)
                .Returns(true);

            // Act
            var userId = aspNetUser.GetUserId();

            // Assert
            Assert.NotEqual(0, userId);

            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Verify(x => x.HttpContext.User.Claims, Times.Once);
            
            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Verify(x => x.HttpContext.User.Identity.IsAuthenticated, Times.Once);
        }
        
        [Trait("Category", "Core - AspNetUser")]
        [Fact(DisplayName = "Get in current user of request your e-mail value (Claim: Email)")]
        public void IAspNetUser_GetUserEmail_WithSuccess()
        {
            // Arrange
            var httpContextAccessor = fixture.Mocker
                .GetMock<IHttpContextAccessor>();

            httpContextAccessor
                .Setup(h => h.HttpContext.User.Claims)
                .Returns(fixture.GeneretUserClaims());
            
            httpContextAccessor
                .Setup(h => h.HttpContext.User.Identity.IsAuthenticated)
                .Returns(true);

            // Act
            var email = aspNetUser.GetUserEmail();

            // Assert
            Assert.NotNull(email);
            Assert.NotEqual(string.Empty, email);

            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Verify(x => x.HttpContext.User.Claims, Times.Once);
            
            fixture.Mocker
                .GetMock<IHttpContextAccessor>()
                .Verify(x => x.HttpContext.User.Identity.IsAuthenticated, Times.Once);
        }
    }
}