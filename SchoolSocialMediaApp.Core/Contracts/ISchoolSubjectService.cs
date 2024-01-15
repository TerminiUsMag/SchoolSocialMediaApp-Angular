using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;
using SchoolSocialMediaApp.ViewModels.Models.User;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface ISchoolSubjectService
    {
        /// <summary>
        /// Assigns a class (by it's ID) to subject (by it's ID).
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="classId"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        Task AssignClassToSubject(Guid schoolId, Guid classId, Guid subjectId);

        /// <summary>
        /// Assigns a teacher (by it's ID) to subject (by it's ID).
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="subjectId"></param>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        Task AssignTeacherToSubject(Guid teacherId, Guid subjectId, Guid schoolId);

        /// <summary>
        /// Asynchronously creates a school subject using a SchoolSubjectCreateModel and the user's ID.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns>true or false</returns>
        Task<bool> CreateSchoolSubjectAsync(SchoolSubjectCreateModel model, Guid userId);

        /// <summary>
        /// Delete a subject (using it's ID) and the user's ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        Task DeleteSubject(Guid userId, Guid subjectId);

        /// <summary>
        /// Returns a Collection of all classes which are assignable to the given subject (using subject's and school's IDs).
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="subjectId"></param>
        /// <returns> A collection of SchoolClassViewModel</returns>
        Task<ICollection<SchoolClassViewModel>> GetAllAssignableToSubjectClassesAsync(Guid schoolId, Guid subjectId);

        /// <summary>
        /// Returns all subjects in a school (by school's and user's ID).
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ICollection<SchoolSubjectViewModel>> GetAllSubjectsInSchoolAsync(Guid schoolId, Guid userId);

        /// <summary>
        /// Returns all assigned classes that are assigned to a specific subject (by subject's ID).
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns>A list of SchoolClassViewModel</returns>
        Task<List<SchoolClassViewModel>> GetAssignedClasses(Guid subjectId);

        /// <summary>
        /// Returns all candidate teachers in a school (by school's and user's ID).
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <returns> A Collection of ApplicationUser</returns>
        Task<ICollection<ApplicationUser>> GetCandidateTeachersInSchool(Guid schoolId, Guid userId, bool isAdmin = false);

        /// <summary>
        /// Returns all candidate teachers in a school (by school's and user's ID).
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <returns>A List of TeacherViewModel</returns>
        Task<List<TeacherViewModel>> GetCandidateTeachersViewModelInSchool(Guid schoolId, Guid userId, bool isAdmin);

        /// <summary>
        /// Returns a subject by it's ID.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns>SchoolSubjectViewModel</returns>
        Task<SchoolSubjectViewModel> GetSubjectById(Guid subjectId);

        /// <summary>
        /// Unassign a class from a subject (by their own IDs and the school's ID).
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="classId"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        Task UnAssignClassFromSubject(Guid schoolId, Guid classId, Guid subjectId);
    }
}
