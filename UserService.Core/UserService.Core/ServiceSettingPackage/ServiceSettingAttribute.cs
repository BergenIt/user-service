namespace UserService.Core.ServiceSettings
{
    public enum ServiceSettingAttribute
    {

        [EnviromentName("LDAP_DISTINGUISHED_NAME")]
        LdapDistinguishedName,

        [EnviromentName("LDAP_DISTINGUISHED_PASSWORD", true)]
        LdapDistinguishedPassword,

        [EnviromentName("LDAP_SEARCH_BASE")]
        LdapSearchBase,

        [EnviromentName("LDAP_LOGIN_ATTRIBUTE_NAME")]
        LdapLoginAttributeName,

        [EnviromentName("LDAP_ROUTE_EMAIL")]
        LdapRouteEmail,

        [EnviromentName("LDAP_ROUTE_FULLNAME")]
        LdapRouteFullname,

        [EnviromentName("LDAP_ROUTE_SUBDIVISION")]
        LdapRouteSubdivision,

        [EnviromentName("LDAP_HOST")]
        LdapHost,

        [EnviromentName("LDAP_PORT", typeof(int))]
        LdapPort,

        [EnviromentName("LDAP_TIMEOUT", typeof(int))]
        LdapTimeout,

        [EnviromentName("LDAP_SSL", typeof(bool))]
        LdapSsl,

        [EnviromentName("SMTP_HOST")]
        SmtpHost,

        [EnviromentName("SMTP_PORT", typeof(int))]
        SmtpPort,

        [EnviromentName("SMTP_SSl_USE", typeof(bool))]
        SmtpSslUse,

        [EnviromentName("SMTP_LOGIN")]
        SmtpLogin,

        [EnviromentName("SMTP_PASSWORD", true)]
        SmtpPassword,

        [EnviromentName("SMTP_SENDER_NAME")]
        SmtpSenderName,

        [EnviromentName("SMTP_SENDER_ADDRESS")]
        SmtpSenderAddress,
    }
}
