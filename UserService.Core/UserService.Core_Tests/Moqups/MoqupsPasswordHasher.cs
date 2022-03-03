using UserService.Core.PolindromHasher;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsPasswordHasher : IPasswordHasher
    {
        public bool ComparePassword(string userName, string hashPassword, string newPassword)
        {
            return true;
        }

        public string HashPassword(string userName, string password)
        {
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(string userName, string hashedPassword, string providedPassword)
        {
            return PasswordVerificationResult.Success;
        }
    }
}
