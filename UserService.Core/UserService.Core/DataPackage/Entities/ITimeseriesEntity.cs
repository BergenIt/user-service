using System;

namespace UserService.Core.Entity
{
    /// <summary>
    /// Интерфейс для временных рядов
    /// </summary>
    public interface ITimeseriesEntity : IBaseEntity
    {
        DateTimeOffset Timestamp { get; set; }

        string IndexKey { get; }
    }
}
