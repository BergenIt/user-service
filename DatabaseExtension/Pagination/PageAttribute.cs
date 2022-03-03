using System.Collections.Generic;

namespace DatabaseExtension
{
    /// <summary>
    /// Страница содержащая аттрибуты журнала
    /// </summary>
    /// <typeparam name="TContract">Тип контракта</typeparam>
    /// <typeparam name="TAttribute">Тип контракта атрибутов</typeparam>
    public class Page<TContract, TAttribute> : Page<TContract>
    {
        /// <summary>
        /// Страница с запрашиваемыми объектами
        /// </summary>
        public Page(Page<TContract> page, IEnumerable<TAttribute> attribute) : base(page)
        {
            Attributes = attribute;
        }

        /// <summary>
        /// Страница с запрашиваемыми объектами
        /// </summary>
        /// <param name="data"></param>
        /// <param name="attribute"></param>
        /// <param name="countItems"></param>
        /// <param name="pagination"></param>
        public Page(IEnumerable<TContract> data, IEnumerable<TAttribute> attribute, int countItems, PaginationFilter pagination) : base(data, countItems, pagination)
        {
            Attributes = attribute;
        }

        /// <summary>
        /// Аттрибуты страницы
        /// </summary>
        public IEnumerable<TAttribute> Attributes { get; set; }
    }
}
