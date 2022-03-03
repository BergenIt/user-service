namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.InvalidArgument)]
    public class InvalidUserTokenException : AuditException
    {
        public InvalidUserTokenException() : base(nameof(InvalidUserTokenException)) { }
    }
}
