using System;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Селектор для настроек оповещений
    /// </summary>
    public interface INotificationSettingGetter<TNotificationSetting> where TNotificationSetting : NotificationSetting, IBaseEntity, new()
    {
        /// <summary>
        /// Получает страницу с настройками
        /// </summary>
        /// <param name="contractProfileIds"></param>
        /// <param name="filterContract"></param>
        /// <returns></returns>
        Task<IPageItems<TNotificationSetting>> GetNotificationSettings(Guid contractProfileIds, FilterContract filterContract);

        /// <summary>
        /// Получает настройку по id
        /// </summary>
        /// <param name="notificationSettingId"></param>
        /// <returns></returns>
        Task<TNotificationSetting> GetNotificationSetting(Guid notificationSettingId);
    }
}
