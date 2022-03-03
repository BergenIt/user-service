using System.Collections.Generic;

namespace UserService.Core.AuditPackage
{
    public interface IAuditActionGetter
    {
        IEnumerable<string> GetActions();
        string GetActionText(string action);
    }
}
