using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Работа с настройками оповещений
    /// </summary>
    public interface INotificationSettingManager<TNotificationSetting> where TNotificationSetting : NotificationSetting
    {
        /// <summary>
        /// Удалить настройку
        /// </summary>
        /// <param name="notificationSettingIds"></param>
        /// <returns></returns>
        Task<IEnumerable<TNotificationSetting>> RemoveNotificationSetting(IEnumerable<Guid> notificationSettingIds);

        /// <summary>
        /// Обновить настройку
        /// </summary>
        /// <param name="notificationSettings"></param>
        /// <returns></returns>
        Task<IEnumerable<TNotificationSetting>> UpdateNotificationSetting(IEnumerable<TNotificationSetting> notificationSettings);

        /// <summary>
        /// Добавить настройку
        /// </summary>
        /// <param name="notificationSettings"></param>
        /// <returns></returns>
        Task<IEnumerable<TNotificationSetting>> AddNotificationSetting(IEnumerable<TNotificationSetting> notificationSettings);

        /// <summary>
        /// Удалить настройку
        /// </summary>
        /// <param name="notificationSettingId"></param>
        /// <returns></returns>
        Task<TNotificationSetting> RemoveNotificationSetting(Guid notificationSettingId);

        /// <summary>
        /// Обновить настройку
        /// </summary>
        /// <param name="notificationSetting"></param>
        /// <returns></returns>
        Task<TNotificationSetting> UpdateNotificationSetting(TNotificationSetting notificationSetting);

        /// <summary>
        /// Создать настройку
        /// </summary>
        /// <param name="notificationSetting"></param>
        /// <returns></returns>
        Task<TNotificationSetting> AddNotificationSetting(TNotificationSetting notificationSetting);
    }
}
