using System.Collections.Generic;

using UserService.Core.AuditPackage;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class Position : BaseEntity
    {
        public Position()
        {
            Users = new HashSet<User>();
        }

        public override string AuditName => Name;

        public string Name { get; set; }
        public string Comment { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

