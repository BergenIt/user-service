using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using UserService.Core.Models;

namespace UserService.Data
{
    /// <summary>
    /// Управление контекстами
    /// </summary>
    public interface IContextManager : IDbContextFactory<UserServiceContext>
    {
        /// <summary>
        /// Сохранить изменения бд в кеш
        /// </summary>
        /// <param name="savedEntries"></param>
        /// <returns></returns>
        ValueTask SaveEntryToCache(IEnumerable<SavedEntry> savedEntries, bool typeValidate = true);

        /// <summary>
        /// Синхронизация кеша и бд
        /// </summary>
        /// <returns></returns>
        Task SynchronizeCacheWithPsql();
    }
}
