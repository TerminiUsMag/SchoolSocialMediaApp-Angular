using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;

namespace SchoolSocialMediaApp.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostService postService;

        public PostController(IPostService _postService)
        {
            this.postService = _postService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var posts = postService.GetAllPostsAsync();

            return View();
        }
    }
}
