using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ComicGallery.Identity;
using ComicGallery.Models;

namespace ComicGallery.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }
        public DbSet<Comic> Comics { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<UserFavoriteComic> UserFavoriteComics { get; set; }
    }
}
