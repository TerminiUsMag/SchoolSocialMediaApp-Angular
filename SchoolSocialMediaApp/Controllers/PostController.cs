using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.ViewModels.Models.Post;

namespace SchoolSocialMediaApp.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostService postService;
        private readonly ISchoolService schoolService;
        private readonly IRoleService roleService;

        public PostController(IPostService _postService, ISchoolService _schoolService, IRoleService _roleService)
        {
            this.postService = _postService;
            this.schoolService = _schoolService;
            this.roleService = _roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, string message = "", string classOfMessage = "")
        {
            var userId = this.GetUserId();
            IEnumerable<PostViewModel>? posts = null;
            try
            {
                var schoolId = await schoolService.GetSchoolIdByUserIdAsync(userId);
                var school = await schoolService.GetSchoolByIdAsync(schoolId);
                int pageSize = 3;
                int pageNumber = page ?? 1;
                posts = await postService.GetAllPostsAsync(schoolId, userId);
                var pagedPosts = posts.ToPagedList(pageNumber, pageSize);
                ViewBag.SchoolName = school.Name;
                ViewBag.Message = message;
                ViewBag.ClassOfMessage = classOfMessage;
                return View(pagedPosts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var post = await postService.GetPostByIdAsync(Id);

            PostEditViewModel model = new PostEditViewModel
            {
                Id = post.Id,
                Content = post.Content,
                Comments = post.Comments,
                DislikesCount = post.DislikesCount,
                Likes = post.Likes,
                LikesCount = post.LikesCount
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.GetUserId();
            try
            {
                var schoolId = await postService.EditPostAsync(model, userId);
                bool userIsAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
                var message = "Post edited successfully";
                var classOfMessage = "text-bg-success";
                if (userIsAdmin)
                {
                    return RedirectToAction("AdminPostView", new { schoolId = schoolId, message = message, classOfMessage = classOfMessage });
                }
                return RedirectToAction("Index", new { message = message, classOfMessage = classOfMessage });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index", "Home", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var userId = this.GetUserId();
            try
            {
                var schoolId = await postService.DeletePostAsync(Id, userId);
                var userIsAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
                if (userIsAdmin)
                    return RedirectToAction("AdminPostView", "Post", new { schoolId = schoolId, message = "Post deleted successfully", classOfMessage = "text-bg-success" });

                return RedirectToAction("Index", new { message = "Post deleted successfully", classOfMessage = "text-bg-success" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index", "Home", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Like(Guid PostId, bool liked)
        {
            var userId = this.GetUserId();
            bool result = false;

            try
            {
                if (!liked)
                    await postService.LikePostAsync(PostId, userId);
                else
                    await postService.UnlikePostAsync(PostId, userId);
                result = true;
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError("", ae.Message);
                result = false;
            }
            return Json(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Guid PostId, string commentText)
        {
            var userId = this.GetUserId();
            bool result = false;

            try
            {
                await postService.AddCommentAsync(PostId, userId, commentText);
                result = true;
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError("", ae.Message);
                result = false;
            }
            return Json(new { success = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(Guid postId)
        {
            try
            {
                var comments = await postService.GetCommentsByPostIdAsync(postId);
                return PartialView("_CommentsPartial", comments); // Assuming "_CommentsPartial" is the name of your partial view for comments
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError("", ae.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CommentDelete(Guid commentId)
        {
            try
            {
                await postService.DeleteCommentAsync(commentId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
