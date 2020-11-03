using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicGallery.Models;
using ComicGallery.Services;
using ComicGallery.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ComicGallery.Pages.Read
{
    public class GalleryModel : PageModel
    {
        private readonly IComicService _comicService;
        private readonly IGalleryService _galleryService;

        public GalleryModel(IComicService comicService, IGalleryService galleryService)
        {
            _comicService = comicService;
            _galleryService = galleryService;
        }

        public string GalleryName { get; set; }
        public List<Comic> Comics { get; set; }
        public PaginationModel Pagination { get; set; }

        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }

        [BindProperty(Name="p", SupportsGet =true)]
        public int? PageNumber { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Gallery gallery = await _galleryService.GetGallery(Id);
            if (gallery == null)
                return NotFound();
            int pageSize = 12; //每页展示的漫画个数
            int comicCount = await _comicService.GetComicCount(Id);
            int maxPage = (comicCount + 1) / 12 + 1;

            GalleryName = gallery.Name;
            Console.WriteLine(PageNumber);
            Comics = await _comicService.GetComics(gallery, PageNumber.GetValueOrDefault(1) - 1, pageSize);
            Pagination = new PaginationModel()
            {
                StartPage = 1,
                EndPage = maxPage,
                CurrentPage = PageNumber.GetValueOrDefault(1),
            };
            Pagination.CreatePaginationItems();
            Pagination.Route = "/Read/Gallery1";
            Pagination.RouteParams = new Dictionary<string, string>();
            //{
            //    {"id",Id.ToString() },
            //};
            foreach (var kv in RouteData.Values)
            {
                Pagination.RouteParams.Add(kv.Key, kv.Value.ToString());
            }
            return Page();
        }
    }
}
