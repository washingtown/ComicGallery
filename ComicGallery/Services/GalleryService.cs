using ComicGallery.Data;
using ComicGallery.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicGallery.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly ApplicationDbContext _dbContext;

        public GalleryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Gallery>> GetAllGalleries()
        {
            return await _dbContext.Galleries.ToListAsync();
        }

        public async Task<Gallery> GetGallery(int id)
        {
            return await _dbContext.Galleries.FindAsync(id);
        }

        public async Task<Gallery> GetGallery(string path)
        {
            return await _dbContext.Galleries.Where(g => g.DefaultDir == path).FirstOrDefaultAsync();
        }
        public async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public async Task AddGallery(Gallery gallery)
        {
            await _dbContext.Galleries.AddAsync(gallery);
            await SaveChanges();
        }

        public async Task AddGalleries(IEnumerable<Gallery> galleries)
        {
            await _dbContext.Galleries.AddRangeAsync(galleries);
            await SaveChanges();
        }
        
    }
}
