
using Novell.Directory.Ldap;

using UserService.Core;
using UserService.Core.Authorizer;

namespace UserService.Main.Ldap
{
    /// <summary>
    /// Создает лдап подключение
    /// </summary>
    public class LdapConnectionFabric : ILdapConnectionFabric
    {
        private readonly ProjectOptions _projectOptions;

        /// <summary>
        /// Создает лдап подключение
        /// </summary>
        /// <param name="projectOptions"></param>
        public LdapConnectionFabric(ProjectOptions projectOptions)
        {
            _projectOptions = projectOptions;
        }

        /// <summary>
        /// Создать лдап подключение из projectOptions
        /// </summary>
        /// <returns></returns>
        public ILdapConnection CreateLdapConnection()
        {
            return CreateLdapConnection(
                _projectOptions.LdapHost, 
                _projectOptions.LdapPort, 
                _projectOptions.LdapSsl, 
                _projectOptions.LdapTimeout, 
                _projectOptions.DistinguishedName, 
                _projectOptions.DistinguishedPassword
            );
        }

        /// <summary>
        /// Создать лдап подключение
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="sslUse"></param>
        /// <param name="timeout"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ILdapConnection CreateLdapConnection(string host, int port, bool sslUse, int timeout, string login, string password)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                return null;
            }

            LdapConnection ldapConnection = new();

            ldapConnection.SecureSocketLayer = sslUse;
            ldapConnection.ConnectionTimeout = timeout;

            ldapConnection.Connect(host, port);

            ldapConnection.Bind(login, password);

            return ldapConnection;
        }
    }
}
