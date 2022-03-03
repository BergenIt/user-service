using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using DatabaseExtension.Translator;

using UserService.Core.AuditPackage.AuditException;
using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.JwtGenerator;
using UserService.Core.Models;
using UserService.Core.UserManager;

namespace UserService.Core.Authorizer
{
    public class Authorizer : IAuthorizer
    {
        private readonly IIdentityManagersProvider _identityManagersProvider;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IUserGetter _userGetter;
        private readonly IUserAccessGetter _userAccessGetter;
        private readonly IDataWorker _dataWorker;
        private readonly ILdapConnector _ldapConnector;
        private readonly IUserManager _userManager;
        private readonly IAuditWorker _auditWorker;
        private readonly ITranslator _translator;

        public Authorizer(IIdentityManagersProvider identityManagersProvider, IJwtGenerator jwtGenerator, IUserGetter userGetter, IUserAccessGetter userAccessGetter, IDataWorker dataWorker, ILdapConnector ldapConnector, IUserManager userManager, IAuditWorker auditWorker, ITranslator translator)
        {
            _identityManagersProvider = identityManagersProvider;
            _jwtGenerator = jwtGenerator;
            _userGetter = userGetter;
            _userAccessGetter = userAccessGetter;
            _dataWorker = dataWorker;
            _ldapConnector = ldapConnector;
            _userManager = userManager;
            _auditWorker = auditWorker;
            _translator = translator;
        }

        public async Task<string> LoginAsync(string userName, string inputPasswod)
        {
            IUserGetter.UserExist userExist = await _userGetter.UserExistAsync(userName);

            if (userExist is IUserGetter.UserExist.Exist)
            {
                await _identityManagersProvider.PasswordSignInAsync(userName, inputPasswod);
            }
            else
            {
                UserCreateCommand user = _ldapConnector.GetUserFromLdap(userName, inputPasswod);

                if (user is null)
                {
                    throw new UserNotFoundException();
                }

                if (userExist is IUserGetter.UserExist.NotFound)
                {
                    _ = await _userManager.CreateUsers(new UserCreateCommand[] { user });
                }
            }

            string token = await GenerateJwtToken(userName);

            Guid userId = await _userGetter.GetUserId(userName);

            _ = await _dataWorker.UpdateAsync<User>(userId, u => u.LastLogin = DateTime.UtcNow);

            await _dataWorker.SaveChangesAsync(false);

            string message = _translator.GetUserText<IAuthorizer>(nameof(LoginAsync));

            AuditCreateCommand auditCreateCommand = new(userName, message, nameof(IAuthorizer));

            await _auditWorker.CreateAudit(auditCreateCommand);

            return token;
        }

        public async Task LogoutAsync(string userName)
        {
            _ = await _identityManagersProvider.UpdateSecurityStampAsync(userName);

            string message = _translator.GetUserText<IAuthorizer>(nameof(LogoutAsync));

            AuditCreateCommand auditCreateCommand = new(userName, message, nameof(IAuthorizer));

            await _auditWorker.CreateAudit(auditCreateCommand);
        }

        public async Task<string> UpdateTokenAsync(string jwtToken)
        {
            TokenMeta tokenMeta = _jwtGenerator.GetTokenData(jwtToken);

            string securityStamp = await _userGetter.GetUserSecurityKey(tokenMeta.UserName);

            if (DateTime.UtcNow > tokenMeta.ValidTo.AddHours(6))
            {
                throw new SessionTimeoutException();
            }

            if (securityStamp != tokenMeta.UserKey)
            {
                throw new InvalidUserTokenException();
            }

            return await GenerateJwtToken(tokenMeta.UserName);
        }

        private async Task<string> GenerateJwtToken(string userName)
        {
            User user = await _userGetter.GetUser(userName, false);

            if (user.UserState != UserState.Active)
            {
                throw new UserLockException();
            }

            IEnumerable<Role> roles = await _userGetter.GetUserRoles(userName, false);

            string[] roleNames = roles.Select(r => r.Name).ToArray();

            ICollection<Claim> accessClaims = await _userAccessGetter.GetUserClaims(user.Id);

            _ = await _identityManagersProvider.UpdateSecurityStampAsync(userName);
            string securityStamp = await _userGetter.GetUserSecurityKey(userName);

            Claim claimPassword = await _identityManagersProvider.GetGeneratrePasswordClaim(user);

            if (claimPassword is not null)
            {
                accessClaims.Add(claimPassword);
            }

            string token = _jwtGenerator.CreateToken(
                user.Id,
                userName,
                user.Email,
                securityStamp,
                roleNames,
                accessClaims
            );

            return token;
        }
    }
}
