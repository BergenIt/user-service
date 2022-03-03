using System;
using System.Collections.Generic;

using UserService.Core.Entity;

namespace UserService.Core.ContractConfigParser
{
    /// <summary>
    /// Парсер сообщений по контрактами и сырым данным
    /// </summary>
    public interface IContractConfigParser
    {
        /// <summary>
        /// Делает предварительную обработку оповещения для предоставленных профилей
        /// </summary>
        /// <param name="contractProfiles"></param>
        /// <param name="notification"></param>
        /// <returns>Словарь где ключ - Id профиля, значение - Сырая сборка сообщений для юзера</returns>
        IDictionary<Guid, IEnumerable<KeyValuePair<string, string>>> BuildRawStringArray(IEnumerable<ContractProfile> contractProfiles, Notification notification);

        /// <summary>
        /// Адаптирует сырую сборку собщений под конкретный тип контракта
        /// </summary>
        /// <param name="rawStringsBuild"></param>
        /// <param name="webHookContractType"></param>
        /// <returns>Словарь где ключ - Id профиля, значение - готовое сообщение для отправки</returns>
        IDictionary<Guid, string> GetMessageFromContractProfiles(IDictionary<Guid, IEnumerable<KeyValuePair<string, string>>> rawStringsBuild, WebHookContractType webHookContractType = WebHookContractType.StringArray);
    }
}
