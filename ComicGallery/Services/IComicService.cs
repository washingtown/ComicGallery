using ComicGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicGallery.Services
{
    public interface IComicService
    {
        /// <summary>
        /// 获取指定ID的漫画
        /// </summary>
        /// <param name="id">漫画ID</param>
        Task<Comic> GetComic(int id);
        /// <summary>
        /// 获取指定路径的漫画
        /// </summary>
        /// <param name="path">漫画路径</param>
        /// <returns></returns>
        Task<Comic> GetComic(string path);
        /// <summary>
        /// 获取分页的漫画列表
        /// </summary>
        /// <param name="page">页码（从0开始）</param>
        /// <param name="pageSize">每页个数</param>
        /// <returns></returns>
        Task<List<Comic>> GetComics(int page, int pageSize);
        /// <summary>
        /// 获取某个分类分页的漫画列表
        /// </summary>
        /// <param name="galleryId">分类id</param>
        /// <param name="page">页码（从0开始）</param>
        /// <param name="pageSize">每页个数</param>
        /// <returns></returns>
        Task<List<Comic>> GetComics(int galleryId,int page, int pageSize);
        /// <summary>
        /// 获取某个分类分页的漫画列表
        /// </summary>
        /// <param name="gallery">分类</param>
        /// <param name="page">页码（从0开始）</param>
        /// <param name="pageSize">每页个数</param>
        /// <returns></returns>
        Task<List<Comic>> GetComics(Gallery gallery, int page, int pageSize);
        /// <summary>
        /// 获取漫画总数
        /// </summary>
        /// <returns></returns>
        Task<int> GetComicCount();
        /// <summary>
        /// 获取指定分类的漫画总数
        /// </summary>
        /// <param name="galleryId">分类id</param>
        /// <returns></returns>
        Task<int> GetComicCount(int galleryId);
        /// <summary>
        /// 获取指定分类的漫画总数
        /// </summary>
        /// <param name="gallery">分类</param>
        /// <returns></returns>
        Task<int> GetComicCount(Gallery gallery);

        Task<int> SaveChanges();

        Task AddComic(Comic comic);
        Task AddComics(IEnumerable<Comic> comics);

    }
}
