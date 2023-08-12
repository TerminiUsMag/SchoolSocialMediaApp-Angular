using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolSocialMediaApp.Controllers;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.School;
using System.Security.Claims;

namespace SchoolSocialMediaApp.Tests.UnitTests
{
    [TestFixture]
    public class SchoolControllerTests
    {
        private SchoolController CreateSchoolController(bool isAuthorized = true, bool isPrincipal = false, bool isAdmin = false)
        {
            var mockSchoolService = new Mock<ISchoolService>();
            var mockInvitationService = new Mock<IInvitationService>();
            var mockRoleService = new Mock<IRoleService>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null
            );

            if (isAuthorized)
            {
                var claims = new List<Claim>();
                if (isPrincipal)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Principal"));
                }
                if (isAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }

                var mockHttpContext = new Mock<HttpContext>();
                var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
                mockHttpContext.Setup(ctx => ctx.User).Returns(mockClaimsPrincipal);

                var controllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                };

                var controller = new SchoolController(
                    mockSchoolService.Object,
                    mockInvitationService.Object,
                    mockUserManager.Object,
                    mockRoleService.Object,
                    mockSignInManager.Object)
                {
                    ControllerContext = controllerContext
                };

                return controller;
            }

            // Return a controller instance for the case when isAuthorized is false
            return new SchoolController(
                mockSchoolService.Object,
                mockInvitationService.Object,
                mockUserManager.Object,
                mockRoleService.Object,
                mockSignInManager.Object);
        }

        [Test]
        public async Task Index_ReturnsViewResult_WithSchools()
        {
            // Arrange
            var mockSchoolService = new Mock<ISchoolService>();
            mockSchoolService.Setup(service => service.GetAllSchoolsAsync()).ReturnsAsync(new List<SchoolViewModel>());

            var controller = new SchoolController(mockSchoolService.Object, null, null, null, null);

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<IEnumerable<SchoolViewModel>>(viewResult.Model);
        }

        [Test]
        public void RegisterGet_ReturnsViewResult()
        {
            // Arrange
            var controller = CreateSchoolController();

            // Act
            var result = controller.Register();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

    }
}
