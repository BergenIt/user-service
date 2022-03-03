using System;

using Grpc.Core;

namespace UserService.Core.AuditPackage.AuditException
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StatusCodeAttribute : Attribute
    {
        public StatusCode StatusCode { get; }

        public StatusCodeAttribute(StatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
