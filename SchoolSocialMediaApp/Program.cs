using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Core.Services;
using SchoolSocialMediaApp.Data;
using SchoolSocialMediaApp.Infrastructure;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;

var builder = WebApplication.CreateBuilder(args);

//// <<----- Default Connection String settings ----->>
//string sqlServerName = Environment.GetEnvironmentVariable("SQL_SERVER_NAME");
//string sqlDbName = Environment.GetEnvironmentVariable("SQL_DB_NAME");
//string sqlPassword = Environment.GetEnvironmentVariable("SQL_SERVER_PASSWORD");

//// Replace the placeholder in the ConnectionStrings section
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
//    .Replace("{SQL_SERVER_NAME}", sqlServerName)
//    .Replace("{SQL_DB_NAME}", sqlDbName)
//    .Replace("{SQL_SERVER_PASSWORD}", sqlPassword);

// <<----- Developer Connection String settings ----->>
var connectionString = builder.Configuration.GetConnectionString("DevConnection");

// Add services to the container.
builder.Services.AddDbContext<SchoolSocialMediaDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<SchoolSocialMediaDbContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Teacher", policy => policy.RequireRole("Teacher"));
    options.AddPolicy("Student", policy => policy.RequireRole("Student"));
    options.AddPolicy("Parent", policy => policy.RequireRole("Parent"));
    options.AddPolicy("Principal", policy => policy.RequireRole("Principal", "Admin"));
    options.AddPolicy("CanBePrincipal", policy => policy.RequireRole("User"/*, "Parent", "Teacher", "Admin"*/));
    options.AddPolicy("IsPartOfSchoolButNotPrincipal", policy => policy.RequireRole("Parent", "Teacher", "Student"));
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddControllersWithViews().AddViewOptions(options =>
{
    options.HtmlHelperOptions.ClientValidationEnabled = true;
});
builder.Services.AddRazorPages();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<ISchoolClassService, SchoolClassService>();
builder.Services.AddScoped<DataSeeder>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // Get the DataSeeder instance and call the SeedData method
        var dataSeeder = services.GetRequiredService<DataSeeder>();
        await dataSeeder.SeedData();
    }
    catch (Exception ex)
    {
        // Handle any errors during seeding if needed
        Console.WriteLine("An error occurred while seeding the database: " + ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
    RequestPath = "/images"

});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "home-page")),
    RequestPath = "/images/home-page"

});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user-images")),
    RequestPath = "/images/user-images"

});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});
app.MapRazorPages();

app.Run();
