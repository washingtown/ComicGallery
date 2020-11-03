using ComicGallery.Models;
using ComicGallery.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ComicGallery.Services
{
    public class ComicService : IComicService
    {
        private readonly ApplicationDbContext _dbContext;

        public ComicService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取指定ID的漫画
        /// </summary>
        /// <param name="id">漫画ID</param>
        /// <returns></returns>
        public async Task<Comic> GetComic(int id)
        {
            return await _dbContext.Comics.FindAsync(id);
        }
        /// <summary>
        /// 获取指定路径的漫画
        /// </summary>
        /// <param name="path">漫画路径</param>
        /// <returns></returns>
        public async Task<Comic> GetComic(string path)
        {
            return await _dbContext.Comics.Where(c => c.DirectoryPath == path).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取漫画总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetComicCount()
        {
            return await _dbContext.Comics.CountAsync();
        }
        /// <summary>
        /// 获取指定分类的漫画总数
        /// </summary>
        /// <param name="galleryId">分类id</param>
        /// <returns></returns>
        public async Task<int> GetComicCount(int galleryId)
        {
            return await _dbContext.Comics.Where(c => c.GalleryId == galleryId).CountAsync();
        }
        /// <summary>
        /// 获取指定分类的漫画总数
        /// </summary>
        /// <param name="gallery">分类</param>
        /// <returns></returns>
        public async Task<int> GetComicCount(Gallery gallery)
        {
            return await GetComicCount(gallery.Id);
        }
        /// <summary>
        /// 获取某个分类分页的漫画列表
        /// </summary>
        /// <param name="galleryId">分类id</param>
        /// <param name="page">页码（从0开始）</param>
        /// <param name="pageSize">每页个数</param>
        /// <returns></returns>
        public async Task<List<Comic>> GetComics(int galleryId, int page, int pageSize)
        {
            return await _dbContext.Comics.Where(c => c.GalleryId == galleryId)
                .Paging(page, pageSize)
                .ToListAsync();
        }
        /// <summary>
        /// 获取某个分类分页的漫画列表
        /// </summary>
        /// <param name="gallery">分类</param>
        /// <param name="page">页码（从0开始）</param>
        /// <param name="pageSize">每页个数</param>
        /// <returns></returns>
        public async Task<List<Comic>> GetComics(Gallery gallery, int page, int pageSize)
        {
            return await GetComics(gallery.Id, page, pageSize);
        }
        /// <summary>
        /// 获取分页的漫画列表
        /// </summary>
        /// <param name="page">页码（从0开始）</param>
        /// <param name="pageSize">每页个数</param>
        /// <returns></returns>
        public async Task<List<Comic>> GetComics(int page, int pageSize)
        {
            return await _dbContext.Comics.Paging(page, pageSize).ToListAsync();
        }
        /// <summary>
        /// 保存改动
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }
        
        public async Task AddComic(Comic comic)
        {
            await _dbContext.Comics.AddAsync(comic);
            await SaveChanges();
        }

        public async Task AddComics(IEnumerable<Comic> comics)
        {
            await _dbContext.Comics.AddRangeAsync(comics);
            await SaveChanges();
        }
    }
}
