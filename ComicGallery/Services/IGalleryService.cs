using ComicGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace ComicGallery.Services
{
    public interface IGalleryService
    {
        public Task<Gallery> GetGallery(int id);
        public Task<Gallery> GetGallery(string path);
        public Task<List<Gallery>> GetAllGalleries();
        public Task<int> SaveChanges();
        public Task AddGallery(Gallery gallery);
        public Task AddGalleries(IEnumerable<Gallery> galleries);
    }
}
