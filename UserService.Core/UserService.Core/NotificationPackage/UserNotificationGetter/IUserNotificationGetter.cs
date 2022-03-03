using System.Threading.Tasks;

using DatabaseExtension;

namespace UserService.Core.NotificationPackage.UserNotificationGetter
{
    /// <summary>
    /// Получатель уведомлений юзера
    /// </summary>
    public interface IUserNotificationGetter
    {
        /// <summary>
        /// Получить страницу с уведомлениями юзера
        /// </summary>
        /// <param name="filterContract"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IPageItems<UserNotificationRecord>> GetUserNotitication(FilterContract filterContract, string userName);
    }
}
