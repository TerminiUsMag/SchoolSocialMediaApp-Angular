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
