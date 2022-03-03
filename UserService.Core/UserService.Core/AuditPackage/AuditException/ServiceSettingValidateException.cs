namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.InvalidArgument)]
    public class ServiceSettingValidateException : AuditException
    {
        public ServiceSettingValidateException() : base(nameof(ServiceSettingValidateException)) { }
    }
}
