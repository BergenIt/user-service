using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.NotificationPackage
{
    /// <summary>
    /// Обработчик событий системы
    /// </summary>
    public interface INotifyEventHandler
    {
        /// <summary>
        /// Создает оповещение и рассылает его получателям
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        Task NotifyEventHandle(Notification notification);
    }
}
