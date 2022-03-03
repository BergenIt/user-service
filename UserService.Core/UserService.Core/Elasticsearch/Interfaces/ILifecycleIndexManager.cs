using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Models;

namespace UserService.Core.Elasticsearch
{
    public interface ILifecycleIndexManager
    {
        Task<IEnumerable<PolicyPhases>> GetAuditPhases();
        Task<IEnumerable<PolicyPhases>> GetNotificationPhases();
        Task UpdatePhases(IEnumerable<PolicyPhases> policyPhases);
    }
}