using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Класс предоставляющий функционал IdentityServer4
    /// </summary>
    public interface IIdentityManagersProvider
    {
        /// <summary>
        /// Удаляет клаймы юзера по смене пароля
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task RemoveGeneratrePasswordClaim(User user);

        /// <summary>
        /// Получает клайм юзера, указывающий сгенерирован ли у него пароль
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Claim> GetGeneratrePasswordClaim(User user);

        /// <summary>
        /// Получает клаймы по смене пароля
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetPasswordClaims(User user);

        /// <summary>
        /// Создает клайм по смене пароля
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldPasword"></param>
        /// <returns></returns>
        Task AddPasswordClaim(User user, string oldPasword);

        /// <summary>
        /// Вход с паролем в систему
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task PasswordSignInAsync(string userName, string password);

        /// <summary>
        /// Обновление штампа пользователя
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<User> UpdateSecurityStampAsync(string userName);

        /// <summary>
        /// Создает юзера с паролем
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task CreateUserAsync(User user, string password);

        /// <summary>
        /// Сменяет пароль юзера
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task ChangeUserPasswordAsync(User user, string password);

        /// <summary>
        /// Валидирует токен юзера на "Забыли пароль"
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> ValidateUserForgotPasswordTokenAsync(User user, string token);

        /// <summary>
        /// Создает токен юзера на "Забыли пароль"
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> CreateUserForgotPasswordTokenAsync(User user);
    }
}
