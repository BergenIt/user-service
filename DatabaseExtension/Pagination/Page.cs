using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseExtension
{
    /// <summary>
    /// Страница с запрашиваемыми объектами
    /// </summary>
    /// <typeparam name="T">Тип запрашеваемых объектов</typeparam>
    public class Page<T>
    {
        /// <summary>
        /// Страница с запрашиваемыми объектами
        /// </summary>
        public Page(IEnumerable<T> data, int countItems, int countPage, bool hasPreviously, bool hasNext)
        {
            Data = data;
            CountItems = countItems;
            CountPage = countPage;
            HasPreviously = hasPreviously;
            HasNext = hasNext;
        }

        /// <summary>
        /// Страница с запрашиваемыми объектами
        /// </summary>
        public Page(Page<T> page)
        {
            Data = page.Data;
            CountItems = page.CountItems;
            CountPage = page.CountPage;
            HasPreviously = page.HasPreviously;
            HasNext = page.HasNext;
        }

        /// <summary>
        /// Страница с запрашиваемыми объектами
        /// </summary>
        /// <param name="data">Запрашиваемые объекты</param>
        /// <param name="countItems">Общее количество объектов</param>
        /// <param name="pagination">Количество страниц</param>
        public Page(IEnumerable<T> data, int countItems, PaginationFilter pagination)
        {
            Data = data;
            CountItems = countItems;

            if (pagination.PageSize is not null && pagination.PageNumber is not null)
            {
                float countPageRaw = countItems / (float)pagination.PageSize;
                CountPage = (int)Math.Ceiling(countPageRaw) + (countPageRaw * 10 % 5 == 0 && countPageRaw % 10 != 0 ? 1 : 0);
                HasNext = pagination.PageNumber != CountPage && data.Any();
                HasPreviously = pagination.PageNumber != 1 && CountPage != 0;
            }
        }

        /// <summary>
        /// Запрашеваемые объекты
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Количество объектов
        /// </summary>
        public int CountItems { get; set; }

        /// <summary>
        /// Количество страниц
        /// </summary>
        public int CountPage { get; set; }

        /// <summary>
        /// Можно ли перейти на следующую страницу
        /// </summary>
        public bool HasNext { get; set; }

        /// <summary>
        /// Можно ли перейти на предыдущую страницу
        /// </summary>
        public bool HasPreviously { get; set; }

        /// <summary>
        /// Конвертация в страницу объектов для использования в csv
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourcePage"></param>
        /// <returns></returns>
        public Page<object> ToObjectPage()
        {
            return new(Data.Select(d => (object)d), CountItems, CountPage, HasPreviously, HasNext);
        }
    }
}
