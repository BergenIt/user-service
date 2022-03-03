
using UserService.Core.UserManager;

namespace UserService.Core.Authorizer
{
    /// <summary>
    /// Подключатеся в AD и пытается вытянуть юзера и проверить его пароль
    /// </summary>
    public interface ILdapConnector
    {
        /// <summary>
        /// Подключатеся в AD и пытается вытянуть юзера и проверить его пароль
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Отдает комманду которой можно создать юзера при необходимости, может отдать null, при не настроенном ldap подключении</returns>
        UserCreateCommand GetUserFromLdap(string userName, string password);
    }
}
