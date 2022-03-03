using System.Threading.Tasks;

namespace UserService.Core.SenderInteraces
{
    /// <summary>
    /// Класс сокета
    /// </summary>
    public interface IWebsocket
    {
        /// <summary>
        /// Отправляет пользователям
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        ValueTask SendUsersAsync(SenderContract contract);
    }
}
