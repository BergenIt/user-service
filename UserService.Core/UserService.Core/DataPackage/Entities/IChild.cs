
using Nest;

namespace UserService.Core.Entity
{
    /// <summary>
    /// Интерфейс для дочерних документов в эластике
    /// </summary>
    public interface IChild : IBaseEntity
    {
        JoinField Relation { get; set; }
    }
}
