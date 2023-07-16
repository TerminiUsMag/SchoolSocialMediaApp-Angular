using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.Invitation;
using SchoolSocialMediaApp.ViewModels.Models.School;

namespace SchoolSocialMediaApp.Core.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IRepository repo;
        private readonly IRoleService roleService;
        private readonly ISchoolService schoolService;

        public InvitationService(IRepository _repo, IRoleService _roleService, ISchoolService _schoolService)
        {
            this.repo = _repo;
            this.roleService = _roleService;
            this.schoolService = _schoolService;
        }

        public async Task AcceptInvitationAsync(Guid id, Guid userId)
        {
            var invitation = repo.All<Invitation>().FirstOrDefault(i => i.Id == id && i.ReceiverId == userId);
            var user = repo.All<ApplicationUser>().FirstOrDefault(u => u.Id == userId);

            if (invitation is null)
            {
                throw new ArgumentException("Invitation is null.");
            }

            if (user is null)
            {
                throw new ArgumentException("User is null.");
            }

            invitation.IsPending = false;
            user.IsInvited = false;
            var acceptedRole = invitation.Role;
            repo.Delete(invitation);

            List<string> roles = await roleService.GetUserRolesAsync(userId);

            foreach (var role in roles)
            {
                await roleService.RemoveUserFromRoleAsync(userId.ToString(), role);
            }

            await roleService.AddUserToRoleAsync(userId.ToString(), acceptedRole);
            await schoolService.AddUserToSchoolAsync(userId, invitation.SchoolId);
            await repo.SaveChangesAsync();
        }

        public async Task DeclineInvitationAsync(Guid id, Guid userId)
        {
            var invitation = repo.All<Invitation>().FirstOrDefault(i => i.Id == id && i.ReceiverId == userId);
            var user = repo.All<ApplicationUser>().FirstOrDefault(u => u.Id == userId);

            if (invitation is null)
            {
                throw new ArgumentException("Invitation is null.");
            }

            if (user is null)
            {
                throw new ArgumentException("User is null.");
            }

            invitation.IsPending = false;
            user.IsInvited = false;
            repo.Delete(invitation);
            await repo.SaveChangesAsync();
        }

        public async Task DeleteAllInvitationsBySchoolIdAsync(Guid schoolId)
        {
            var invitations = await repo.All<Invitation>().Where(i => i.SchoolId == schoolId).Include(i => i.Receiver).ToListAsync();
            foreach (var user in invitations.Select(i => i.Receiver))
            {
                user.IsInvited = false;
            }
            repo.DeleteRange(invitations);
            await repo.SaveChangesAsync();
        }

        public async Task DeleteInvitationByIdAsync(Guid invitationId)
        {
            var invitation = await repo.AllReadonly<Invitation>().Include(i => i.Receiver).FirstOrDefaultAsync(i => i.Id == invitationId);

            if (invitation is null)
            {
                throw new ArgumentException("Invitation is null.");
            }

            var user = invitation.Receiver;

            repo.Delete(invitation!);
            user.IsInvited = false;
            await repo.SaveChangesAsync();
        }

        public async Task<List<ApplicationUser>> GetCandidatesAsync()
        {
            return await repo.AllReadonly<ApplicationUser>().Where(u => !u.IsParent && !u.IsTeacher && !u.IsStudent && !u.IsPrincipal && !u.IsAdmin && !u.IsInvited).ToListAsync();
        }

        public async Task<List<InvitationViewModel>> GetReceivedInvitationsByUserIdAsync(Guid userId)
        {
            var invitations = await repo.AllReadonly<Invitation>().Where(i => i.ReceiverId == userId).Include(i => i.School).Include(i => i.Sender).ToListAsync();

            return invitations.Select(i => new InvitationViewModel
            {
                Id = i.Id,
                SenderId = i.SenderId,
                Sender = i.Sender,
                ReceiverId = i.ReceiverId,
                Receiver = i.Receiver,
                SchoolId = i.SchoolId,
                School = new SchoolViewModel
                {
                    Id = i.School.Id,
                    Description = i.School.Description,
                    ImageUrl = i.School.ImageUrl,
                    Name = i.School.Name,
                    PrincipalId = i.School.PrincipalId,
                },
                Role = i.Role,
                CreatedOn = i.CreatedOn,
                IsPending = i.IsPending,
            }).ToList();
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
