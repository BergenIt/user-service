
using Novell.Directory.Ldap;

using UserService.Core.Authorizer;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupLdapConnectionFabric : ILdapConnectionFabric
    {
        public ILdapConnection CreateLdapConnection()
        {
            return new MoqupsLdapConnection();
        }

        public ILdapConnection CreateLdapConnection(string host, int port, bool sslUse, int timeout, string login, string password)
        {
            return new MoqupsLdapConnection();
        }
    }
}
