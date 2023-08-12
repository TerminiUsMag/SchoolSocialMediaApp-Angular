using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolSocialMediaApp.Controllers;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.Post;
using SchoolSocialMediaApp.ViewModels.Models.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.Tests.UnitTests
{
    [TestFixture]
    public class PostControllerTests
    {
        private PostController CreatePostController(bool isAuthorized = true)
        {
            var mockPostService = new Mock<IPostService>();
            var mockSchoolService = new Mock<ISchoolService>();
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
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "user_id_here") // Replace with a valid user ID
                };
                var mockHttpContext = new Mock<HttpContext>();
                var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
                mockHttpContext.Setup(ctx => ctx.User).Returns(mockClaimsPrincipal);

                var controllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                };

                var controller = new PostController(
                    mockPostService.Object,
                    mockSchoolService.Object,
                    mockRoleService.Object)
                {
                    ControllerContext = controllerContext
                };

                return controller;
            }

            return new PostController(
                mockPostService.Object,
                mockSchoolService.Object,
                mockRoleService.Object);
        }

        [Test]
        public async Task Index_ReturnsViewResult_WithPosts()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            var mockSchoolService = new Mock<ISchoolService>();
            var mockRoleService = new Mock<IRoleService>();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null
            );

            var controller = new PostController(
                mockPostService.Object,
                mockSchoolService.Object,
                mockRoleService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = Mock.Of<HttpContext>()
                }
            };

            var userId = Guid.NewGuid();
            var schoolId = Guid.NewGuid();
            var schoolName = "School Name";
            var posts = new List<PostViewModel>
            {
                new PostViewModel { Id = Guid.NewGuid(), Content = "Post 1" },
                new PostViewModel { Id = Guid.NewGuid(), Content = "Post 2" }
            };

            mockPostService.Setup(service => service.GetAllPostsAsync(schoolId, userId)).ReturnsAsync(posts);
            mockSchoolService.Setup(service => service.GetSchoolIdByUserIdAsync(userId)).ReturnsAsync(schoolId);
            mockSchoolService.Setup(service => service.GetSchoolByIdAsync(schoolId)).ReturnsAsync(new SchoolViewModel { Id = schoolId ,Name = schoolName });

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<IEnumerable<PostViewModel>>(viewResult.Model);
            var model = viewResult.Model as IEnumerable<PostViewModel>;
            Assert.AreEqual(posts.Count, model.Count());
        }

        [Test]
        public async Task Create_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            var mockSchoolService = new Mock<ISchoolService>();
            var mockRoleService = new Mock<IRoleService>();

            var controller = new PostController(
                mockPostService.Object,
                mockSchoolService.Object,
                mockRoleService.Object);

            var model = new PostCreateModel { Content = "New Post" };

            // Act
            var result = await controller.Create(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

    }
}
