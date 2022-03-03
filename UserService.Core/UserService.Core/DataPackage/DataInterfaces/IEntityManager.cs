using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Интерфейс для базовой работы с сущностями
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public interface IEntityManager<TEntity> where TEntity : class, IBaseEntity, new()
    {
        /// <summary>
        /// Создает сущности
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> AddEntitites(IEnumerable<TEntity> entities);

        /// <summary>
        /// Получает страницу с сущностями
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IPageItems<TEntity>> GetEntitites(FilterContract filter);

        /// <summary>
        /// Получает сущность по ее id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetEntity(Guid id);

        /// <summary>
        /// Удаляет сущности
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> RemoveEntitites(IEnumerable<Guid> ids);

        /// <summary>
        /// Обновляет сущности
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> UpdateEntitites(IEnumerable<TEntity> entities);
    }
}
