using Clinic_system.Data;
using Clinic_system.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Clinic_system
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connString = builder.Configuration.GetConnectionString("Main");
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ClinicdbContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(connString);
            });
            builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddSession(a => { });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(a =>
                {
                    a.LoginPath = "/account/login";
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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
                name: "home",
                pattern: "{action=Index}",
                defaults: new { controller = "Home" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
