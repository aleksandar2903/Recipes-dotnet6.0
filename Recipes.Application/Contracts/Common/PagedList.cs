using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Application.Contracts.Common
{
    /// <summary>
    /// Represents the paged list.
    /// </summary>
    /// <typeparam name="T">The type of the response.</typeparam>
    public sealed class PagedList<T>
    {
        private const int MaxPageSize = 20;
        private const int MinPageNumberAndSize = 1;
        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public IReadOnlyCollection<T> Results { get; }
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Results = items;
        }
        public static async Task<PagedList<T>> ToPagedList(
            IQueryable<T> source, 
            int pageNumber, 
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if(pageNumber <= MinPageNumberAndSize)
                pageNumber = MinPageNumberAndSize;
            if(pageSize is < MinPageNumberAndSize or > MaxPageSize)
                pageSize = MaxPageSize;

            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
