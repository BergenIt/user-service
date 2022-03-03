using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.SenderInteraces;
using UserService.Core.Authorizer;

namespace UserService.Core.ServiceSettings
{
    public class ServiceSettingManager : IServiceSettingManager
    {
        private readonly IMailTransportFabric _mailTransportFabric;
        private readonly ILdapConnectionFabric _ldapConnectionFabric;

        public const string CredentialValue = "***";

        private readonly IDataWorker _dataWorker;
        private readonly IDataGetter _dataGetter;

        private static readonly ServiceSettingAttribute[] s_smtpServiceSettingAttributes = new ServiceSettingAttribute[]
        {
            ServiceSettingAttribute.SmtpLogin,
            ServiceSettingAttribute.SmtpPassword,
            ServiceSettingAttribute.SmtpPort,
            ServiceSettingAttribute.SmtpSenderAddress,
            ServiceSettingAttribute.SmtpSenderName,
            ServiceSettingAttribute.SmtpSslUse,
            ServiceSettingAttribute.SmtpHost,
        };

        private static readonly ServiceSettingAttribute[] s_ldapServiceSettingAttributes = new ServiceSettingAttribute[]
        {
            ServiceSettingAttribute.LdapDistinguishedName,
            ServiceSettingAttribute.LdapDistinguishedPassword,
            ServiceSettingAttribute.LdapHost,
            ServiceSettingAttribute.LdapPort,
            ServiceSettingAttribute.LdapRouteEmail,
            ServiceSettingAttribute.LdapRouteFullname,
            ServiceSettingAttribute.LdapRouteSubdivision,
            ServiceSettingAttribute.LdapSearchBase,
            ServiceSettingAttribute.LdapTimeout,
            ServiceSettingAttribute.LdapSsl,
            ServiceSettingAttribute.LdapLoginAttributeName,
        };

        public ServiceSettingManager(IDataWorker dataWorker, IDataGetter dataGetter, IMailTransportFabric mailTransportFabric, ILdapConnectionFabric ldapConnectionFabric)
        {
            _ldapConnectionFabric = ldapConnectionFabric;
            _mailTransportFabric = mailTransportFabric;
            _dataWorker = dataWorker;
            _dataGetter = dataGetter;
        }

        public async Task UpdateServiceSettings(IEnumerable<ServiceSetting> serviceSettings)
        {
            ServiceSettingAttribute[] serviceSettingAttributes = serviceSettings.Select(s => s.ServiceSettingAttribute).ToArray();

            IEnumerable<ServiceSetting> serviceSettingEntities = await _dataGetter
                .GetEntitiesAsync<ServiceSetting>(s => serviceSettingAttributes.Contains(s.ServiceSettingAttribute));

            foreach (ServiceSetting serviceSettingEntity in serviceSettingEntities)
            {
                ServiceSetting sourceValue = serviceSettings
                    .Single(s => s.ServiceSettingAttribute == serviceSettingEntity.ServiceSettingAttribute);

                sourceValue.ValidateValue();

                if (!serviceSettingEntity.IsCredential() || sourceValue.ServiceSettingValue != CredentialValue)
                {
                    serviceSettingEntity.ServiceSettingValue = sourceValue.ServiceSettingValue;
                }

                Environment.SetEnvironmentVariable(sourceValue.ServiceSettingAttribute.GetEnviromentName(), sourceValue.ServiceSettingValue);
            }

            await ValidateConnectAsync(serviceSettingEntities);

            _ = _dataWorker.UpdateRange(serviceSettingEntities);
            await _dataWorker.SaveChangesAsync();
        }

        public Task<IEnumerable<ServiceSetting>> GetLdapSettings() =>
            _dataGetter.GetEntitiesAsync<ServiceSetting>(s => s_ldapServiceSettingAttributes.Contains(s.ServiceSettingAttribute));

        public Task<IEnumerable<ServiceSetting>> GetSmtpSettings() =>
            _dataGetter.GetEntitiesAsync<ServiceSetting>(s => s_smtpServiceSettingAttributes.Contains(s.ServiceSettingAttribute));

        private async Task ValidateConnectAsync(IEnumerable<ServiceSetting> serviceSettings)
        {
            _ = _ldapConnectionFabric.CreateLdapConnection(
                await GetValue(ServiceSettingAttribute.LdapHost, serviceSettings),
                int.Parse(await GetValue(ServiceSettingAttribute.LdapPort, serviceSettings)),
                bool.Parse(await GetValue(ServiceSettingAttribute.LdapSsl, serviceSettings)),
                int.Parse(await GetValue(ServiceSettingAttribute.LdapTimeout, serviceSettings)),
                await GetValue(ServiceSettingAttribute.LdapDistinguishedName, serviceSettings),
            await GetValue(ServiceSettingAttribute.LdapDistinguishedPassword, serviceSettings)
            );

            _ = await _mailTransportFabric.CreateMailTransport(
                await GetValue(ServiceSettingAttribute.SmtpHost, serviceSettings),
                int.Parse(await GetValue(ServiceSettingAttribute.SmtpPort, serviceSettings)),
                bool.Parse(await GetValue(ServiceSettingAttribute.SmtpSslUse, serviceSettings)),
                await GetValue(ServiceSettingAttribute.SmtpLogin, serviceSettings),
                await GetValue(ServiceSettingAttribute.SmtpPassword, serviceSettings)
            );
        }

        private async Task<string> GetValue(ServiceSettingAttribute serviceSettingAttribute, IEnumerable<ServiceSetting> serviceSettings)
        {
            ServiceSetting serviceSetting = serviceSettings.SingleOrDefault(s => s.ServiceSettingAttribute == serviceSettingAttribute)
                ?? await _dataGetter.GetSingleEntityAsync<ServiceSetting>(s => s.ServiceSettingAttribute == serviceSettingAttribute);

            return serviceSetting.ServiceSettingValue;
        }
    }
}
