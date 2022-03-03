namespace DatabaseExtension
{
    /// <summary>
    /// Запрос на пагинацию
    /// </summary>
    public class PaginationFilter
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
