using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserService.Core.AuditPackage.AuditException.Tests
{
    [TestClass]
    public class AuditException_Tests
    {
        private readonly AuditException[] _exceptions_types = new AuditException[]
        {
            new ActiveDirectoryUserOperationLockException(),
            new CaptchaCodeException(),
            new InvalidUserTokenException(),
            new JtiUserException(),
            new PasswordInputException(),
            new PasswordInvalidChangeException(PasswordInvalidChangeVariant.Compare),
            new ServiceSettingValidateException(),
            new SessionTimeoutException(),
            new UserLockException(),
            new UserNotExistException(),
            new UserNotificationDeletePermissionException(),
            new UserNotificationReadPermissionException(),
            new ValidateNotificationSettingException()
        };

        [TestMethod]
        public void Base_AuditException_Tests()
        {
            foreach (AuditException auditException in _exceptions_types)
            {
                System.Type type = auditException.GetType();

                Assert.IsTrue(auditException.AuditType == type.Name, auditException.GetType().Name);
            }
        }
    }
}
