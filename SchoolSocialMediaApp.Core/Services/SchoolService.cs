﻿using Microsoft.AspNetCore.Hosting;
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

        public SchoolService(IRepository _repo, IRoleService _roleService, UserManager<ApplicationUser> _userManager,IWebHostEnvironment _env)
        {
            this.repo = _repo;
            this.roleService = _roleService;
            this.userManager = _userManager;
            this.env = _env;
        }

        public async Task<SchoolViewModel> CreateSchoolAsync(SchoolViewModel model, Guid userId)
        {
            var userIsPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
            if (userIsPrincipal)
            {
                throw new ArgumentException("User is already a principal of another school.");
            }

            var userAddedToRole = await roleService.AddUserToRoleAsync(userId.ToString(), "Principal");
            if (!userAddedToRole)
            {
                throw new ArgumentException("User could not be added to role.");
            }

            var user = repo.All<ApplicationUser>().Where(x => x.Id == userId).FirstOrDefault();


            var school = new School
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl!,
                Location = model.Location,
                Principal = user!,
                PrincipalId = userId,
            };

            await repo.AddAsync<School>(school);

            await repo.SaveChangesAsync();

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
            var schoolCount = await repo.All<School>().CountAsync();
            if (schoolCount == 0)
            {
                throw new ArgumentException("There are no schools.");
            }
            var schools = await repo.All<School>().ToListAsync();

            foreach (var school in schools)
            {
                school.Principal = await userManager.FindByIdAsync(school.PrincipalId.ToString());
            }

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


            return result;
        }

        public async Task<SchoolViewModel> GetSchoolByIdAsync(Guid id)
        {
            var school = await repo.GetByIdAsync<School>(id);
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

            var roles = new List<string>() { "Principal", "Teacher", "Parent", "Student" };

            foreach (var role in roles)
            {
                if (await roleService.UserIsInRoleAsync(userId.ToString(), role))
                {
                    //var user = await userManager.FindByIdAsync(userId.ToString());
                    var user = await repo.All<ApplicationUser>().Include(u => u.School).FirstOrDefaultAsync(u => u.Id == userId);
                    var school = user!.School;

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

        public async Task<SchoolViewModel> GetSchoolIdByNameAsync(string name)
        {
            var school = await repo.All<School>().FirstOrDefaultAsync(s => s.Name == name);
            if (school is null)
            {
                throw new ArgumentException("School does not exist.");
            }
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
            var user = await repo.All<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new ArgumentException("User does not exist.");
            }

            var school = await repo.All<School>().FirstOrDefaultAsync(s => s.Id == schoolId);
            if (school is null)
            {
                throw new ArgumentException("School does not exist.");
            }

            user.School = school;
            user.SchoolId = school.Id;
            await repo.SaveChangesAsync();
        }

        public async Task<Guid> GetSchoolIdByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty.");
            }

            var roles = new List<string>() { "Principal", "Teacher", "Parent", "Student" };

            foreach (var role in roles)
            {
                if (await roleService.UserIsInRoleAsync(userId.ToString(), role))
                {
                    var user = await userManager.FindByIdAsync(userId.ToString());
                    //var userEager = await repo.All<ApplicationUser>().Include(u => u.School).FirstOrDefaultAsync(u => u.Id == userId);
                    var schoolId = user.SchoolId;
                    if (schoolId == Guid.Empty || schoolId is null)
                    {
                        throw new ArgumentException("School id cannot be empty.");
                    }
                    return schoolId.Value;
                }
            }
            throw new ArgumentException("User is not a member of any school.");

            //var schoolId = await repo.AllReadonly<School>().Where(s => s.PrincipalId == userId).Select(s => s.Id).FirstOrDefaultAsync();

            //if (await repo.AllReadonly<Student>().AnyAsync(s => s.UserId == userId))//User is student
            //{
            //    school = await repo.AllReadonly<Student>().Where(s => s.UserId == userId).Select(s => s.School.Id).FirstOrDefaultAsync();
            //}
            //else if (await repo.AllReadonly<Parent>().AnyAsync(p => p.UserId == userId))//User is parent
            //{
            //    school = await repo.AllReadonly<Parent>().Where(p => p.UserId == userId).Select(p => p.School.Id).FirstOrDefaultAsync();
            //}
            //else if (await repo.AllReadonly<Teacher>().AnyAsync(t => t.UserId == userId))//User is teacher
            //{
            //    school = await repo.AllReadonly<Teacher>().Where(t => t.UserId == userId).Select(t => t.School.Id).FirstOrDefaultAsync();
            //}
            //else if (await roleService.UserIsInRoleAsync(userId.ToString(), "Principal")/*await repo.AllReadonly<Principal>().AnyAsync(p => p.UserId == userId)*/)//User is principal
            //{
            //    //school = await repo.AllReadonly<Principal>().Where(p => p.UserId == userId).Select(p => p.School.Id).FirstOrDefaultAsync();
            //    school = await repo.AllReadonly<School>().Where(s => s.PrincipalId == userId).Select(s => s.Id).FirstOrDefaultAsync();
            //}
            //else
            //{
            //    throw new ArgumentException("User is not a student, parent, teacher, or principal.");
            //}

        }

        public async Task<SchoolManageViewModel?> GetSchoolManageViewModelByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty.");
            }

            var roles = new List<string>() { "Principal", "Teacher", "Parent", "Student" };

            foreach (var role in roles)
            {
                if (await roleService.UserIsInRoleAsync(userId.ToString(), role))
                {
                    //var user = await userManager.FindByIdAsync(userId.ToString());
                    var user = await repo.All<ApplicationUser>().Include(u => u.School).FirstOrDefaultAsync(u => u.Id == userId);
                    var school = user!.School;
                    if (school is null)
                    {
                        throw new ArgumentException("School cannot be null.");
                    }
                    var parents = await repo.AllReadonly<ApplicationUser>().Where(u => u.SchoolId == school.Id && u.IsParent).ToListAsync();
                    var students = await repo.AllReadonly<ApplicationUser>().Where(u => u.SchoolId == school.Id && u.IsStudent).ToListAsync();
                    var teachers = await repo.AllReadonly<ApplicationUser>().Where(u => u.SchoolId == school.Id && u.IsTeacher).ToListAsync();

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
            }
            throw new ArgumentException("User is not a member of any school.");
        }

        public async Task UpdateSchoolAsync(SchoolViewModel school)
        {
            var schoolToUpdate = await repo.GetByIdAsync<School>(school.Id);

            if (schoolToUpdate is null)
            {
                throw new ArgumentException("School does not exist.");
            }

            schoolToUpdate.Name = school.Name;
            schoolToUpdate.Description = school.Description;
            schoolToUpdate.ImageUrl = school.ImageUrl!;
            schoolToUpdate.Location = school.Location;
            schoolToUpdate.PrincipalId = school.PrincipalId;

            await repo.SaveChangesAsync();
        }

        public async Task RemoveUserFromSchoolAsync(Guid userId, Guid schoolId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty.");
            }

            if (schoolId == Guid.Empty)
            {
                throw new ArgumentException("School id cannot be empty.");
            }

            var user = await repo.All<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);
            var school = await repo.All<School>().FirstOrDefaultAsync(s => s.Id == schoolId);

            if (user is null)
            {
                throw new ArgumentException("User does not exist.");
            }

            if (school is null)
            {
                throw new ArgumentException("School does not exist.");
            }

            if (user.SchoolId != school.Id)
            {
                throw new ArgumentException("User is not a member of this school.");
            }

            user.SchoolId = null;
            user.School = null;
            user.IsParent = false;
            user.IsStudent = false;
            user.IsTeacher = false;
            var userRoles = await roleService.GetUserRolesAsync(userId);

            foreach (var role in userRoles)
            {
                if (role.ToLower() != "User" && role.ToLower() != "admin" && role.ToLower() != "principal")
                    await roleService.RemoveUserFromRoleAsync(userId.ToString(), role);
            }

            await repo.SaveChangesAsync();
        }

        public async Task RenameSchoolAsync(Guid schoolId, string schoolName)
        {
            var school = await repo.All<School>().Where(s => s.Id == schoolId).FirstOrDefaultAsync();
            if (school is null)
            {
                throw new ArgumentException("School doesn't exist");
            }

            if (await repo.All<School>().AnyAsync(s => s.Name == schoolName))
            {
                throw new ArgumentException("Name is already taken");
            }

            school.Name = schoolName;

            await repo.SaveChangesAsync();
        }

        public async Task ChangeSchoolPicture(Guid userId, string imagePath)
        {
            var school = await this.GetSchoolByUserIdAsync(userId);
            if (school is null)
            {
                throw new ArgumentException("School doesn't exist");
            }

            school.ImageUrl = imagePath;
            await UpdateSchoolAsync(school);
        }

        public async Task UpdateSchoolAsync(SchoolManageViewModel model)
        {
            var school = await repo.All<School>().Where(s=>s.Id == model.Id).FirstOrDefaultAsync();
            if (school is null)
            {
                throw new ArgumentException("School doesn't exist");
            }

            try
            {
                // Get the uploaded file from the request
                var file = model.ImageFile;
                var fileExtension = Path.GetExtension(file?.FileName);

                // Check if a file was uploaded
                if ((file is not null || file?.Length != 0) && (fileExtension == ".jpg" || fileExtension == ".png"))
                {
                    // Create a unique file name to save the uploaded image
                    var fileName = Guid.NewGuid().ToString() + fileExtension;

                    // Specify the directory where the image will be saved
                    var imagePath = Path.Combine(env.WebRootPath, "images", "user-images", fileName);

                    // Save the file to the specified path
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file!.CopyToAsync(stream);
                    }

                    model.ImageUrl = $"images/user-images/{fileName}";
                }

                school.Location = model.Location;
                //school.Principal = model.Principal;
                school.Name = model.Name;
                school.Description = model.Description;
                school.ImageUrl = model.ImageUrl;

                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
