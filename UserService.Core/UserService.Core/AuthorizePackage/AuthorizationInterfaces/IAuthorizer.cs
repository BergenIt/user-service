using System.Threading.Tasks;

namespace UserService.Core.Authorizer
{
    /// <summary>
    /// Работа с сессиями пользователя
    /// </summary>
    public interface IAuthorizer
    {
        /// <summary>
        /// Логинит юзера и отдает токен
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="inputPasswod"></param>
        /// <returns>jwt</returns>
        Task<string> LoginAsync(string userName, string inputPasswod);

        /// <summary>
        /// При возможности обновляет токен юзера
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns>jwt</returns>
        Task<string> UpdateTokenAsync(string jwtToken);

        /// <summary>
        /// Конец сессии юзера
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task LogoutAsync(string userName);
    }
}
