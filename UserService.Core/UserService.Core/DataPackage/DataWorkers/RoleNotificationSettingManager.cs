using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.NotificationPackage.ContractProfileValidator;

namespace UserService.Core.DataPackage.DataWorkers
{
    public class RoleNotificationSettingManager : BasePackOperation, INotificationSettingManager<RoleNotificationSetting>
    {
        private readonly INotificationSettingValidator _notificationSettingValidator;

        private readonly IDataWorker _dataWorker;
        private readonly IDataGetter _dataGetter;

        public RoleNotificationSettingManager(INotificationSettingValidator contractValidatorProvider, IDataWorker dataWorker, IDataGetter dataGetter)
        {
            _notificationSettingValidator = contractValidatorProvider;
            _dataWorker = dataWorker;
            _dataGetter = dataGetter;
        }

        public async Task<RoleNotificationSetting> RemoveNotificationSetting(Guid notificationSettingId)
        {
            RoleNotificationSetting entity = await _dataWorker.RemoveAsync<RoleNotificationSetting>(notificationSettingId);
            await _dataWorker.SaveChangesAsync();

            return entity;
        }

        public async Task<RoleNotificationSetting> AddNotificationSetting(RoleNotificationSetting notificationSetting)
        {
            await _notificationSettingValidator.Validate(notificationSetting, s => s.RoleId == notificationSetting.RoleId && s.SubdivisionId == notificationSetting.SubdivisionId);

            notificationSetting.ContractProfile = null;
            notificationSetting.Subdivision = null;
            notificationSetting.Role = null;

            notificationSetting.Id = Guid.NewGuid();

            _ = await _dataWorker.AddAsync(notificationSetting);
            await _dataWorker.SaveChangesAsync();

            return notificationSetting;
        }

        public async Task<RoleNotificationSetting> UpdateNotificationSetting(RoleNotificationSetting notificationSetting)
        {
            RoleNotificationSetting entity = await _dataGetter.GetSingleEntityAsync<RoleNotificationSetting>(notificationSetting.Id);

            entity.Enable = notificationSetting.Enable;
            entity.TargetNotifies = notificationSetting.TargetNotifies;

            entity.SubdivisionId = notificationSetting.SubdivisionId;
            entity.RoleId = notificationSetting.RoleId;

            await _notificationSettingValidator.Validate(entity, s => s.RoleId == notificationSetting.RoleId && s.SubdivisionId == notificationSetting.SubdivisionId);

            _ = _dataWorker.Update(entity);
            await _dataWorker.SaveChangesAsync();

            return entity;
        }

        public Task<IEnumerable<RoleNotificationSetting>> UpdateNotificationSetting(IEnumerable<RoleNotificationSetting> notificationSettings) =>
            EntityPackOperationAsync<RoleNotificationSetting>(notificationSettings, UpdateNotificationSetting);

        public Task<IEnumerable<RoleNotificationSetting>> AddNotificationSetting(IEnumerable<RoleNotificationSetting> notificationSettings) =>
            EntityPackOperationAsync<RoleNotificationSetting>(notificationSettings, AddNotificationSetting);

        public Task<IEnumerable<RoleNotificationSetting>> RemoveNotificationSetting(IEnumerable<Guid> notificationSettingIds) =>
            EntityPackOperationAsync(notificationSettingIds, RemoveNotificationSetting);
    }
}
