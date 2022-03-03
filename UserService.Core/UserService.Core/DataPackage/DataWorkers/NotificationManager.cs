using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.DataInterfaces;
using UserService.Core.Elasticsearch;
using UserService.Core.Entity;

using static Nest.JoinField;

namespace UserService.Core.DataPackage.DataWorkers
{
    public class NotificationManager : INotificationManager
    {
        private readonly IElasticsearchGetter _timeseriesGetter;
        private readonly IElasticsearchWorker _timeseriesWorker;

        public NotificationManager(IElasticsearchWorker timeseriesWorker, IElasticsearchGetter timeseriesGetter)
        {
            _timeseriesWorker = timeseriesWorker;
            _timeseriesGetter = timeseriesGetter;
        }

        public Task ReadUserNotifications(IEnumerable<Guid> userNotificationIds)
        {
            return _timeseriesWorker.UpdateAsync<UserNotification>(userNotificationIds, nameof(UserNotification.IsRead), $"{true}".ToLowerInvariant(), $"{nameof(Notification)}-*".ToLowerInvariant());
        }

        public Task RemoveUserNotifications(IEnumerable<Guid> userNotificationIds)
        {
            return _timeseriesWorker.RemoveAsync<UserNotification>(userNotificationIds, $"{nameof(Notification)}-*".ToLowerInvariant());
        }

        public Task<IPageItems<Notification>> GetUserNotifications(FilterContract filterContract, string userName)
        {
            return _timeseriesGetter.GetParentEntities<Notification, UserNotification, string>(
                u => u.UserName,
                userName,
                filterContract
            );
        }

        public async Task CreateNotification(Notification notification, IEnumerable<string> userNames)
        {
            notification.Id = Guid.NewGuid();
            notification.Relation = Root<Notification>();

            await _timeseriesWorker.InsertAsync(notification);

            IEnumerable<UserNotification> userNotifications = userNames.Select(u => new UserNotification
            {
                Id = Guid.NewGuid(),
                UserName = u,
                IsRead = false,
                Relation = Link<UserNotification>(notification.Id),
                NotificationId = notification.Id,
            });

            await _timeseriesWorker.InsertChildAsync<UserNotification, Notification>(notification.IndexKey, userNotifications);
        }
    }
}
