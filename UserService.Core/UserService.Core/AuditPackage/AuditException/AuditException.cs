using System;

namespace UserService.Core.AuditPackage.AuditException
{
    [StatusCode(Grpc.Core.StatusCode.Unknown)]
    public abstract class AuditException : Exception
    {
        protected AuditException(string auditType)
        {
            AuditType = auditType;
        }

        public string AuditType { get; init; }
    }
}
