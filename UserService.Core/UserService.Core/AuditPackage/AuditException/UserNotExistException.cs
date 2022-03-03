namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.InvalidArgument)]
    public class UserNotExistException : AuditException
    {
        public UserNotExistException() : base(nameof(UserNotExistException)) { }
    }
}
