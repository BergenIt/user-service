using System.Linq;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Data.EntityWorkers
{
    /// <summary>
    /// Сервис предоставляет IQueryable для сложных запросов
    /// </summary>
    public interface IInternalDataGetter : IDataGetter
    {
        internal IQueryable<TEntity> GetQueriable<TEntity>() where TEntity : class, IBaseEntity, new();
    }
}
