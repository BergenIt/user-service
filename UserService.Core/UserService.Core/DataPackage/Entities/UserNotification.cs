using System;

using Nest;

namespace UserService.Core.Entity
{
    [ElasticsearchType(RelationName = nameof(UserNotification))]
    public class UserNotification : NotifyRelation, IChild
    {
        [Text(Fielddata = true)]
        public string UserName { get; set; }

        [Boolean]
        public bool IsRead { get; set; }

        [Ignore]
        public Guid NotificationId { get; set; }
    }
}
