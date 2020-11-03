using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicGallery.Services
{
    public static class QueryExtensions
    {
        /// <summary>
        /// 对EF查询进行分页处理
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="query">实体查询对象</param>
        /// <param name="page">页码(从0开始)</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static IQueryable<T> Paging<T>(this IQueryable<T> query,int page,int pageSize)
        {
            return query.Skip(page * pageSize).Take(pageSize);
        }
    }
}
