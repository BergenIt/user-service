using System;
using System.Collections.Generic;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class ContractSettingLine : BaseEntity
    {
        public ContractSettingLine()
        {
            ContractPropperties = new HashSet<ContractSettingPropperty>();
        }

        public override string AuditName => UserTemplate;

        public string UserProppertyName { get; set; }
        public string UserTemplate { get; set; }

        public bool Enable { get; set; }

        public int LineNumber { get; set; }

        public ICollection<ContractSettingPropperty> ContractPropperties { get; set; }

        public ContractProfile ContractProfile { get; set; }
        public Guid ContractProfileId { get; set; }
    }
}

