using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Controllers;

namespace SchoolSocialMediaApp.Areas.Teacher.Controllers
{
    public class TeacherController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
