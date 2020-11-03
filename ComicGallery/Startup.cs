using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicGallery.Common;
using ComicGallery.Data;
using ComicGallery.Identity;
using ComicGallery.Services;
using ComicGallery.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace ComicGallery
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("ComicGalleryContextConnection"));
            });
            services.Configure<ApplicationInfos>(Configuration.GetSection("ApplicationInfos"));
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
             {
                 options.Password.RequireDigit = false;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireUppercase = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequiredLength = 1;
             }).AddEntityFrameworkStores<ApplicationDbContext>()
                //.AddDefaultUI()
                .AddDefaultTokenProviders();
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Login");
            services.AddMvc();
            services.AddRazorPages();
            //Add SignalR
            services.AddSignalR();
            services.AddScoped<IComicService, ComicService>();
            services.AddScoped<IGalleryService, GalleryService>();
            services.AddScoped<ScanService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Identity",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<ScanHub>("/scanhub");
            });
            
        }
    }
}
