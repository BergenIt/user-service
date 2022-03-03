namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.FailedPrecondition)]
    public class JtiUserException : AuditException
    {
        public JtiUserException() : base(nameof(JtiUserException)) { }
    }
}
