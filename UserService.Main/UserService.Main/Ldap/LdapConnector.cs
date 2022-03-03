using System;
using System.Collections.Generic;
using System.Linq;

using Novell.Directory.Ldap;

using UserService.Core;
using UserService.Core.AuditPackage.AuditException;
using UserService.Core.Authorizer;
using UserService.Core.UserManager;
using UserService.Data;

namespace UserService.Main.Ldap
{
    /// <summary>
    /// Класс, реализующий коннект AD
    /// </summary>
    public class LdapConnector : ILdapConnector
    {
        private readonly ProjectOptions _options;

        private readonly ILdapConnection _ldapConnection;

        /// <summary>
        /// Класс, реализующий коннект AD
        /// </summary>
        /// <param name="ldapConnection"></param>
        /// <param name="options"></param>
        public LdapConnector(ILdapConnection ldapConnection, ProjectOptions options)
        {
            _ldapConnection = ldapConnection;
            _options = options;
        }

        /// <summary>
        /// Подключается к AD, ищет пользователя и логинится под указанным паролем
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserCreateCommand GetUserFromLdap(string userName, string password)
        {
            if (_ldapConnection is null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordInputException();
            }

            LdapEntry user = SearchUser(userName);

            try
            {
                _ldapConnection.Bind(user.DN, password);
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.ForContext<ILdapConnection>().Error("Error ldap connection {ex}", ex);

                throw new PasswordInputException();
            }

            LdapAttribute ldapAttributeEmail = user.getAttribute(_options.LoginEmail);

            if (ldapAttributeEmail is null || ldapAttributeEmail.size() == 0)
            {
                throw new KeyNotFoundException($"Email {userName} not found");
            }

            string email = ldapAttributeEmail.StringValue;

            string name = GetAttribute(user, _options.LoginFullname, userName);
            string subdivisionId = GetAttribute(user, _options.LoginSubdivisionId, null);

            _ldapConnection.Disconnect();

            return new UserCreateCommand(
                userName,
                email,
                string.IsNullOrWhiteSpace(subdivisionId) ? UserServiceContext.CreateGuid(1) : Guid.Parse(subdivisionId),
                null,
                string.Empty,
                string.Empty,
                name,
                DateTime.UtcNow.AddYears(2),
                DateTime.MaxValue,
                Array.Empty<Guid>(),
                false,
                PasswordActionEnum.WithoutChange,
                string.Empty
            );

        }

        private LdapEntry SearchUser(string userName)
        {
            IEnumerable<LdapEntry> lsc = _ldapConnection.Search(_options.SearchBase,
                               LdapConnection.SCOPE_SUB,
                               $"{_options.LoginAttribute}={userName}",
                               new string[] { _options.LoginEmail, _options.LoginFullname, _options.LoginAttribute },
                               false);

            LdapEntry ldapEntry = lsc.FirstOrDefault(l => l.getAttribute(_options.LoginAttribute)?.StringValue?.ToLower() == userName.ToLower());

            if (ldapEntry is null)
            {
                throw new UserNotFoundException();
            }

            return ldapEntry;
        }

        private static string GetAttribute(LdapEntry ldapEntry, string name, string defaultName)
        {
            string attribValue;
            try
            {
                string ldapAttribute = ldapEntry.getAttribute(name)?.StringValue;
                attribValue = string.IsNullOrEmpty(ldapAttribute) ? defaultName : ldapAttribute;
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.ForContext<ILdapConnection>().Warning("Attribute {name} not found, with error: {ex}", name, ex);

                attribValue = defaultName;
            }

            return attribValue;
        }
    }
}
