using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IInvitationService
    {
        Task DeleteAllInvitationsBySchoolIdAsync(Guid schoolId);
        Task DeleteInvitationByIdAsync(Guid invitationId);
        Task<List<ApplicationUser>> GetCandidatesAsync();
        Task<List<InvitationViewModel>> GetSentInvitationsBySchoolIdAsync(Guid schoolId);
        Task SendInvitationAsync(CreateInvitationViewModel model);
    }
}
