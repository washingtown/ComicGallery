using ComicGallery.Common;
using ComicGallery.Data;
using ComicGallery.Identity;
using ComicGallery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ComicGallery
{
    public static class SeedData
    {
        public static string AdminRoleName = "Admin";

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using(ApplicationDbContext dbContext=new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()
                ))
            {
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
                IOptions<ApplicationSettings> option = serviceProvider.GetRequiredService<IOptions<ApplicationSettings>>();
                //if (!dbContext.Galleries.Any())
                //    InitGalleriesAndComics(option, dbContext);
                RoleManager<ApplicationRole> roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (!await roleManager.Roles.AnyAsync())
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = AdminRoleName,
                    });
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = "Reader",
                    });
                }
                if(!await userManager.Users.AnyAsync())
                {
                    ApplicationRole adminRole = await roleManager.FindByNameAsync(AdminRoleName);
                    string password = option.Value.DefaultPassword;
                    ApplicationUser adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                    };
                    await userManager.CreateAsync(adminUser, password);
                    await userManager.AddToRoleAsync(adminUser, AdminRoleName);
                }
            }
        }

        private static void InitGalleriesAndComics(IOptions<ApplicationSettings> option, ApplicationDbContext dbContext)
        {
            List<Gallery> galleries = new List<Gallery>();
            DirectoryInfo directoryInfo = new DirectoryInfo(option.Value.RootDir);
            var directories = directoryInfo.GetDirectories();
            foreach (var dir in directories)
            {
                Gallery gallery = new Gallery
                {
                    Name = dir.Name,
                    Index = directories.IndexOf(dir),
                    DefaultDir = dir.FullName,
                };
                gallery.Comics = GetComics(dir);
                galleries.Add(gallery);
            }
            dbContext.Galleries.AddRange(galleries);
            dbContext.SaveChanges();
        }

        private static List<Comic> GetComics(DirectoryInfo dir)
        {
            List<Comic> result = new List<Comic>();
            foreach (var subDir in dir.GetDirectories())
            {
                Comic comic = new Comic
                {
                    Title = subDir.Name,
                    DirectoryPath = subDir.FullName,
                    AddTime = subDir.CreationTime
                };
                comic.PageCount = comic.ImageFiles.Length;
                comic.CoverImage = comic.ImageFiles[0];
                result.Add(comic);
            }
            return result;
        }
    }
}
