using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.NotificationPackage.ContractProfileValidator;

namespace UserService.Core.DataPackage.DataWorkers
{
    public class UserNotificationSettingManager : BasePackOperation, INotificationSettingManager<UserNotificationSetting>
    {
        private readonly INotificationSettingValidator _notificationSettingValidator;

        private readonly IDataWorker _dataWorker;
        private readonly IDataGetter _dataGetter;

        public UserNotificationSettingManager(INotificationSettingValidator notificationSettingValidator, IDataWorker dataWorker, IDataGetter dataGetter)
        {
            _notificationSettingValidator = notificationSettingValidator;
            _dataWorker = dataWorker;
            _dataGetter = dataGetter;
        }

        public async Task<UserNotificationSetting> RemoveNotificationSetting(Guid notificationSettingId)
        {
            UserNotificationSetting entity = await _dataWorker.RemoveAsync<UserNotificationSetting>(notificationSettingId);
            await _dataWorker.SaveChangesAsync();

            return entity;
        }

        public async Task<UserNotificationSetting> AddNotificationSetting(UserNotificationSetting notificationSetting)
        {
            await _notificationSettingValidator.Validate(notificationSetting, s => s.UserId == notificationSetting.UserId);

            notificationSetting.ContractProfile = null;
            notificationSetting.User = null;

            notificationSetting.Id = Guid.NewGuid();

            _ = await _dataWorker.AddAsync(notificationSetting);
            await _dataWorker.SaveChangesAsync();

            return notificationSetting;
        }

        public async Task<UserNotificationSetting> UpdateNotificationSetting(UserNotificationSetting notificationSetting)
        {
            UserNotificationSetting entity = await _dataGetter.GetSingleEntityAsync<UserNotificationSetting>(notificationSetting.Id);

            entity.Enable = notificationSetting.Enable;
            entity.TargetNotifies = notificationSetting.TargetNotifies;

            entity.UserId = notificationSetting.UserId;

            await _notificationSettingValidator.Validate(entity, s => s.UserId == notificationSetting.UserId);

            _ = _dataWorker.Update(entity);
            await _dataWorker.SaveChangesAsync();

            return entity;
        }

        public Task<IEnumerable<UserNotificationSetting>> UpdateNotificationSetting(IEnumerable<UserNotificationSetting> notificationSettings) =>
            EntityPackOperationAsync<UserNotificationSetting>(notificationSettings, UpdateNotificationSetting);

        public Task<IEnumerable<UserNotificationSetting>> AddNotificationSetting(IEnumerable<UserNotificationSetting> notificationSettings) =>
            EntityPackOperationAsync<UserNotificationSetting>(notificationSettings, AddNotificationSetting);

        public Task<IEnumerable<UserNotificationSetting>> RemoveNotificationSetting(IEnumerable<Guid> notificationSettingIds) =>
            EntityPackOperationAsync(notificationSettingIds, RemoveNotificationSetting);
    }
}
