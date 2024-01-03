using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Controllers;

namespace SchoolSocialMediaApp.Areas.Parent.Controllers
{
    public class ParentController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
