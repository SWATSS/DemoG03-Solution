using DemoG03.BusinessLogic.Profiles;
using DemoG03.BusinessLogic.Services.Classes;
using DemoG03.BusinessLogic.Services.Interfaces;
using DemoG03.DataAccess.Data.Contexts;
using DemoG03.DataAccess.Models.IdentityModels;
using DemoG03.DataAccess.Repositories.Departments;
using DemoG03.DataAccess.Repositories.Employees;
using DemoG03.DataAccess.Repositories.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoG03.PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            //builder.Services.AddScoped<ApplicationDbContext>();// We Dont Use This In DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                //options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies();
            });
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredUniqueChars = 3;

                    options.User.RequireUniqueEmail = true;
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(3);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                }).AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddDefaultTokenProviders(); // Token Service
            ;
            #endregion

            var app = builder.Build();

            #region Configure the HTTP request pipeline.
            // 
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); //01
            app.UseAuthorization(); //02

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));
            #endregion

            app.Run();
        }
    }
}
