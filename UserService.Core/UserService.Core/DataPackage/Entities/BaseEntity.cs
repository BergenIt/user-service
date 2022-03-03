using System;
using System.ComponentModel.DataAnnotations;

using Nest;

namespace UserService.Core.Entity
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Ignore]
        public virtual string AuditName => string.Empty;
    }
}
