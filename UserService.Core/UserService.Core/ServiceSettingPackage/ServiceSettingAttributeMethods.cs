using System;
using System.Reflection;

using UserService.Core.AuditPackage.AuditException;

namespace UserService.Core.ServiceSettings
{
    public static class ServiceSettingAttributeMethods
    {
        public static void ValidateValue(this ServiceSettingAttribute serviceSettingAttribute, string value)
        {
            EnviromentNameAttribute enviromentNameAttribute = typeof(ServiceSettingAttribute)
                .GetField(serviceSettingAttribute.ToString())
                .GetCustomAttribute<EnviromentNameAttribute>();

            Type type = enviromentNameAttribute.ValueType;

            object convertValue;

            try
            {
                convertValue = (value as IConvertible)?.ToType(type, null);
            }
            catch (Exception)
            {
                throw new ServiceSettingValidateException();
            }

            if (convertValue is null)
            {
                throw new ServiceSettingValidateException();
            }
        }

        public static string GetEnviromentName(this ServiceSettingAttribute serviceSettingAttribute)
        {
            EnviromentNameAttribute enviromentNameAttribute = typeof(ServiceSettingAttribute)
                .GetField(serviceSettingAttribute.ToString())
                .GetCustomAttribute<EnviromentNameAttribute>();

            return enviromentNameAttribute.EnviromentVariableName;
        }

        public static bool IsCredential(this ServiceSettingAttribute serviceSettingAttribute)
        {
            EnviromentNameAttribute enviromentNameAttribute = typeof(ServiceSettingAttribute)
                .GetField(serviceSettingAttribute.ToString())
                .GetCustomAttribute<EnviromentNameAttribute>();

            return enviromentNameAttribute.IsCredential;
        }
    }
}
