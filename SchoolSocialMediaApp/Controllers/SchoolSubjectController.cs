using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;

namespace SchoolSocialMediaApp.Controllers
{
    public class SchoolSubjectController : BaseController
    {
        private readonly ISchoolSubjectService schoolSubjectService;

        public SchoolSubjectController(ISchoolSubjectService _schoolSubjectService)
        {
            this.schoolSubjectService = _schoolSubjectService;
        }

        [HttpGet]
        public async Task<IActionResult> ManageAll(Guid schoolId)
        {
            ICollection<SchoolSubjectViewModel> subjectsInSchool = await schoolSubjectService.GetAllSubjectsInSchoolAsync(schoolId);

            return View(subjectsInSchool);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new SchoolSubjectCreateModel();

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Create(SchoolSubjectCreateModel model)
        {

        }
    }
}
