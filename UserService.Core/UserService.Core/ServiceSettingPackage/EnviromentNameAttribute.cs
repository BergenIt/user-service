using System;

namespace UserService.Core.ServiceSettings
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnviromentNameAttribute : Attribute
    {
        public Type ValueType { get; }
        public string EnviromentVariableName { get; }
        public bool IsCredential { get; }
        public bool IsConnectionData { get; }

        public EnviromentNameAttribute(string enviromentVariableName)
        {
            EnviromentVariableName = enviromentVariableName;
            ValueType = typeof(string);
            IsCredential = false;
            IsConnectionData = false;
        }

        public EnviromentNameAttribute(string enviromentVariableName, Type valueType, bool isConnectionData = false)
        {
            EnviromentVariableName = enviromentVariableName;
            ValueType = valueType;
            IsCredential = false;
            IsConnectionData = isConnectionData;
        }

        public EnviromentNameAttribute(string enviromentVariableName, bool isCredential)
        {
            EnviromentVariableName = enviromentVariableName;
            ValueType = typeof(string);
            IsCredential = isCredential;
            IsConnectionData = false;
        }
    }
}
