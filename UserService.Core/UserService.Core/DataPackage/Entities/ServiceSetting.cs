using UserService.Core.AuditPackage;
using UserService.Core.ServiceSettings;

namespace UserService.Core.Entity
{
    [AuditEntity]
    public class ServiceSetting : BaseEntity
    {
        public override string AuditName => ServiceSettingAttribute.ToString();

        public ServiceSettingAttribute ServiceSettingAttribute { get; set; }
        public string ServiceSettingValue { get; set; }

        public void ValidateValue() => ServiceSettingAttribute.ValidateValue(ServiceSettingValue);
        public bool IsCredential() => ServiceSettingAttribute.IsCredential();
    }
}

