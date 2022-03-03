using System;

namespace UserService.Core.AuditPackage
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuditEntityAttribute : Attribute { }
}
