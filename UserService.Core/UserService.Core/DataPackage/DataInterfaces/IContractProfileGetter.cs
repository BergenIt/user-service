using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Запросы на получение контрактов уведомлений
    /// </summary>
    public interface IContractProfileGetter
    {
        /// <summary>
        /// Получить страницу с контрактами
        /// </summary>
        /// <param name="filterContract"></param>
        /// <returns></returns>
        Task<IPageItems<ContractProfile>> GetContractProfiles(FilterContract filterContract);

        /// <summary>
        /// Получить контракт по id
        /// </summary>
        /// <param name="contractProfilesId"></param>
        /// <returns></returns>
        Task<ContractProfile> GetContractProfile(Guid contractProfilesId);

        /// <summary>
        /// Получает необходимые контракты по типу события (Include webhook entity)
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        Task<IEnumerable<ContractProfile>> GetContractProfilesWithWebhooks(string eventType);

        /// <summary>
        /// Получает которакты которые подходят пользовтелю
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<ContractProfile>> GetUserContractProfiles(Guid userId);

        /// <summary>
        /// Получает юзеров по ids контрактов и целевого объекта (contract profiles должны быть с 1 типом)
        /// </summary>
        /// <param name="contractProfileIds"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        Task<IEnumerable<UserSendView>> GetUsersFromContractProfiles(IEnumerable<Guid> contractProfileIds, string objectId);
    }
}
