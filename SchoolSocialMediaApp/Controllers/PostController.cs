using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.ViewModels.Models;

namespace SchoolSocialMediaApp.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostService postService;
        private readonly ISchoolService schoolService;

        public PostController(IPostService _postService, ISchoolService _schoolService)
        {
            this.postService = _postService;
            this.schoolService = _schoolService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = this.GetUserId();
            IEnumerable<PostViewModel>? posts = null;
            try
            {
                var schoolId = await schoolService.GetSchoolIdByUserIdAsync(userId);
                posts = await postService.GetAllPostsAsync(schoolId);
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError("", ae.Message);
                throw;
            }
            return View(posts);
        }
    }
}
