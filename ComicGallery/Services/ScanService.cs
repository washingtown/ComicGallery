using ComicGallery.Common;
using ComicGallery.Hubs;
using ComicGallery.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ComicGallery.Services
{
    public class ScanService
    {
        private readonly IOptions<ApplicationSettings> _options;
        private readonly IHubContext<ScanHub> _hubContext;
        private readonly IGalleryService _galleryService;
        private readonly IComicService _comicService;
        public ScanService(
            IOptions<ApplicationSettings> options, 
            IHubContext<ScanHub> hubContext,
            IGalleryService galleryService,
            IComicService comicService
            )
        {
            _options = options;
            _hubContext = hubContext;
            _galleryService = galleryService;
            _comicService = comicService;
        }
        public static string ScanMethodName { get; set; } = "ScanMessage";
        public static string ScanStatusName { get; set; } = "Scanning";
        public bool Scanning { get; set; } = false;

        public async Task SendMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync(ScanMethodName, message);
        }
        public async Task SendScanStatus()
        {
            await _hubContext.Clients.All.SendAsync(ScanStatusName, Scanning);
        }
        public async Task Scan()
        {
            Scanning = true;
            await SendScanStatus();
            DirectoryInfo rootDir = new DirectoryInfo(_options.Value.RootDir);
            List<Gallery> galleries =  await _galleryService.GetAllGalleries();
            int maxIndex = 0;
            if (galleries.Count() != 0) {
                maxIndex = galleries.Select(g => g.Index).OrderByDescending(i => i).FirstOrDefault() + 1;
            }
            List<Gallery> newGalleries = new List<Gallery>();
            await SendMessage("开始扫描");
            foreach (var galleryDir in rootDir.GetDirectories())
            {
                if (galleryDir.Name.StartsWith(".") || galleryDir.Name.StartsWith("@"))
                    continue;
                if(await _galleryService.GetGallery(galleryDir.FullName) == null)
                {
                    Gallery newGallery = new Gallery
                    {
                        Name = galleryDir.Name,
                        Index = maxIndex,
                        DefaultDir = galleryDir.FullName
                    };
                    maxIndex++;
                    await SendMessage($"新增分类：{newGallery.Name}");
                    await _galleryService.AddGallery(newGallery);
                    newGalleries.Add(newGallery);
                }
            }
            galleries.AddRange(newGalleries);
            int newComicCount = 0;
            foreach (var gallery in galleries)
            {
                DirectoryInfo dir = new DirectoryInfo(gallery.DefaultDir);
                newComicCount += await scanComic(dir, gallery.Id);
            }
            await SendMessage($"扫描完毕，共增加{newGalleries.Count}个分类，{newComicCount}个漫画");
            Scanning = false;
            await SendScanStatus();
        }

        private async Task<int> scanComic(DirectoryInfo directory,int galleryId)
        {
            int addCount = 0;
            foreach (var subDir in directory.GetDirectories())
            {
                if (subDir.Name.StartsWith(".") || subDir.Name.StartsWith("@"))
                    continue;
                var images = Comic.GetImageFiles(subDir.FullName);
                int imagesCount = images.Count();
                if (imagesCount > 0 && await _comicService.GetComic(subDir.FullName)==null)
                {
                    Comic comic = new Comic
                    {
                        Title = subDir.Name,
                        CoverImage = images.FirstOrDefault().Name,
                        DirectoryPath = subDir.FullName,
                        PageCount = imagesCount,
                        AddTime = subDir.CreationTime,
                        GalleryId = galleryId
                    };
                    await SendMessage($"新增漫画 {comic.Title}");
                    await _comicService.AddComic(comic);
                    addCount++;
                }
                addCount += await scanComic(subDir, galleryId);
            }
            return addCount;
        }
    }
}
