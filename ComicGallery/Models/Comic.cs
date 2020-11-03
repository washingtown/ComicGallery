using ComicGallery.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ComicGallery.Models
{
    public class Comic
    {
       

        public int Id { get; set; }
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public string DirectoryPath { get; set; }
        public int PageCount { get; set; }
        public DateTime AddTime { get; set; }
        public int GalleryId { get; set; }
        public Gallery Gallery { get; set; }

        [NotMapped]
        public static string[] ImageFileExt = { "jpg", "png", "bmp", "gif" };
        
        [NotMapped]
        public string[] ImageFiles 
        {
            get
            {
                IComparer<string> fileNameComparer = new FilesNameComparerClass();
                return GetImageFiles(DirectoryPath)
                    .OrderBy(f => f.Name, fileNameComparer)
                    .Select(f => f.Name)
                    .ToArray(); ;
            }
        }
        public static IEnumerable<FileInfo> GetImageFiles(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            return ImageFileExt.Select(s => "*." + s)
                .SelectMany(s => directory.GetFiles(s));
        } 


        public string GetImageFullPath(string imageFilename)
        {
            return Path.Join(DirectoryPath, imageFilename);
        }
    }
}
