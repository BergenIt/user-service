using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.ContractConfigParser;
using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core.NotificationPackage.UserNotificationGetter
{
    public class UserNotificationGetter : IUserNotificationGetter
    {
        private readonly IUserGetter _userGetter;

        private readonly INotificationManager _notificationGetter;
        private readonly IContractProfileGetter _contractProfileWorker;

        private readonly IContractConfigParser _contractConfigParser;

        public UserNotificationGetter(IUserGetter userGetter, INotificationManager notificationGetter, IContractProfileGetter contractProfileWorker, IContractConfigParser contractConfigParser)
        {
            _userGetter = userGetter;
            _notificationGetter = notificationGetter;
            _contractProfileWorker = contractProfileWorker;
            _contractConfigParser = contractConfigParser;
        }

        public async Task<IPageItems<UserNotificationRecord>> GetUserNotitication(FilterContract filterContract, string userName)
        {
            List<UserNotificationRecord> result = new();

            Guid userId = await _userGetter.GetUserId(userName);

            IPageItems<Notification> notifications = await _notificationGetter.GetUserNotifications(filterContract, userName);

            IEnumerable<ContractProfile> contractProfiles = await _contractProfileWorker.GetUserContractProfiles(userId);

            foreach (Notification notification in notifications.Items)
            {
                IEnumerable<ContractProfile> contractProfilesEvent = contractProfiles.Where(c => c.NotifyEventType == notification.NotifyEventType);

                if (!contractProfilesEvent.Any())
                {
                    continue;
                }

                IDictionary<Guid, IEnumerable<KeyValuePair<string, string>>> rawMessage = _contractConfigParser.BuildRawStringArray(contractProfilesEvent, notification);

                IDictionary<Guid, string> messages = _contractConfigParser.GetMessageFromContractProfiles(rawMessage, WebHookContractType.StringArray);

                UserNotification userNotification = notification.UserNotifications.Single();

                IEnumerable<UserNotificationRecord> notificationRecords = messages
                    .Select(m => new UserNotificationRecord(
                        userNotification.Id,
                        notification.NotifyEventType,
                        notification.Timestamp,
                        m.Value,
                        notification.ObjectId,
                        userNotification.IsRead
                    )
                );

                result.AddRange(notificationRecords);
            }

            return new PageItems<UserNotificationRecord>(result, notifications.CountItems);
        }
    }
}
