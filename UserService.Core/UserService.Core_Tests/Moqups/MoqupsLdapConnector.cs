using System;

using UserService.Core.Authorizer;
using UserService.Core.UserManager;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsLdapConnector : ILdapConnector
    {
        private const string DefaultValue = "testUser";

        public UserCreateCommand GetUserFromLdap(string userName, string password)
        {
            return new UserCreateCommand(
                userName,
                DefaultValue,
                CreateGuid(1),
                null,
                string.Empty,
                string.Empty,
                DefaultValue,
                DateTime.UtcNow.AddYears(2),
                DateTime.MaxValue,
                Array.Empty<Guid>(),
                false,
                PasswordActionEnum.WithoutChange,
                string.Empty
            );
        }

        private static Guid CreateGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}
