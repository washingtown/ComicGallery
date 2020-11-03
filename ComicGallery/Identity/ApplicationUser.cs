using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicGallery.Models;
using Microsoft.AspNetCore.Identity;

namespace ComicGallery.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public List<UserFavoriteComic> FavoriteComics { get; set; }
    }
}
