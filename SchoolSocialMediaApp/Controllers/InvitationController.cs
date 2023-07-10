using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models;

namespace SchoolSocialMediaApp.Controllers
{
    public class InvitationController : BaseController
    {
        private readonly IInvitationService invitationService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISchoolService schoolService;

        public InvitationController(IInvitationService _invitationService, UserManager<ApplicationUser> _userManager, ISchoolService _schoolService)
        {
            this.invitationService = _invitationService;
            this.userManager = _userManager;
            this.schoolService = _schoolService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Policy = "Principal")]
        public async Task<IActionResult> Send(string role, string message)
        {
            var model = new CreateInvitationViewModel();
            model.Role = role;
            model.SenderId = this.GetUserId();
            var school = await schoolService.GetSchoolByUserIdAsync(model.SenderId);
            model.SchoolId = school.Id;
            model.Candidates = await invitationService.GetCandidatesAsync();
            ViewBag.Role = role;
            ViewBag.Message = message;

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "Principal")]
        public async Task<IActionResult> Send(CreateInvitationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid invitation data.");
                return View(model);
            }

            try
            {
                await invitationService.SendInvitationAsync(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

            return RedirectToAction("Send", "Invitation", new { role = model.Role, message = "Invitation sent successfully" });
        }

        [HttpGet]
        [Authorize(Policy = "Principal")]
        public async Task<IActionResult> ManageSent(Guid schoolId, string message)
        {
            List<InvitationViewModel> invitations = await invitationService.GetSentInvitationsBySchoolIdAsync(schoolId);
            ViewBag.Message = message;
            return View(invitations);
        }

        [HttpPost]
        [Authorize(Policy = "Principal")]
        public async Task<IActionResult> Delete(Guid id, Guid schoolId)
        {
            await invitationService.DeleteInvitationByIdAsync(id);

            return RedirectToAction("ManageSent", "Invitation", new { schoolId = schoolId, message = "Invitation deleted successfully" });
        }

        [HttpGet]
        [Authorize(Policy = "Principal")]
        public async Task<IActionResult> DeleteAll(Guid schoolId)
        {
            await invitationService.DeleteAllInvitationsBySchoolIdAsync(schoolId);

            return RedirectToAction("Manage", "School", new { message = "Invitations deleted successfully" });
        }
    }
}
