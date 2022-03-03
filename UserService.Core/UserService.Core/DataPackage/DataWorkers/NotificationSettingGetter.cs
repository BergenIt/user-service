using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core.DataPackage.DataWorkers
{
    public class NotificationSettingGetter<TNotificationSetting> : INotificationSettingGetter<TNotificationSetting> where TNotificationSetting : NotificationSetting, IBaseEntity, new()
    {
        private readonly IDataGetter _dataGetter;

        public NotificationSettingGetter(IDataGetter dataGetter)
        {
            _dataGetter = dataGetter;
        }

        public Task<TNotificationSetting> GetNotificationSetting(Guid notificationSettingId)
        {
            return _dataGetter.GetSingleEntityAsync<TNotificationSetting>(notificationSettingId);
        }

        public Task<IPageItems<TNotificationSetting>> GetNotificationSettings(Guid contractProfileId, FilterContract filterContract)
        {
            filterContract.SearchFilters = new List<SearchFilter>(filterContract.SearchFilters)
            {
                new SearchFilter
                {
                    ColumnName = nameof(NotificationSetting.ContractProfileId),
                    Value = contractProfileId.ToString(),
                }
            };

            return _dataGetter.GetPage<TNotificationSetting>(filterContract);
        }
    }
}
