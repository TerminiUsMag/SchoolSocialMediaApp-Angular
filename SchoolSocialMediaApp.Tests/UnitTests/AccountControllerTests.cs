using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolSocialMediaApp.Controllers;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.User;

namespace SchoolSocialMediaApp.Tests.UnitTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void RegisterGet_ReturnsViewResult()
        {
            // Arrange
            var mockAccountService = new Mock<IAccountService>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var mockRoleService = new Mock<IRoleService>();
            var mockSchoolService = new Mock<ISchoolService>();

            var controller = new AccountController(mockAccountService.Object, mockUserManager.Object, mockRoleService.Object, mockSchoolService.Object);

            // Act
            var result = controller.Register();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task RegisterPost_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(service => service.UsernameIsFree(It.IsAny<string>())).ReturnsAsync(true);
            mockAccountService.Setup(service => service.PhoneNumberIsValid(It.IsAny<string>())).Returns(true);
            mockAccountService.Setup(service => service.PhoneNumberIsFree(It.IsAny<string>())).ReturnsAsync(true);
            mockAccountService.Setup(service => service.EmailIsValid(It.IsAny<string>())).Returns(true);
            mockAccountService.Setup(service => service.EmailIsFree(It.IsAny<string>())).ReturnsAsync(true);
            mockAccountService.Setup(service => service.RegisterAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var mockRoleService = new Mock<IRoleService>();
            var mockSchoolService = new Mock<ISchoolService>();

            var controller = new AccountController(mockAccountService.Object, mockUserManager.Object, mockRoleService.Object, mockSchoolService.Object);
            var model = new RegisterViewModel();

            // Act
            var result = await controller.Register(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

        [Test]
        public async Task Login_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "testuser@example.com" };
            var signInResult = Microsoft.AspNetCore.Identity.SignInResult.Success;
            var mockSchoolService = new Mock<ISchoolService>();
            var mockRoleService = new Mock<IRoleService>();

            var mockAccountService = new Mock<IAccountService>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null
            );

            mockSignInManager.Setup(manager => manager.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(signInResult);

            mockUserManager.Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var controller = new AccountController(mockAccountService.Object, mockUserManager.Object, mockRoleService.Object, mockSchoolService.Object);
            var model = new LoginViewModel();

            // Act
            var result = await controller.Login(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Login", redirectResult.ActionName);
            Assert.AreEqual("Account", redirectResult.ControllerName);
        }

        [Test]
        public async Task Logout_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockSchoolService = new Mock<ISchoolService>();
            var mockRoleService = new Mock<IRoleService>();
            var mockAccountService = new Mock<IAccountService>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null
            );

            mockSignInManager.Setup(manager => manager.SignOutAsync())
                .Returns(Task.CompletedTask);

            var controller = new AccountController(mockAccountService.Object, mockUserManager.Object, mockRoleService.Object, mockSchoolService.Object);

            // Act
            var result = await controller.Logout();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

    }
}