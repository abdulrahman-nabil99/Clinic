using Clinic_system.Data;
using Clinic_system.Helpers;
using Clinic_system.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Clinic_system.Helpers.JobSchedule;
using Quartz;


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

            // Add Quartz services
            builder.Services.AddQuartz(q =>
            {
                // Use the Microsoft Dependency Injection (DI) job factory
                q.UseMicrosoftDependencyInjectionJobFactory();

                // Register the job with a unique key
                var jobKey = new JobKey("AppointmentReminderJob");
                q.AddJob<AppointmentReminderJob>(opts => opts.WithIdentity(jobKey));

                // Schedule the job to run daily at 8:00 AM
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("AppointmentReminderTrigger")
                    .WithCronSchedule("0 0 0 * * ?")); // Runs daily at 12:00 AM
                    //.WithCronSchedule("0 * * * * ?")); // Runs every minute
            });
            // Add Quartz hosted service for background jobs
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


            builder.Services.AddDbContext<ClinicdbContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(connString);
            });
            builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IServiceService, ServiceService>();
            builder.Services.AddScoped<RateLimiterHelper>();
            builder.Services.AddScoped<OtpHelper>();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60); 
                options.Cookie.HttpOnly = true; 
                options.Cookie.IsEssential = true; 
            });

            builder.Services.AddMemoryCache();
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
            app.UseSession();

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
