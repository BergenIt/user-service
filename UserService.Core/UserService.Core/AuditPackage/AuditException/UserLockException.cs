namespace UserService.Core.AuditPackage.AuditException
{

    [StatusCode(Grpc.Core.StatusCode.PermissionDenied)]
    public class UserLockException : AuditException
    {
        public UserLockException() : base(nameof(UserLockException)) { }
    }
}
