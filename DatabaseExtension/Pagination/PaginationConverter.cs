namespace DatabaseExtension
{
    public static class PaginationConverter
    {
        public static Proto.PaginationFilter ToProtoPagination(this PaginationFilter pagination)
        {
            if (pagination is null || pagination.PageNumber <= 0 || pagination.PageSize <= 0)
            {
                return new();
            }

            return new()
            {
                PageSize = pagination.PageSize,
                PageNumber = pagination.PageNumber
            };
        }

        public static PaginationFilter FromProtoPagination(this Proto.PaginationFilter paginationProto)
        {
            if (paginationProto is null || paginationProto.PageNumber <= 0 || paginationProto.PageSize <= 0)
            {
                return new();
            }

            return new()
            {
                PageNumber = paginationProto.PageNumber,
                PageSize = paginationProto.PageSize
            };
        }
    }
}
