using System;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class WebHook : BaseEntity
    {
        public override string AuditName => Name;

        public string Name { get; set; }
        public string Comment { get; set; }

        public string Url { get; set; }

        public bool Enable { get; set; }

        public WebHookContractType WebHookContractType { get; set; }

        public Guid ContractProfileId { get; set; }
        public ContractProfile ContractProfile { get; set; }
    }
}

