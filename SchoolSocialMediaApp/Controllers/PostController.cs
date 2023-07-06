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

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PostCreateModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.GetUserId();
            try
            {
                await postService.CreatePostAsync(model, userId);
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError("", ae.Message);
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}
