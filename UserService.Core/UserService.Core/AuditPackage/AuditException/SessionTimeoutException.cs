namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.PermissionDenied)]
    public class SessionTimeoutException : AuditException
    {
        public SessionTimeoutException() : base(nameof(SessionTimeoutException)) { }
    }
}
