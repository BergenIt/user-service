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
    public class RoleNotifySettingServices : RoleNotifySettingService.RoleNotifySettingServiceBase
    {
        private readonly IMapper _mapper;

        private readonly INotificationSettingManager<Core.Entity.RoleNotificationSetting> _notifySettingManager;
        private readonly INotificationSettingGetter<Core.Entity.RoleNotificationSetting> _notificationSettingGetter;

        /// <summary>
        /// Сервис работы с настройками получателей уведомлений
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="notifySettingManager"></param>
        /// <param name="notificationSettingGetter"></param>
        public RoleNotifySettingServices(IMapper mapper, INotificationSettingManager<Core.Entity.RoleNotificationSetting> notifySettingManager, INotificationSettingGetter<Core.Entity.RoleNotificationSetting> notificationSettingGetter)
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
        public override async Task<RoleNotifySetting> GetRoleNotifySettingById(RoleNotifySettingGetRequest request, ServerCallContext context)
        {
            Guid notifySettingId = Guid.Parse(request.Id);

            Core.Entity.RoleNotificationSetting notifySetting = await _notificationSettingGetter.GetNotificationSetting(notifySettingId);

            return _mapper.Map<RoleNotifySetting>(notifySetting);
        }

        /// <summary>
        /// Получить настройку получателей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RoleNotifySettingPage> GetRoleNotifySettings(RoleNotifySettingsGetRequest request, ServerCallContext context)
        {
            Guid contractProfileId = Guid.Parse(request.ContractProfileId);

            FilterContract filter = request.Filter.FromProtoFilter<RoleNotifySetting, Core.Entity.RoleNotificationSetting>();

            IPageItems<Core.Entity.RoleNotificationSetting> notifySetting = await _notificationSettingGetter.GetNotificationSettings(contractProfileId, filter);

            return new()
            {
                NotifySettingList = { _mapper.Map<IEnumerable<RoleNotifySetting>>(notifySetting.Items) },
                CountItems = (int)notifySetting.CountItems
            };
        }

        /// <summary>
        /// Создать новую настройку получателей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RoleNotifySettings> CreateRoleNotifySettings(RoleNotifySettingsCreateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.RoleNotificationSetting> notifySettings = _mapper.Map<IEnumerable<Core.Entity.RoleNotificationSetting>>(request.CreateNotifySettingsList);

            IEnumerable<Core.Entity.RoleNotificationSetting> addedNotifySettings = await _notifySettingManager.AddNotificationSetting(notifySettings);

            return new()
            {
                NotifySettingList = { _mapper.Map<IEnumerable<RoleNotifySetting>>(addedNotifySettings) }
            };
        }

        /// <summary>
        /// Обновить новую настройку получателей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RoleNotifySettings> UpdateRoleNotifySettings(RoleNotifySettingsUpdateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.RoleNotificationSetting> notifySetting = _mapper.Map<IEnumerable<Core.Entity.RoleNotificationSetting>>(request.UpdateNotifySettingsList);

            IEnumerable<Core.Entity.RoleNotificationSetting> updatedNotifySettings = await _notifySettingManager.UpdateNotificationSetting(notifySetting);

            return new()
            {
                NotifySettingList = { _mapper.Map<IEnumerable<RoleNotifySetting>>(updatedNotifySettings) }
            };
        }

        /// <summary>
        /// Удалить новую настройку получателей
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RoleNotifySettings> RemoveRoleNotifySettings(RoleNotifySettingsRemoveCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> notifySettingIds = request.RemoveNotifySettingsId.Select(r => Guid.Parse(r));

            IEnumerable<Core.Entity.RoleNotificationSetting> removedNotifySettings = await _notifySettingManager.RemoveNotificationSetting(notifySettingIds);

            return new()
            {
                NotifySettingList = { _mapper.Map<IEnumerable<RoleNotifySetting>>(removedNotifySettings) }
            };
        }
    }
}
