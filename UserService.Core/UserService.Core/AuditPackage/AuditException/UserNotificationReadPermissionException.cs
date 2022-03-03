namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.InvalidArgument)]
    public class UserNotificationReadPermissionException : AuditException
    {
        public UserNotificationReadPermissionException() : base(nameof(UserNotificationReadPermissionException)) { }
    }
}
