using Microsoft.AspNetCore.Identity;
using OutOfOffice.Services.Data;
using OutOfOffice.Services.Repository.EntityFramework;
using OutOfOffice.Services.Repository;
using OutOfOffice.Models;

namespace OutOfOffice
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<ApplicationDbContext>();
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<ApplicationUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddControllersWithViews();

			builder.Services.AddScoped(typeof(IRepositoryService<>), typeof(RepositoryService<>));

			var app = builder.Build();

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

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			using (var scope = app.Services.CreateScope())
			{
				var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
				var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

				if (roleManager != null)
				{
					if (!roleManager.RoleExistsAsync("Employee").Result)
						roleManager.CreateAsync(new IdentityRole()
						{
							Name = "Employee"
						}).Wait();
					if (!roleManager.RoleExistsAsync("HR_Manager").Result)
						roleManager.CreateAsync(new IdentityRole()
						{
							Name = "HR_Manager"
						}).Wait();
					if (!roleManager.RoleExistsAsync("ProjectManager").Result)
						roleManager.CreateAsync(new IdentityRole()
						{
							Name = "ProjectManager"
						}).Wait();
					if (!roleManager.RoleExistsAsync("Administrator").Result)
						roleManager.CreateAsync(new IdentityRole()
						{
							Name = "Administrator"
						}).Wait();
				}

				if (userManager != null)
				{
					var createResult = userManager.CreateAsync(
								 new ApplicationUser()
								 {
									 UserName = "JohnDoe@smart.eu",
									 Email = "JohnDoe@smart.eu",
									 LockoutEnabled = false,
									 AccessFailedCount = 0,
									 EmployeeId = 1
								 }, "Az123456$").Result;
					var user = userManager.FindByNameAsync("JohnDoe@smart.eu").Result;
					userManager.AddToRoleAsync(user, "HR_Manager").Wait();

					createResult = userManager.CreateAsync(
								 new ApplicationUser()
								 {
									 UserName = "DianaLynch@smart.eu",
									 Email = "DianaLynch@smart.eu",
									 LockoutEnabled = false,
									 AccessFailedCount = 0,
									 EmployeeId = 2
								 }, "Az123456$").Result;
					user = userManager.FindByNameAsync("DianaLynch@smart.eu").Result;
					userManager.AddToRoleAsync(user, "HR_Manager").Wait();

					createResult = userManager.CreateAsync(
								 new ApplicationUser()
								 {
									 UserName = "HarrisonTaylor@smart.eu",
									 Email = "HarrisonTaylor@smart.eu",
									 LockoutEnabled = false,
									 AccessFailedCount = 0,
									 EmployeeId = 3
								 }, "Az123456$").Result;
					user = userManager.FindByNameAsync("HarrisonTaylor@smart.eu").Result;
					userManager.AddToRoleAsync(user, "Employee").Wait();

					createResult = userManager.CreateAsync(
								 new ApplicationUser()
								 {
									 UserName = "DeborahMorris@smart.eu",
									 Email = "DeborahMorris@smart.eu",
									 LockoutEnabled = false,
									 AccessFailedCount = 0,
									 EmployeeId = 4
								 }, "Az123456$").Result;
					user = userManager.FindByNameAsync("DeborahMorris@smart.eu").Result;
					userManager.AddToRoleAsync(user, "ProjectManager").Wait();

					createResult = userManager.CreateAsync(
								 new ApplicationUser()
								 {
									 UserName = "IlaSalazar@smart.eu",
									 Email = "IlaSalazar@smart.eu",
									 LockoutEnabled = false,
									 AccessFailedCount = 0,
									 EmployeeId = 5
								 }, "Az123456$").Result;
					user = userManager.FindByNameAsync("IlaSalazar@smart.eu").Result;
					userManager.AddToRoleAsync(user, "Administrator").Wait();

				}
			}

			app.Run();

			
		}

	}
}
