using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    /// <summary>
    /// Пользовательская работа с контрактами уведомлений
    /// </summary>
    public interface IContractProfileManager
    {
        /// <summary>
        /// Удалить контракт
        /// </summary>
        /// <param name="contractProfileIds"></param>
        /// <returns></returns>
        Task<IEnumerable<ContractProfile>> RemoveContractProfile(IEnumerable<Guid> contractProfileIds);

        /// <summary>
        /// Обновить контракт
        /// </summary>
        /// <param name="contractProfiles"></param>
        /// <returns></returns>
        Task<IEnumerable<ContractProfile>> UpdateContractProfile(IEnumerable<ContractProfile> contractProfiles);

        /// <summary>
        /// Добавить контракт
        /// </summary>
        /// <param name="contractProfiles"></param>
        /// <returns></returns>
        Task<IEnumerable<ContractProfile>> AddContractProfile(IEnumerable<ContractProfile> contractProfiles);

        /// <summary>
        /// Удалить контракты
        /// </summary>
        /// <param name="contractProfileId"></param>
        /// <returns></returns>
        Task<ContractProfile> RemoveContractProfile(Guid contractProfileId);

        /// <summary>
        /// Обновить контракты
        /// </summary>
        /// <param name="contractProfile"></param>
        /// <returns></returns>
        Task<ContractProfile> UpdateContractProfile(ContractProfile contractProfile);

        /// <summary>
        /// Создать контракты
        /// </summary>
        /// <param name="contractProfile"></param>
        /// <returns></returns>
        Task<ContractProfile> AddContractProfile(ContractProfile contractProfile);
    }
}
