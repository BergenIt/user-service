using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

namespace UserService.Core
{
    public class ProjectOptions
    {
        public bool AcceptRanges => GetEnvironmentVariable<bool>(nameof(HeaderNames.AcceptRanges), validationIgnore: true);

        public string ElasticsearchUri => GetEnvironmentVariable("ELASTICSEARCH_URI");
        public string ElasticsearchToken => GetEnvironmentVariable("ELASTICSEARCH_BASIC_TOKEN", validationIgnore: true);

        public string AuditRoute => GetEnvironmentVariable("AUDIT_ROUTE");

        public string PermissionRoute => GetEnvironmentVariable("PERMISSION_ROUTE", configurationName: "DefaultRolesRoute");

        public string NotifyEventTypeRoute => GetEnvironmentVariable("NOTIFY_EVENT_TYPE_SETTING_ROUTE", configurationName: "NotifyEvenTypeSettingRoute");

        public string PsqlDbConnectionString => $"Host={PsqlDbHost};{(PsqlDbPort == default ? string.Empty : $"Port={PsqlDbPort};")}User Id={PsqlDbUser};Password={PsqlDbPass};Database={PsqlDbName};";
        public string PsqlDbHost => GetEnvironmentVariable("PSQL__HOST", configurationName: "Psql:Host");
        public int PsqlDbPort => GetEnvironmentVariable<int>("PSQL__PORT", validationIgnore: true, configurationName: "Psql:Port");
        public string PsqlDbName => GetEnvironmentVariable("PSQL__NAME", configurationName: "Psql:Name");
        public string PsqlDbUser => GetEnvironmentVariable("PSQL__USER", configurationName: "Psql:User");
        public string PsqlDbPass => GetEnvironmentVariable("PSQL__PASS", configurationName: "Psql:Password");

        public string DistinguishedName => GetEnvironmentVariable("LDAP_DISTINGUISHED_NAME", true);
        public string DistinguishedPassword => GetEnvironmentVariable("LDAP_DISTINGUISHED_PASSWORD", true);
        public string SearchBase => GetEnvironmentVariable("LDAP_SEARCH_BASE", true);
        public string LoginAttribute => GetEnvironmentVariable("LDAP_LOGIN_ATTRIBUTE_NAME", true);
        public string LoginEmail => GetEnvironmentVariable("LDAP_ROUTE_EMAIL", true);
        public string LoginFullname => GetEnvironmentVariable("LDAP_ROUTE_FULLNAME", true);
        public string LoginSubdivisionId => GetEnvironmentVariable("LDAP_ROUTE_SUBDIVISION", true);
        public bool LdapSsl => GetEnvironmentVariable<bool>("LDAP_SSL", true);
        public string LdapHost => GetEnvironmentVariable("LDAP_HOST", true);
        public int LdapPort => GetEnvironmentVariable<int>("LDAP_PORT", true);
        public int LdapTimeout => GetEnvironmentVariable<int>("LDAP_TIMEOUT", true);

        public string LdapCertPath => GetEnvironmentVariable("LDAP_CERTS_ROUTE", true);
        public string LdapCertPassword => GetEnvironmentVariable("LDAP_CERTS_PASSWORD", true);

        public string IdentitySecret => GetEnvironmentVariable("IDENTITY_SECRET");

        public string SmtpHost => GetEnvironmentVariable("SMTP_HOST", validationIgnore: true);
        public int SmtpPort => GetEnvironmentVariable<int>("SMTP_PORT", true);
        public bool SmtpSslUse => GetEnvironmentVariable<bool>("SMTP_SSl_USE");
        public string SmtpLogin => GetEnvironmentVariable("SMTP_LOGIN", true);
        public string SmtpPassword => GetEnvironmentVariable("SMTP_PASSWORD", true);

        public string SmtpSenderName => GetEnvironmentVariable("SMTP_SENDER_NAME", true);
        public string SmtpSenderAddress => GetEnvironmentVariable("SMTP_SENDER_ADDRESS", true);

        public int LoadBalanserDelay => GetEnvironmentVariable<int>("LOAD_BALANCER_TIMER_DELAY");

        public string SerilogMinimumLevel => GetEnvironmentVariable("MINIMUM_LEVEL");

        public int GrpcPort => GetEnvironmentVariable<int>("GRPC_PORT", configurationName: "Port:Grpc");
        public int HttpPort => GetEnvironmentVariable<int>("HTTP_PORT", configurationName: "Port:Http");

        #region Logic
        private readonly IConfiguration _configuration;

        public ProjectOptions() { }

        public ProjectOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T GetEnvironmentVariable<T>(string variableName, bool validationIgnore = false, string configurationName = null) where T : IConvertible
        {
            string value = Environment.GetEnvironmentVariable(variableName);

            if (configurationName is not null && string.IsNullOrWhiteSpace(value))
            {
                value = _configuration.GetValue<string>(configurationName);
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return validationIgnore
                    ? default
                    : throw new EnvironmentVariableException(variableName);
            }

            return (T)(value as IConvertible)?.ToType(typeof(T), null);
        }

        public string GetEnvironmentVariable(string variableName, bool validationIgnore = false, string configurationName = null)
        {
            string value = Environment.GetEnvironmentVariable(variableName);

            if (configurationName is not null && string.IsNullOrWhiteSpace(value))
            {
                value = _configuration.GetValue<string>(configurationName);
            }

            if (string.IsNullOrWhiteSpace(value) && !validationIgnore)
            {
                throw new EnvironmentVariableException(variableName);
            }

            return value ?? string.Empty;
        }
        #endregion
    }
}
