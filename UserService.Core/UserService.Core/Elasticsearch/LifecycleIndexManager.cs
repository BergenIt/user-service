using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.AuditPackage;
using UserService.Core.Models;
using UserService.Core.NotifyEventTypeGetter;

namespace UserService.Core.Elasticsearch
{
    public class LifecycleIndexManager : ILifecycleIndexManager
    {
        private readonly IElasticsearchGetter _elasticsearchGetter;
        private readonly IElasticsearchWorker _elasticsearchWorker;

        private readonly IAuditActionGetter _auditActionGetter;
        private readonly INotifyEventTypeGetter _notifyEventTypeGetter;

        public LifecycleIndexManager(IElasticsearchGetter elasticsearchGetter, IElasticsearchWorker elasticsearchWorker, IAuditActionGetter auditActionGetter, INotifyEventTypeGetter notifyEventTypeGetter)
        {
            _elasticsearchGetter = elasticsearchGetter;
            _elasticsearchWorker = elasticsearchWorker;
            _auditActionGetter = auditActionGetter;
            _notifyEventTypeGetter = notifyEventTypeGetter;
        }

        public Task<IEnumerable<PolicyPhases>> GetAuditPhases()
        {
            IEnumerable<string> keys = _auditActionGetter.GetActions();

            return _elasticsearchGetter.GetPolicyPhasesAsync<Entity.Audit>(keys);
        }

        public Task<IEnumerable<PolicyPhases>> GetNotificationPhases()
        {
            IEnumerable<string> keys = _notifyEventTypeGetter.GetAllNotifyEventTypes();

            return _elasticsearchGetter.GetPolicyPhasesAsync<Entity.Notification>(keys);
        }

        public Task UpdatePhases(IEnumerable<PolicyPhases> policyPhases)
        {
            return _elasticsearchWorker.UpdatePolicyPhases(policyPhases);
        }
    }
}
