using Novell.Directory.Ldap;

namespace UserService.Core.Authorizer
{
    /// <summary>
    /// Подключатеся в AD и пытается вытянуть юзера и проверить его пароль
    /// </summary>
    public interface ILdapConnectionFabric
    {
        /// <summary>
        /// Создает подключение с логином к Ад из projectOptions
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ILdapConnection CreateLdapConnection();

        /// <summary>
        /// Создает подключение с логином к Ад
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="sslUse"></param>
        /// <param name="timeout"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ILdapConnection CreateLdapConnection(string host, int port, bool sslUse, int timeout, string login, string password);
    }
}
