using System;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.PasswordManager
{
    /// <summary>
    /// Управление паролями
    /// </summary>
    public interface IPasswordManager
    {
        /// <summary>
        /// Создать и отправить токен забыли пароль
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="applicationUrl"></param>
        /// <returns></returns>
        Task CreateForgotPasswordToken(string userName, string applicationUrl);

        /// <summary>
        /// Изменить пароль юзера
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task ChangeUserPassword(string userName, string password);

        /// <summary>
        /// Изменить пароль юзера
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="sendToEmail"></param>
        /// <returns></returns>
        Task ChangeUserPassword(Guid userId, string password, bool sendToEmail);

        /// <summary>
        /// Изменить пароль юзера
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="sendToEmail"></param>
        /// <returns></returns>
        Task ChangeUserPassword(User user, string password, bool sendToEmail = false);

        /// <summary>
        /// Изменить забытый пароль юзера по токену
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task ChangeForgotPasswordToken(string userName, string token, string password);
    }
}
