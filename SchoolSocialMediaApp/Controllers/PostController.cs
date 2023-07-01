using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;

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
        public IActionResult Index()
        {
            var userId = this.GetUserId();
            var schoolId = this.GetSchoolId();
            var posts = postService.GetAllPostsAsync();

            return View();
        }
    }
}
