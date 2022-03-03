namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.NotFound)]
    public class UserNotFoundException : AuditException
    {
        public UserNotFoundException() : base(nameof(UserNotFoundException)) { }
    }
}
