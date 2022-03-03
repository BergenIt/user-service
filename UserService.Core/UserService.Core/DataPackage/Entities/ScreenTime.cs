using System;

using Nest;

namespace UserService.Core.Entity
{
    [ElasticsearchType(RelationName = nameof(ScreenTime))]
    public class ScreenTime : BaseEntity, ITimeseriesEntity
    {
        [Text(Fielddata = true)]
        public string UserName { get; set; }

        [Date(Name = "@timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [StringTimeSpan]
        public TimeSpan Duration { get; set; }

        [Ignore]
        public string IndexKey => string.Empty;
    }
}
