using System;
using System.Collections.Generic;
using System.Linq;

using UserService.Core.Entity;

namespace UserService.Core.Models
{
    public record ChangeRelationResult<TEntity>(IEnumerable<TEntity> AddedEntities, IEnumerable<TEntity> RemovedEntities) where TEntity : class, IBaseEntity, new()
    {
        public IEnumerable<TEntity> ChangeEntities { get => AddedEntities.Concat(RemovedEntities); }
        public IEnumerable<Guid> ChangeEntityIds { get => AddedEntities.Select(e => e.Id).Concat(RemovedEntities.Select(e => e.Id)); }
    }
}
