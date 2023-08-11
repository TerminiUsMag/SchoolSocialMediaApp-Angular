using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Core.Services;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;

namespace SchoolSocialMediaApp.Tests.UnitTests
{
    [TestFixture]
    public class AccountServiceTests
    {
        [Test]
        public async Task EmailIsFree_ReturnsTrueWhenEmailIsFree()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var repoMock = new Mock<IRepository>(); // Mock your IRepository dependency
            var roleServiceMock = new Mock<IRoleService>();
            var envMock = new Mock<IWebHostEnvironment>();
            var signInMock = new Mock<SignInManager<ApplicationUser>>();

            userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                          .ReturnsAsync((ApplicationUser)null);

            var accountService = new AccountService(userManagerMock.Object, signInMock.Object, repoMock.Object, roleServiceMock.Object, envMock.Object);

            // Act
            var result = await accountService.EmailIsFree("test@example.com");

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task LoginAsync_ReturnsTrueWhenSuccessful()
        {
            // Arrange
            var roleServiceMock = new Mock<IRoleService>();
            var envMock = new Mock<IWebHostEnvironment>();
            var repoMock = new Mock<IRepository>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);

            userManagerMock.Setup(um => um.FindByEmailAsync("test@example.com"))
                          .ReturnsAsync(new ApplicationUser());

            signInManagerMock.Setup(sm => sm.PasswordSignInAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Success);

            var accountService = new AccountService(userManagerMock.Object, signInManagerMock.Object, repoMock.Object, roleServiceMock.Object, envMock.Object);

            // Act
            var result = await accountService.LoginAsync("test@example.com", "password", false);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
