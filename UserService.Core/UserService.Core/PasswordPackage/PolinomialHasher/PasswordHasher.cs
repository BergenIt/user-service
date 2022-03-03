
using Microsoft.AspNetCore.Identity;

using UserService.Core.Entity;

namespace UserService.Core.PolindromHasher
{
    public class PasswordHasher : IPasswordHasher<User>
    {
        private readonly IPasswordHasher _passwordHasher;

        public PasswordHasher(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user.UserName, password);
        }

        public Microsoft.AspNetCore.Identity.PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            return (Microsoft.AspNetCore.Identity.PasswordVerificationResult)_passwordHasher.VerifyHashedPassword(user.UserName, hashedPassword, providedPassword);
        }
    }
}
