using System.Collections.Generic;

using Nest;

namespace UserService.Core.Entity
{
    /// <summary>
    /// Интерфейс для дочерних документов в эластике
    /// </summary>
    public interface IParent<TChild> : IBaseEntity where TChild : class, IChild, new()
    {
        JoinField Relation { get; set; }

        [Ignore]
        IEnumerable<TChild> Childs { get; set; }
    }
}
