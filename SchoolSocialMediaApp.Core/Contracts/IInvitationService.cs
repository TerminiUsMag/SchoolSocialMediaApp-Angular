using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.Invitation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IInvitationService
    {
        Task AcceptInvitationAsync(Guid id, Guid userId);
        Task DeclineInvitationAsync(Guid id, Guid userId);
        Task DeleteAllInvitationsBySchoolIdAsync(Guid schoolId);
        Task DeleteInvitationByIdAsync(Guid invitationId);
        Task<List<ApplicationUser>> GetCandidatesAsync();
        Task<List<InvitationViewModel>> GetReceivedInvitationsByUserIdAsync(Guid userId);
        Task<List<InvitationViewModel>> GetSentInvitationsBySchoolIdAsync(Guid schoolId);
        Task SendInvitationAsync(CreateInvitationViewModel model);
    }
}
