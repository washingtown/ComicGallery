using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicGallery.Common;
using ComicGallery.Models;
using ComicGallery.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace ComicGallery.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IComicService _comicService;
        private readonly IGalleryService _galleryService;
        private readonly IOptions<ApplicationSettings> _appOptions;

        public IndexModel(IComicService comicService, IGalleryService galleryService,
            IOptions<ApplicationSettings> options)
        {
            _comicService = comicService;
            _galleryService = galleryService;
            _appOptions = options;
        }

        public List<Gallery> Galleries { get; set; }

        public async Task OnGetAsync()
        {
            var galleries = await _galleryService.GetAllGalleries();
            foreach (var gallery in galleries)
            {
                gallery.Comics = await _comicService.GetComics(gallery, 0, 6);
            }
            Galleries = galleries;
        }
    }
}
