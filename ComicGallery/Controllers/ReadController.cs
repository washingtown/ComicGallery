using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ComicGallery.Common;
using ComicGallery.Models;
using ComicGallery.Services;
using ComicGallery.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace ComicGallery.Controllers
{
    public class ReadController : Controller
    {
        private readonly IComicService _comicService;
        private readonly IGalleryService _galleryService;
        private readonly IOptions<ApplicationSettings> _appOptions;

        public ReadController(IComicService comicService, IGalleryService galleryService,
            IOptions<ApplicationSettings> options)
        {
            _comicService = comicService;
            _galleryService = galleryService;
            _appOptions = options;
        }

        [Route("Image/{comicId}/{imageFilename}")]
        public async Task<IActionResult> Image(int comicId,string imageFilename)
        {
            Comic comic = await _comicService.GetComic(comicId);
            string filePath = comic.GetImageFullPath(imageFilename);
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists)
            {
                return NotFound();
            }
            else
            {
                var contentTypeProvider = new FileExtensionContentTypeProvider();
                string contentType = contentTypeProvider.Mappings[file.Extension];
                var stream = System.IO.File.OpenRead(filePath);
                return File(stream, contentType);
            }
        }
    }
}
