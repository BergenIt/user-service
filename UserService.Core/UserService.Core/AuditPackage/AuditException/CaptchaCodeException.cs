namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.InvalidArgument)]
    public class CaptchaCodeException : AuditException
    {
        public CaptchaCodeException() : base(nameof(CaptchaCodeException)) { }
    }
}
