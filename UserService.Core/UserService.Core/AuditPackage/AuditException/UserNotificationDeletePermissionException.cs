namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.InvalidArgument)]
    public class UserNotificationDeletePermissionException : AuditException
    {
        public UserNotificationDeletePermissionException() : base(nameof(UserNotificationDeletePermissionException)) { }
    }
}
