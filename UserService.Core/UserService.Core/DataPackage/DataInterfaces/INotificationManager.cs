using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Управление оповещениями
    /// </summary>
    public interface INotificationManager
    {
        /// <summary>
        /// Получает оповещения юзера
        /// </summary>
        /// <param name="filterContract"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IPageItems<Notification>> GetUserNotifications(FilterContract filterContract, string userName);

        /// <summary>
        /// Создает оповещения
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="userNames"></param>
        /// <returns></returns>
        Task CreateNotification(Notification notification, IEnumerable<string> userNames);

        /// <summary>
        /// Читает оповещения
        /// </summary>
        /// <param name="userNotificationIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task ReadUserNotifications(IEnumerable<Guid> userNotificationIds);

        /// <summary>
        /// Удаляет оповещения пользователя
        /// </summary>
        /// <param name="userNotificationIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemoveUserNotifications(IEnumerable<Guid> userNotificationIds);
    }
}
