using DemoG03.BusinessLogic.Profiles;
using DemoG03.BusinessLogic.Services.Classes;
using DemoG03.BusinessLogic.Services.Interfaces;
using DemoG03.DataAccess.Data.Contexts;
using DemoG03.DataAccess.Repositories.Departments;
using DemoG03.DataAccess.Repositories.Employees;
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

            builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
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

            //app.UseAuthorization(); //01
            //app.UseAuthentication(); //02

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));
            #endregion

            app.Run();
        }
    }
}
