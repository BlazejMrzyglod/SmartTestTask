using Microsoft.AspNetCore.Identity;
using OutOfOffice.MapperProfiles;
using OutOfOffice.Models.Models;
using OutOfOffice.Services.Data;
using OutOfOffice.Services.Repository;
using OutOfOffice.Services.Repository.EntityFramework;

namespace OutOfOffice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            _ = builder.Services.AddDbContext<ApplicationDbContext>();
            _ = builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            _ = builder.Services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            _ = builder.Services.AddControllersWithViews();

            _ = builder.Services.AddScoped(typeof(IRepositoryService<>), typeof(RepositoryService<>));

            _ = builder.Services.AddAutoMapper(typeof(EmployeesProfile), typeof(ApprovalRequestsProfile), typeof(LeaveRequestsProfile), typeof(ProjectsProfile));

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                _ = app.UseMigrationsEndPoint();
            }
            else
            {
                _ = app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                _ = app.UseHsts();
            }

            _ = app.UseHttpsRedirection();
            _ = app.UseStaticFiles();

            _ = app.UseRouting();

            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            _ = app.MapRazorPages();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                UserManager<ApplicationUser>? userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                RoleManager<IdentityRole>? roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                if (roleManager != null)
                {
                    if (!roleManager.RoleExistsAsync("Employee").Result)
                    {
                        roleManager.CreateAsync(new IdentityRole()
                        {
                            Name = "Employee"
                        }).Wait();
                    }

                    if (!roleManager.RoleExistsAsync("HR_Manager").Result)
                    {
                        roleManager.CreateAsync(new IdentityRole()
                        {
                            Name = "HR_Manager"
                        }).Wait();
                    }

                    if (!roleManager.RoleExistsAsync("ProjectManager").Result)
                    {
                        roleManager.CreateAsync(new IdentityRole()
                        {
                            Name = "ProjectManager"
                        }).Wait();
                    }

                    if (!roleManager.RoleExistsAsync("Administrator").Result)
                    {
                        roleManager.CreateAsync(new IdentityRole()
                        {
                            Name = "Administrator"
                        }).Wait();
                    }
                }

                if (userManager != null)
                {
                    _ = userManager.CreateAsync(
                                 new ApplicationUser()
                                 {
                                     UserName = "JohnDoe@smart.eu",
                                     Email = "JohnDoe@smart.eu",
                                     LockoutEnabled = false,
                                     AccessFailedCount = 0,
                                     EmployeeId = 1
                                 }, "Az123456$").Result;
                    ApplicationUser? user = userManager.FindByNameAsync("JohnDoe@smart.eu").Result;
                    if (user != null)
                        userManager.AddToRoleAsync(user, "HR_Manager").Wait();

                    _ = userManager.CreateAsync(
                                 new ApplicationUser()
                                 {
                                     UserName = "DianaLynch@smart.eu",
                                     Email = "DianaLynch@smart.eu",
                                     LockoutEnabled = false,
                                     AccessFailedCount = 0,
                                     EmployeeId = 2
                                 }, "Az123456$").Result;
                    user = userManager.FindByNameAsync("DianaLynch@smart.eu").Result;
                    if (user != null)
                        userManager.AddToRoleAsync(user, "HR_Manager").Wait();

                    _ = userManager.CreateAsync(
                                 new ApplicationUser()
                                 {
                                     UserName = "HarrisonTaylor@smart.eu",
                                     Email = "HarrisonTaylor@smart.eu",
                                     LockoutEnabled = false,
                                     AccessFailedCount = 0,
                                     EmployeeId = 3
                                 }, "Az123456$").Result;
                    user = userManager.FindByNameAsync("HarrisonTaylor@smart.eu").Result;
                    if (user != null)
                        userManager.AddToRoleAsync(user, "Employee").Wait();

                    _ = userManager.CreateAsync(
                                 new ApplicationUser()
                                 {
                                     UserName = "DeborahMorris@smart.eu",
                                     Email = "DeborahMorris@smart.eu",
                                     LockoutEnabled = false,
                                     AccessFailedCount = 0,
                                     EmployeeId = 4
                                 }, "Az123456$").Result;
                    user = userManager.FindByNameAsync("DeborahMorris@smart.eu").Result;
                    if (user != null)
                        userManager.AddToRoleAsync(user, "ProjectManager").Wait();

                    _ = userManager.CreateAsync(
                                 new ApplicationUser()
                                 {
                                     UserName = "IlaSalazar@smart.eu",
                                     Email = "IlaSalazar@smart.eu",
                                     LockoutEnabled = false,
                                     AccessFailedCount = 0,
                                     EmployeeId = 5
                                 }, "Az123456$").Result;
                    user = userManager.FindByNameAsync("IlaSalazar@smart.eu").Result;
                    if (user != null)
                        userManager.AddToRoleAsync(user, "Administrator").Wait();

                }
            }

            app.Run();


        }

    }
}
