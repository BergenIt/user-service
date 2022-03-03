using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DatabaseExtension;

using Grpc.Core;

using UserService.Core.DataInterfaces;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис работы с настройками получателей уведомлений
    /// </summary>
    public class UserNotifySettingServices : UserNotifySettingService.UserNotifySettingServiceBase
    {
        private readonly IMapper _mapper;

        private readonly INotificationSettingManager<Core.Entity.UserNotificationSetting> _notifySettingManager;
        private readonly INotificationSettingGetter<Core.Entity.UserNotificationSetting> _notificationSettingGetter;

        /// <summary>
        /// Сервис работы с настройками получателей уведомлений
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="notifySettingManager"></param>
        /// <param name="notificationSettingGetter"></param>
        public UserNotifySettingServices(IMapper mapper, INotificationSettingManager<Core.Entity.UserNotificationSetting> notifySettingManager, INotificationSettingGetter<Core.Entity.UserNotificationSetting> notificationSettingGetter)
        {
            _mapper = mapper;
            _notifySettingManager = notifySettingManager;
            _notificationSettingGetter = notificationSettingGetter;
        }

        /// <summary>
        /// Получить настройку получателей по Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserNotifySetting> GetUserNotifySettingById(UserNotifySettingGetRequest request, ServerCallContext context)
        {
            Guid notifySettingId = Guid.Parse(request.Id);

            Core.Entity.UserNotificationSetting notifySetting = await _notificationSettingGetter.GetNotificationSetting(notifySettingId);

            return _mapper.Map<UserNotifySetting>(notifySetting);
        }

        /// <summary>
        /// Получить настройку получателей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserNotifySettingPage> GetUserNotifySettings(UserNotifySettingsGetRequest request, ServerCallContext context)
        {
            Guid contractProfileId = Guid.Parse(request.ContractProfileId);

            FilterContract filter = request.Filter.FromProtoFilter<UserNotifySetting, Core.Entity.UserNotificationSetting>();

            IPageItems<Core.Entity.UserNotificationSetting> notifySetting = await _notificationSettingGetter.GetNotificationSettings(contractProfileId, filter);

            return new()
            {
                NotifySettingList = { _mapper.Map<IEnumerable<UserNotifySetting>>(notifySetting.Items) },
                CountItems = (int)notifySetting.CountItems
            };
        }

        /// <summary>
        /// Создать новую настройку получателей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserNotifySettings> CreateUserNotifySettings(UserNotifySettingsCreateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.UserNotificationSetting> notifySetting = _mapper.Map<IEnumerable<Core.Entity.UserNotificationSetting>>(request.CreateNotifySettingsList);

            IEnumerable<Core.Entity.UserNotificationSetting> addedNotifySettings = await _notifySettingManager.AddNotificationSetting(notifySetting);

            return new()
            {
                NotifySettingList = { _mapper.Map<IEnumerable<UserNotifySetting>>(addedNotifySettings) }
            };
        }

        /// <summary>
        /// Обновить новую настройку получателей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserNotifySettings> UpdateUserNotifySettings(UserNotifySettingsUpdateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.UserNotificationSetting> notifySetting = _mapper.Map<IEnumerable<Core.Entity.UserNotificationSetting>>(request.UpdateNotifySettingsList);

            IEnumerable<Core.Entity.UserNotificationSetting> updatedNotifySettings = await _notifySettingManager.UpdateNotificationSetting(notifySetting);

            return new()
            {
                NotifySettingList = { _mapper.Map<IEnumerable<UserNotifySetting>>(updatedNotifySettings) }
            };
        }

        /// <summary>
        /// Удалить новую настройку получателей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserNotifySettings> RemoveUserNotifySettings(UserNotifySettingsRemoveCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> notifySettingIds = request.RemoveNotifySettingsId.Select(r => Guid.Parse(r));

            IEnumerable<Core.Entity.UserNotificationSetting> removedNotifySettings = await _notifySettingManager.RemoveNotificationSetting(notifySettingIds);

            return new()
            {
                NotifySettingList = { _mapper.Map<IEnumerable<UserNotifySetting>>(removedNotifySettings) }
            };
        }
    }
}
