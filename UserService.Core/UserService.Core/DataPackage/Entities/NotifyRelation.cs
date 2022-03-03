
using Nest;

namespace UserService.Core.Entity
{
    public abstract class NotifyRelation : BaseEntity
    {
        public JoinField Relation { get; set; }
    }
}

