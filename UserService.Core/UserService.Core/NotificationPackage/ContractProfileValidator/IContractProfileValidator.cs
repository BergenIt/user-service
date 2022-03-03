using System.Collections.Generic;

using UserService.Core.Entity;

namespace UserService.Core.NotificationPackage.ContractProfileValidator
{
    /// <summary>
    /// Валидатор настроек, которые ввел юзер
    /// </summary>
    public interface IContractProfileValidator
    {
        /// <summary>
        /// Валидация контракта
        /// </summary>
        /// <param name="contractProfiles"></param>
        void ValidateContractProfile(IEnumerable<ContractProfile> contractProfiles);

        /// <summary>
        /// Валидация контракта
        /// </summary>
        /// <param name="contractProfile"></param>
        void ValidateContractProfile(ContractProfile contractProfile);
    }
}
