namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.FailedPrecondition)]
    public class ActiveDirectoryUserOperationLockException : AuditException
    {
        public ActiveDirectoryUserOperationLockException() : base(nameof(ActiveDirectoryUserOperationLockException)) { }
    }
}
