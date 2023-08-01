using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.Invitation;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IInvitationService
    {

        /// <summary>
        /// Accepts an Invitation, deletes it after accepting and removes all roles from user except the newly accepted one, adds the user to the School and Saves the changes to the DB.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task AcceptInvitationAsync(Guid id, Guid userId);

        /// <summary>
        /// Declines an Invitation and deletes it after that.Sets the isInvited of the user to false and aves the changes to the DB.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task DeclineInvitationAsync(Guid id, Guid userId);

        /// <summary>
        /// Deletes all pending invitations of a school.
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        Task DeleteAllInvitationsBySchoolIdAsync(Guid schoolId);

        /// <summary>
        /// Deletes a specific invitations by its ID.
        /// </summary>
        /// <param name="invitationId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task DeleteInvitationByIdAsync(Guid invitationId);

        /// <summary>
        /// Gets a list of all free candidates which can be invited to a school.
        /// </summary>
        /// <returns></returns>
        Task<List<ApplicationUser>> GetCandidatesAsync();

        /// <summary>
        /// Gets the received invitations by user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<InvitationViewModel>> GetReceivedInvitationsByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets the Sent invitations by school.
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        Task<List<InvitationViewModel>> GetSentInvitationsBySchoolIdAsync(Guid schoolId);

        /// <summary>
        /// Sends an invitation to a user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task SendInvitationAsync(CreateInvitationViewModel model);


    }
}
