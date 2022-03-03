using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Grpc.Core;

using UserService.Core.ServiceSettings;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис работы с настройками сервиса
    /// </summary>
    public class ServiceSettingServices : ServiceSettingService.ServiceSettingServiceBase
    {
        private readonly IServiceSettingManager _serviceSettingWorker;
        private readonly IMapper _mapper;

        /// <summary>
        /// Сервис работы с настройками сервиса
        /// </summary>
        /// <param name="serviceSettingWorker"></param>
        /// <param name="mapper"></param>
        public ServiceSettingServices(IServiceSettingManager serviceSettingWorker, IMapper mapper)
        {
            _serviceSettingWorker = serviceSettingWorker;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить настройки ldap подключений
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ServiceSettings> GetLdapServiceSetting(GetServiceSettingRequest request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.ServiceSetting> serviceSettings = await _serviceSettingWorker.GetLdapSettings();

            return new()
            {
                ServiceSettingList = { _mapper.Map<IEnumerable<ServiceSetting>>(serviceSettings) }
            };
        }

        /// <summary>
        /// Получить настройки ldap подключений
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ServiceSettings> GetSmtpServiceSetting(GetServiceSettingRequest request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.ServiceSetting> serviceSettings = await _serviceSettingWorker.GetSmtpSettings();

            return new()
            {
                ServiceSettingList = { _mapper.Map<IEnumerable<ServiceSetting>>(serviceSettings) }
            };
        }

        /// <summary>
        /// Обновить настройки сервиса
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ServiceSettings> UpdateServiceSetting(ServiceSettings request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.ServiceSetting> serviceSettings = _mapper.Map<IEnumerable<Core.Entity.ServiceSetting>>(request.ServiceSettingList);

            await _serviceSettingWorker.UpdateServiceSettings(serviceSettings);

            return request;
        }
    }
}
