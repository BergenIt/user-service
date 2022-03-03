using System.Threading.Tasks;

namespace UserService.Core.DataPackage.Cleaner
{
    /// <summary>
    /// Интерфейс очистки данных
    /// </summary>
    public interface ICleaner
    {
        Task CleanAudit(int hour);
        Task CleanNotification(int hour);
        Task CleanUserNotification(int hour);
    }
}
