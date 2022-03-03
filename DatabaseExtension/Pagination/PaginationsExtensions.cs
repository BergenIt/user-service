using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DatabaseExtension
{
    public static class PaginationsExtensions
    {
        /// <summary>
        /// Применить пагинацию на запрос в Ef Core
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="paginationFilter"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> Paginations<TEntity>([NotNull] this IQueryable<TEntity> source, PaginationFilter paginationFilter) where TEntity : class
        {
            return paginationFilter is null || paginationFilter.PageNumber is null || paginationFilter.PageSize is null ? source :
                source.Skip((int)((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)).Take((int)paginationFilter.PageSize);
        }

        /// <summary>
        /// Применить пагинацию на перечисление
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="paginationFilter"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> Paginations<TEntity>([NotNull] this IEnumerable<TEntity> source, PaginationFilter paginationFilter) where TEntity : class
        {
            return paginationFilter is null || paginationFilter.PageNumber is null || paginationFilter.PageSize is null ? source :
                source.Skip((int)((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)).Take((int)paginationFilter.PageSize);
        }
    }
}
