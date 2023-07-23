using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.School;

namespace SchoolSocialMediaApp.Core.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IRepository repo;
        private readonly IRoleService roleService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment env;

        public SchoolService(IRepository _repo, IRoleService _roleService, UserManager<ApplicationUser> _userManager, IWebHostEnvironment _env)
        {
            this.repo = _repo;
            this.roleService = _roleService;
            this.userManager = _userManager;
            this.env = _env;
        }

        public async Task<SchoolViewModel> CreateSchoolAsync(SchoolViewModel model, Guid userId)
        {
            //Checks if the user is already principal of a school
            var userIsPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
            if (userIsPrincipal)
            {
                throw new ArgumentException("User is already a principal of another school.");
            }

            //Checks if the name of the new school is unique
            if (await repo.All<School>().AnyAsync(s => s.Name.ToLower().Trim() == model.Name.ToLower().Trim()))
            {
                throw new ArgumentException("Already exists school with that name");
            }

            //Adds user to the "Principal" role
            var userAddedToRole = await roleService.AddUserToRoleAsync(userId.ToString(), "Principal");
            if (!userAddedToRole)
            {
                throw new ArgumentException("User could not be added to role.");
            }

            //Gets the user from the Db
            var user = repo.All<ApplicationUser>().Where(x => x.Id == userId).FirstOrDefault();

            //Creates the new school entity
            var school = new School
            {
                Id = Guid.NewGuid(),
                Name = model.Name.Trim(),
                Description = model.Description,
                ImageUrl = model.ImageUrl!,
                Location = model.Location,
                Principal = user!,
                PrincipalId = userId,
            };

            //Adds the school entity to the Db
            await repo.AddAsync<School>(school);
            //Save changes to Db
            await repo.SaveChangesAsync();

            //Returns the school model to be viewed in the success page
            return new SchoolViewModel
            {
                Id = school.Id,
                Name = school.Name,
                Description = school.Description,
                ImageUrl = school.ImageUrl,
                Location = school.Location,
                PrincipalId = school.PrincipalId,
                PrincipalName = user!.FirstName + " " + user.LastName
            };
        }

        public async Task DeleteSchoolAsync(Guid id)
        {
            var school = await repo.GetByIdAsync<School>(id);
            if (school is null)
            {
                throw new ArgumentException("School does not exist.");
            }

            repo.Delete<School>(school);

            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<SchoolViewModel>> GetAllSchoolsAsync()
        {
            //Check if there's any registered schools
            var schoolCount = await repo.All<School>().CountAsync();
            if (schoolCount == 0)
            {
                throw new ArgumentException("There are no schools.");
            }

            //Gets the schools from the Database
            var schools = await repo.All<School>().ToListAsync();
            //If the school's principal is deleted make the first and last name of the principal "Deleted User".
            foreach (var school in schools)
            {
                school.Principal = await userManager.FindByIdAsync(school.PrincipalId.ToString());
                if (school.Principal is null)
                {
                    school.Principal = new ApplicationUser
                    {
                        FirstName = "Deleted",
                        LastName = "User"
                    };
                }
            }

            //Map the school to SchoolViewModel to use it in other layers of the application.
            var result = schools.Select(s => new SchoolViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ImageUrl = s.ImageUrl,
                Location = s.Location,
                PrincipalId = s.PrincipalId,
                PrincipalName = s.Principal.FirstName + " " + s.Principal./*User.*/LastName
            }).ToList();

            //Return the mapped SchoolViewModels.
            return result;
        }

        public async Task<SchoolViewModel> GetSchoolByIdAsync(Guid id)
        {
            //Get school and check if it's null.
            var school = await repo.All<School>().Where(s => s.Id == id).Include(s => s.Principal).FirstOrDefaultAsync();
            if (school is null)
            {
                throw new ArgumentException("The school doesn't exist");
            }

            //If school's principal is not found set his name to "Deleted User".
            if (school.Principal is null)
            {
                school.Principal = new ApplicationUser
                {
                    FirstName = "Deleted",
                    LastName = "User"
                };
            }

            //Map the school to SchoolViewModel to be returned and used in other layers of the application.
            return new SchoolViewModel
            {
                Id = school.Id,
                Name = school.Name,
                Description = school.Description,
                ImageUrl = school.ImageUrl,
                Location = school.Location,
                PrincipalId = school.PrincipalId,
                PrincipalName = school.Principal.FirstName + " " + school.Principal.LastName,
            };
        }

        public async Task<SchoolViewModel> GetSchoolByUserIdAsync(Guid userId)
        {
            //All roles in the application which are connected to schools.
            var roles = new List<string>() { "Principal", "Teacher", "Parent", "Student" };

            //Check if the user is in any of those roles and if it's in one of them then he is registered in a school and we find that school and map it to a SchoolViewModel to be usable in other layers of the application.
            foreach (var role in roles)
            {
                if (await roleService.UserIsInRoleAsync(userId.ToString(), role))
                {
                    //var user = await userManager.FindByIdAsync(userId.ToString());
                    var user = await repo.All<ApplicationUser>().Include(u => u.School).FirstOrDefaultAsync(u => u.Id == userId);
                    var school = user!.School;
                    if (school is null)
                    {
                        throw new ArgumentException("The user is in role but not in school.");
                    }

                    return new SchoolViewModel
                    {
                        Id = school!.Id,
                        Description = school.Description,
                        ImageUrl = school.ImageUrl,
                        Location = school.Location,
                        Name = school.Name,
                        PrincipalId = school.PrincipalId,
                        PrincipalName = school.Principal.FirstName + " " + school.Principal.LastName
                    };
                }
            }
            throw new ArgumentException("User is not a member of any school.");
        }

        public async Task<SchoolViewModel> GetSchoolByNameAsync(string name)
        {
            //Get the school and check if it's null.
            var school = await repo.All<School>().FirstOrDefaultAsync(s => s.Name == name);
            if (school is null)
            {
                throw new ArgumentException("School does not exist.");
            }
            //Map the school to a SchoolViewModel to be used in other layers of the application.
            return new SchoolViewModel
            {
                Id = school.Id,
                Name = school.Name,
                Description = school.Description,
                ImageUrl = school.ImageUrl,
                Location = school.Location,
                PrincipalId = school.PrincipalId
            };
        }

        public async Task AddUserToSchoolAsync(Guid userId, Guid schoolId)
        {
            //Get user from Database and check if it's null.
            var user = await repo.All<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new ArgumentException("User does not exist.");
            }
            //Get school from Database and check if it's null.
            var school = await repo.All<School>().FirstOrDefaultAsync(s => s.Id == schoolId);
            if (school is null)
            {
                throw new ArgumentException("School does not exist.");
            }

            //Add user to school and save changes in the Database.
            user.School = school;
            user.SchoolId = school.Id;
            await repo.SaveChangesAsync();
        }

        public async Task<Guid> GetSchoolIdByUserIdAsync(Guid userId)
        {
            //Checks if the userId is empty.
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty.");
            }

            //All roles which are related to a school.
            var roles = new List<string>() { "Principal", "Teacher", "Parent", "Student" };

            //Check if the user is in any of them roles.
            foreach (var role in roles)
            {
                //If the user is in any one of the roles we get the User from the Database and check his schoolId if it's null or empty if not we return it.
                if (await roleService.UserIsInRoleAsync(userId.ToString(), role))
                {
                    var user = await userManager.FindByIdAsync(userId.ToString());
                    var schoolId = user.SchoolId;
                    if (schoolId == Guid.Empty || schoolId is null)
                    {
                        throw new ArgumentException("School id cannot be empty.");
                    }
                    return schoolId.Value;
                }
            }
            throw new ArgumentException("User is not a member of any school.");
        }

        public async Task<SchoolManageViewModel?> GetSchoolManageViewModelByUserIdAsync(Guid userId)
        {
            //Checks if the give userId is not empty.
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty.");
            }

            //Checks if the user is Principal, because only principals have permission to manage schools.
            if (await roleService.UserIsInRoleAsync(userId.ToString(), "Principal"))
            {
                //If the user is Principal get the user and it's school.Check if the user is null.
                var user = await repo.All<ApplicationUser>().Include(u => u.School).FirstOrDefaultAsync(u => u.Id == userId);
                if (user is null)
                {
                    throw new ArgumentException("User doesn't exist");
                }

                //Gets user's school and checks if it's null.
                var school = user.School;
                if (school is null)
                {
                    await roleService.RemoveUserFromRoleAsync(userId.ToString(), "Principal");
                    throw new ArgumentException("School cannot be null.");
                }

                //Gets schools parents, students and teachers by their properties.
                var parents = await repo.AllReadonly<ApplicationUser>().Where(u => u.SchoolId == school.Id && u.IsParent).ToListAsync();
                var students = await repo.AllReadonly<ApplicationUser>().Where(u => u.SchoolId == school.Id && u.IsStudent).ToListAsync();
                var teachers = await repo.AllReadonly<ApplicationUser>().Where(u => u.SchoolId == school.Id && u.IsTeacher).ToListAsync();

                //Map the school and it's participants to a School Manage View Model and return it.
                return new SchoolManageViewModel
                {
                    Id = school.Id,
                    Description = school.Description,
                    ImageUrl = school.ImageUrl,
                    Location = school.Location,
                    Name = school.Name,
                    PrincipalId = school.PrincipalId,
                    Principal = school.Principal,
                    Parents = parents,
                    Students = students,
                    Teachers = teachers,
                };
            }
            //Throw exception if the user is not a principal of any school.
            throw new ArgumentException("User is not a principal of any school.");
        }

        public async Task UpdateSchoolAsync(SchoolViewModel school)
        {
            //Gets the school which we are going to be updating from the Database and checks if it exists.
            var schoolToUpdate = await repo.GetByIdAsync<School>(school.Id);
            if (schoolToUpdate is null)
            {
                throw new ArgumentException("School does not exist.");
            }

            //Maps the changeable properties of the school to the SchoolViewModel.
            schoolToUpdate.Name = school.Name;
            schoolToUpdate.Description = school.Description;
            schoolToUpdate.ImageUrl = school.ImageUrl!;
            schoolToUpdate.Location = school.Location;
            schoolToUpdate.PrincipalId = school.PrincipalId;

            //Saves the changes to the Database.
            await repo.SaveChangesAsync();
        }

        public async Task RemoveUserFromSchoolAsync(Guid userId, Guid schoolId)
        {
            //Checks if the give user and school ID's are not empty.
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty.");
            }
            if (schoolId == Guid.Empty)
            {
                throw new ArgumentException("School id cannot be empty.");
            }

            //Gets the school and the user from the Database.
            var user = await repo.All<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);
            var school = await repo.All<School>().FirstOrDefaultAsync(s => s.Id == schoolId);


            //Checks if the user and school are null.
            if (user is null)
            {
                throw new ArgumentException("User does not exist.");
            }
            if (school is null)
            {
                throw new ArgumentException("School does not exist.");
            }

            //Checks if the user is part of this school and if not throws an exception.
            if (user.SchoolId != school.Id)
            {
                throw new ArgumentException("User is not a member of this school.");
            }

            //Sets the user's school to null.
            user.SchoolId = null;
            user.School = null;
            user.IsParent = false;
            user.IsStudent = false;
            user.IsTeacher = false;

            //Gets the user's roles and removes parent, student and teeacher roles from the user.
            var userRoles = await roleService.GetUserRolesAsync(userId);
            foreach (var role in userRoles)
            {
                if (role.ToLower() != "User" && role.ToLower() != "admin" && role.ToLower() != "principal")
                    await roleService.RemoveUserFromRoleAsync(userId.ToString(), role);
            }

            //Save changes to the Database.
            await repo.SaveChangesAsync();
        }

        public async Task RenameSchoolAsync(Guid schoolId, string schoolName)
        {
            //Gets the school by it's ID and checks if it exists.
            var school = await repo.All<School>().Where(s => s.Id == schoolId).FirstOrDefaultAsync();
            if (school is null)
            {
                throw new ArgumentException("School doesn't exist");
            }

            //Checks if the name is not used by another school.
            if (await repo.All<School>().AnyAsync(s => s.Name.ToLower().Trim() == schoolName.ToLower().Trim()))
            {
                throw new ArgumentException("Name is already taken");
            }

            //Sets the new school name.
            school.Name = schoolName;

            //Saves the changes to the Database.
            await repo.SaveChangesAsync();
        }

        public async Task ChangeSchoolPicture(Guid userId, string imagePath)
        {
            //Get the school by user's ID and check if it exists.
            var school = await this.GetSchoolByUserIdAsync(userId);
            if (school is null)
            {
                throw new ArgumentException("School doesn't exist");
            }

            //Sets the new school image path and saves the changes to the Database.
            school.ImageUrl = imagePath;
            await UpdateSchoolAsync(school);
        }

        public async Task UpdateSchoolAsync(SchoolManageViewModel model)
        {
            //Gets the school to update and checks if it exists.
            var school = await repo.All<School>().Where(s => s.Id == model.Id).FirstOrDefaultAsync();
            if (school is null)
            {
                throw new ArgumentException("School doesn't exist");
            }

            try
            {
                //Gets the file from the model.
                var file = model.ImageFile;
                var fileExtension = Path.GetExtension(file?.FileName);

                // Check if a file was uploaded.
                if ((file is not null || file?.Length != 0) && (fileExtension == ".jpg" || fileExtension == ".png"))
                {
                    // Create a unique file name to save the uploaded image.
                    var fileName = Guid.NewGuid().ToString() + fileExtension;

                    //The directory where the image will be saved.
                    var imagePath = Path.Combine(env.WebRootPath, "images", "school-images", fileName);

                    // Save the file to the specified path.
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file!.CopyToAsync(stream);
                    }

                    model.ImageUrl = $"images/school-images/{fileName}";
                }

                //Map the school to the model's properties.
                school.Location = model.Location;
                school.Name = model.Name;
                school.Description = model.Description;
                school.ImageUrl = model.ImageUrl;

                //Save changes to the Database.
                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
