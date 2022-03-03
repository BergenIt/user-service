using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DatabaseExtension;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using UserService.Core.DataInterfaces;
using UserService.Core.JwtGenerator;
using UserService.Core.NotificationPackage;
using UserService.Core.NotificationPackage.UserNotificationGetter;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис работы с уведомлениями
    /// </summary>
    public class NotificationServices : NotificationService.NotificationServiceBase
    {
        private readonly IMapper _mapper;

        private readonly IUserNotificationGetter _userNotificationGetter;
        private readonly INotificationManager _notificationManager;

        private readonly INotifyEventHandler _notifyEventHandler;

        /// <summary>
        /// Сервис работы с уведомлениями
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userNotificationGetter"></param>
        /// <param name="notificationManager"></param>
        /// <param name="notifyEventHandler"></param>
        public NotificationServices(IMapper mapper, IUserNotificationGetter userNotificationGetter, INotificationManager notificationManager, INotifyEventHandler notifyEventHandler)
        {
            _mapper = mapper;
            _userNotificationGetter = userNotificationGetter;
            _notificationManager = notificationManager;
            _notifyEventHandler = notifyEventHandler;
        }

        /// <summary>
        /// Создать новое уведомление
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Empty> CreateNotification(CreateNotificationCommand request, ServerCallContext context)
        {
            await _notifyEventHandler.NotifyEventHandle(new Core.Entity.Notification
            {
                NotifyEventType = request.NotifyEventType,
                ObjectId = request.ObjectId,
                Timestamp = DateTime.UtcNow,
                JsonData = request.JsonData
            });

            return new();
        }

        /// <summary>
        /// Получить уведомления текущего пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Notifications> GetUserNotifications(UserNotificationGetRequest request, ServerCallContext context)
        {
            string userName = context.RequestHeaders.Get(JwtGenerator.UserName).Value;

            FilterContract filterContract = request.Filter.FromProtoFilter<Notifications, Core.Entity.Notification>();

            IPageItems<UserNotificationRecord> notifications = await _userNotificationGetter.GetUserNotitication(filterContract, userName);

            return new()
            {
                NotificationCount = notifications.CountItems,
                NotificationList = { _mapper.Map<IEnumerable<Notification>>(notifications.Items) }
            };
        }

        /// <summary>
        /// Удалить уведомление текущего пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<Empty> DeleteUserNotifications(DeleteUserNotificationsCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> ids = request.UserNotificationIds.Select(n => Guid.Parse(n));

            return _notificationManager
                .RemoveUserNotifications(ids)
                .ContinueWith(_ => new Empty());
        }

        /// <summary>
        /// Отметить уведомление как прочитанное
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<Empty> ReadUserNotifications(ReadUserNotificationsCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> ids = request.UserNotificationIds.Select(n => Guid.Parse(n));

            return _notificationManager
                .ReadUserNotifications(ids)
                .ContinueWith(_ => new Empty());
        }
    }
}
