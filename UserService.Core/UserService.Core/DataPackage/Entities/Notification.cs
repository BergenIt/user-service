using System;
using System.Collections.Generic;

using Nest;

namespace UserService.Core.Entity
{
    [ElasticsearchType(RelationName = nameof(Notification))]
    public class Notification : NotifyRelation, ITimeseriesEntity, IParent<UserNotification>
    {
        public Notification() => UserNotifications = new HashSet<UserNotification>();


        [Text(Fielddata = true)]
        public string ObjectId { get; set; }

        [Text(Fielddata = true)]
        public string NotifyEventType { get; set; }

        [Date(Name = "@timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [Nested]
        public IDictionary<string, string> JsonData { get; set; }

        [Ignore]
        public string IndexKey => NotifyEventType;

        [Ignore]
        public IEnumerable<UserNotification> UserNotifications { get; set; }

        [Ignore]
        IEnumerable<UserNotification> IParent<UserNotification>.Childs { get => UserNotifications; set => UserNotifications = value; }
    }
}

