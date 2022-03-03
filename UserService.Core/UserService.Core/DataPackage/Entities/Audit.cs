using System;
using System.Collections.Generic;

using Nest;

using UserService.Core.Models;

namespace UserService.Core.Entity
{

    [ElasticsearchType(RelationName = nameof(Audit))]
    public class Audit : BaseEntity, ITimeseriesEntity
    {
        public Audit(AuditCreateCommand auditCreateCommand)
        {
            Message = auditCreateCommand.Message;
            Action = auditCreateCommand.Action;

            Timestamp = DateTime.UtcNow;

            Roles = Array.Empty<string>();
        }

        public Audit() { }

        [Text(Fielddata = true)]
        public string IpAddress { get; set; }

        [Date(Name = "@timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [Text(Fielddata = true)]
        public string Message { get; set; }

        [Text(Fielddata = true)]
        public string Action { get; set; }

        [Text(Fielddata = true)]
        public string FullName { get; set; }

        [Text(Fielddata = true)]
        public string UserName { get; set; }

        [Text(Fielddata = true)]
        public string Subdivision { get; set; }

        [Text(Fielddata = true)]
        public string Position { get; set; }

        [Text(Fielddata = true)]
        public IEnumerable<string> Roles { get; set; }

        [Ignore]
        public override string AuditName => base.AuditName;

        [Ignore]
        public string IndexKey { get; set; }
    }
}
