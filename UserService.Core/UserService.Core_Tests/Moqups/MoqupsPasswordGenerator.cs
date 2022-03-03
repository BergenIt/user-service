using System;

using UserService.Core.PasswordGenerator;

namespace UserService.Core_Tests.Moqups
{

    public class MoqupsPasswordGenerator : IPasswordGenerator
    {
        public string GeneratePassword()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
