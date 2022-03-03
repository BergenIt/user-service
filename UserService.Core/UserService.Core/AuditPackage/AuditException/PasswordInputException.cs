namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.PermissionDenied)]
    public class PasswordInputException : AuditException
    {
        public PasswordInputException() : base(nameof(PasswordInputException)) { }
    }
}
