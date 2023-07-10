using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models;

namespace SchoolSocialMediaApp.Core.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IRepository repo;

        public InvitationService(IRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task DeleteAllInvitationsBySchoolIdAsync(Guid schoolId)
        {
            var invitations = await repo.All<Invitation>().Where(i => i.SchoolId == schoolId).ToListAsync();
            repo.DeleteRange(invitations);
            await repo.SaveChangesAsync();
        }

        public async Task DeleteInvitationByIdAsync(Guid invitationId)
        {
            await repo.DeleteAsync<Invitation>(invitationId);
            await repo.SaveChangesAsync();
        }

        public async Task<List<ApplicationUser>> GetCandidatesAsync()
        {
            return await repo.AllReadonly<ApplicationUser>().Where(u => !u.IsParent && !u.IsTeacher && !u.IsStudent && !u.IsPrincipal && !u.IsAdmin && !u.IsInvited).ToListAsync();
        }

        public async Task<List<InvitationViewModel>> GetSentInvitationsBySchoolIdAsync(Guid schoolId)
        {
            return await repo.All<Invitation>().Where(i => i.SchoolId == schoolId).Include(i => i.Sender).Include(i => i.Receiver).Select(i => new InvitationViewModel
            {
                Id = i.Id,
                SenderId = i.SenderId,
                Sender = i.Sender,
                ReceiverId = i.ReceiverId,
                Receiver = i.Receiver,
                SchoolId = i.SchoolId,
                Role = i.Role,
                CreatedOn = i.CreatedOn,
                IsPending = i.IsPending,
            }).ToListAsync();
        }

        public async Task SendInvitationAsync(CreateInvitationViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentException("Model is null.");
            }
            var invitation = new Invitation
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                ReceiverId = model.ReceiverId,
                SenderId = model.SenderId,
                SchoolId = model.SchoolId,
                Role = model.Role,
                IsPending = true,
            };
            var user = await repo.All<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == model.ReceiverId);
            if (user is null)
            {
                throw new ArgumentException("User is null.");
            }
            if (user.IsInvited)
            {
                throw new ArgumentException("User is already invited.");
            }
            user.IsInvited = true;
            await repo.AddAsync(invitation);
            await repo.SaveChangesAsync();
        }
    }
}
