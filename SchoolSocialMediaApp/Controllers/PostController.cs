using Microsoft.AspNetCore.Mvc;

namespace SchoolSocialMediaApp.Controllers
{
    public class PostController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
