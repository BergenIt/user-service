using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.ServiceSettings
{
    /// <summary>
    /// Работа с настройками сервиса
    /// </summary>
    public interface IServiceSettingManager
    {
        /// <summary>
        /// Получить настройки Ldap подключения
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ServiceSetting>> GetLdapSettings();

        /// <summary>
        /// Получить smtp настройки
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ServiceSetting>> GetSmtpSettings();

        /// <summary>
        /// Обновить настройки
        /// </summary>
        /// <param name="serviceSettings"></param>
        /// <returns></returns>
        Task UpdateServiceSettings(IEnumerable<ServiceSetting> serviceSettings);
    }
}
