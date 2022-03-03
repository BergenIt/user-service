using System;
using System.Collections.Generic;

namespace UserService.Core.Entity
{
    public abstract class NotificationSetting : BaseEntity
    {
        public bool Enable { get; set; }

        public Guid ContractProfileId { get; set; }
        public ContractProfile ContractProfile { get; set; }

        public IEnumerable<TargetNotify> TargetNotifies { get; set; }
    }
}

