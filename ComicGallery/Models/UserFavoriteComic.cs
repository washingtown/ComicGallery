using ComicGallery.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicGallery.Models
{
    /// <summary>
    /// 用户收藏的漫画
    /// </summary>
    public class UserFavoriteComic
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Comic Comic { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
