namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.InvalidArgument)]
    public class ValidateNotificationSettingException : AuditException
    {
        public ValidateNotificationSettingException() : base(nameof(ValidateNotificationSettingException)) { }
    }
}
