using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicGallery.Models;
using ComicGallery.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ComicGallery.Pages.Read
{
    public class ComicModel : PageModel
    {
        private readonly IComicService _comicService;
        private readonly IGalleryService _galleryService;
        public ComicModel(IComicService comicService, IGalleryService galleryService)
        {
            _comicService = comicService;
            _galleryService = galleryService;
        }

        public Comic Comic { get; set; }

        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!ModelState.IsValid)
                return NotFound();
            var comic = await _comicService.GetComic(Id);
            if (comic == null)
            {
                return NotFound();
            }
            Comic = comic;
            return Page();
        }
    }
}
