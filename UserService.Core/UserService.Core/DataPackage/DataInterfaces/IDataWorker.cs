using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Базовый класс для crud операций с сущностями
    /// </summary>
    public interface IDataWorker
    {
        /// <summary>
        /// Создает сущность
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Создает сущности
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Вытягивает сущность и удаляет ее
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> RemoveAsync<TEntity>(Guid id) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Удаляет сущности
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Remove<TEntity>(TEntity entity) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Вытягивает сущности по ids и удаляет их
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> RemoveRangeAsync<TEntity>(IEnumerable<Guid> ids) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Удаляет сущности
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        IEnumerable<TEntity> RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Обновляет сущность
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Update<TEntity>(TEntity entity) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Вытягивает сущность из бд и применяет на действия, после чего обновляет
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="id"></param>
        /// <param name="actions"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync<TEntity>(Guid id, params Action<TEntity>[] actions) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Вытягивает сущности из бд и применяет на них действия, после чего обновляет
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="ids"></param>
        /// <param name="actions"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> UpdateRangeAsync<TEntity>(IEnumerable<Guid> ids, params Action<TEntity>[] actions) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Обновляет сущности
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        IEnumerable<TEntity> UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Сохраняет изменения в бд
        /// </summary>
        /// <param name="entryHandle"></param>
        /// <returns></returns>
        Task SaveChangesAsync(bool entryHandle = true);
    }
}
