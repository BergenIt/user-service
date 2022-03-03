using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Базовый интерфейс для получения данных из контекста
    /// </summary>
    public interface IDataGetter
    {
        /// <summary>
        /// Получить страницу
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="filterContract"></param>
        /// <returns></returns>
        Task<IPageItems<TEntity>> GetPage<TEntity>(FilterContract filterContract) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Получить сингл сущность
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<TEntity> GetSingleEntityAsync<TEntity>(Guid Id) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Получить первую сущность
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstEntityAsync<TEntity>(Guid Id) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Получает сущности по их ids
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="guids"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>(IEnumerable<Guid> guids) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Получает сущности по предикату
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Получает сущность по предикату
        /// </summary>
        /// <typeparam name="TEntity">Сущность для получения</typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> GetSingleEntityAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Получает рекурсивно связанные сущности
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetRecoursiveEntities<TEntity>(Guid id, Expression<Func<TEntity, IEnumerable<TEntity>>> path) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Получает рекурсивно связанные сущности
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<IEnumerable<Guid>> GetRecoursiveEntityIds<TEntity>(Guid id, Expression<Func<TEntity, IEnumerable<TEntity>>> path) where TEntity : class, IBaseEntity, new();
    }
}
