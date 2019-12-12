using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GsCore.Api.V1.Helpers
{
    /// <summary>
    /// Reusable PagedList
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class PagedList<T>:List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < TotalPages);

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages=(int)Math.Ceiling(count / (double) pageSize);
            AddRange(items);
        }
        /// <summary>
        /// Creates PagedList
        /// </summary>
        /// <param name="source">IQueryable of type entity</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>PagedList of type entity, Total record count, Page Number and Page Size</returns>
        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            return new PagedList<T>(items,count,pageNumber,pageSize);
        }

    }
}
