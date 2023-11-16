using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface ISchoolSubjectService
    {
        Task<ICollection<SchoolSubjectViewModel>> GetAllSubjectsInSchoolAsync(Guid schoolId);
    }
}
