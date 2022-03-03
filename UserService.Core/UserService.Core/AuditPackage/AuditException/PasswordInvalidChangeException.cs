namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.InvalidArgument)]
    public class PasswordInvalidChangeException : AuditException
    {
        public PasswordInvalidChangeException(PasswordInvalidChangeVariant invalidChange) : base(nameof(PasswordInvalidChangeException))
        {
            InvalidChange = invalidChange;
        }

        public PasswordInvalidChangeVariant InvalidChange { get; }
    }

    public enum PasswordInvalidChangeVariant
    {
        Compare,
        BasePolicy
    }
}
