using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Controllers;

namespace SchoolSocialMediaApp.Areas.Student.Controllers
{
    public class StudentController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
