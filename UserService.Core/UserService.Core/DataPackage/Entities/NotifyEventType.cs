using System.Collections.Generic;

namespace UserService.Core.Entity
{
    public class NotifyEventType : BaseEntity
    {
        public string Type { get; set; }

        public ICollection<ContractProfile> ContractProfiles { get; set; }
    }
}
