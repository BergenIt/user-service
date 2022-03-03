using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension.Translator;

using UserService.Core.AuditPackage.AuditException;
using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.Models;
using UserService.Core.PasswordGenerator;
using UserService.Core.PolindromHasher;
using UserService.Core.SenderInteraces;

namespace UserService.Core.PasswordManager
{
    public class PasswordManager : IPasswordManager
    {
        private readonly IAuditWorker _auditWorker;

        private readonly ITranslator _translator;

        private readonly IIdentityManagersProvider _identityManagersProvider;
        private readonly IUserGetter _userGetter;
        private readonly IEmailSender _emailSender;

        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordGenerator _passwordGenerator;

        public PasswordManager(IAuditWorker auditWorker, ITranslator translator, IIdentityManagersProvider identityManagersProvider, IUserGetter userGetter, IEmailSender emailSender, IPasswordHasher passwordHasher, IPasswordGenerator passwordGenerator)
        {
            _auditWorker = auditWorker;
            _translator = translator;
            _identityManagersProvider = identityManagersProvider;
            _userGetter = userGetter;
            _emailSender = emailSender;
            _passwordHasher = passwordHasher;
            _passwordGenerator = passwordGenerator;
        }

        public async Task ChangeUserPassword(string userName, string password)
        {
            User user = await _userGetter.GetUser(userName);

            bool generate = password is null;

            if (generate)
            {
                password = _passwordGenerator.GeneratePassword();
            }

            await ChangeUserPassword(user, password, generate);
        }

        public async Task ChangeUserPassword(Guid userId, string password, bool sendToEmail = false)
        {
            User user = await _userGetter.GetUser(userId);

            await ChangeUserPassword(user, password, sendToEmail);
        }

        public async Task ChangeUserPassword(User user, string password, bool sendToEmail = false)
        {
            AuditException auditException = await _userGetter.UserExistAsync(user.UserName) switch
            {
                IUserGetter.UserExist.Ldap => new ActiveDirectoryUserOperationLockException(),
                IUserGetter.UserExist.NotFound => new UserNotExistException(),
                _ => null,
            };

            if (auditException is not null)
            {
                throw auditException;
            }

            if (password is null)
            {
                throw new PasswordInvalidChangeException(PasswordInvalidChangeVariant.Compare);
            }

            if (!sendToEmail)
            {
                IList<string> oldPasswords = await _identityManagersProvider.GetPasswordClaims(user).ContinueWith(t => t.Result.ToList());
                oldPasswords.Add(user.PasswordHash);

                foreach (string passwordClaims in oldPasswords)
                {
                    if (!_passwordHasher.ComparePassword(user.UserName, passwordClaims, password))
                    {
                        throw new PasswordInvalidChangeException(PasswordInvalidChangeVariant.Compare);
                    }
                }
            }

            string oldPassword = user.PasswordHash;

            User identityUser = await _identityManagersProvider.UpdateSecurityStampAsync(user.UserName);

            await _identityManagersProvider.ChangeUserPasswordAsync(identityUser, password);

            await _identityManagersProvider.RemoveGeneratrePasswordClaim(identityUser);

            if (sendToEmail)
            {
                string subject = _translator.GetUserText<IPasswordManager>(nameof(SenderContract.Subject));
                string prefixMsg = _translator.GetUserText<IPasswordManager>(nameof(IEmailSender.Send));

                _emailSender.Send(new SenderContract($"{prefixMsg}\n{password}", user.Email, subject));

                await _identityManagersProvider.AddPasswordClaim(identityUser, oldPassword);
            }

            string message = _translator.GetUserText<IPasswordManager>(nameof(ChangeUserPassword));

            AuditCreateCommand auditCreateCommand = new(user.UserName, message, nameof(IPasswordManager));

            await _auditWorker.CreateAudit(auditCreateCommand);
        }

        public async Task CreateForgotPasswordToken(string userName, string applicationUrl)
        {
            string GetForgotPasswordUrl(string applicationUrl, string userName, string token) =>
                $"{applicationUrl}/login/ChangePass?UserName={userName}&Token={token}";

            AuditException auditException = await _userGetter.UserExistAsync(userName) switch
            {
                IUserGetter.UserExist.Ldap => new ActiveDirectoryUserOperationLockException(),
                IUserGetter.UserExist.NotFound => new UserNotExistException(),
                _ => null,
            };

            if (auditException is not null)
            {
                throw auditException;
            }

            User user = await _userGetter.GetUser(userName);

            if (user.UserState is UserState.Lock)
            {
                throw new UserLockException();
            }

            string token = await _identityManagersProvider.CreateUserForgotPasswordTokenAsync(user);

            string url = GetForgotPasswordUrl(applicationUrl, userName, token);

            string subject = _translator.GetUserText<IPasswordManager>(nameof(SenderContract.Subject));
            string prefixMsg = _translator.GetUserText<IPasswordManager>(nameof(IPasswordManager.CreateForgotPasswordToken));

            _emailSender.Send(new SenderContract($"{prefixMsg}\n{url}", user.Email, subject));

            string message = _translator.GetUserText<IPasswordManager>(nameof(GetForgotPasswordUrl));

            AuditCreateCommand auditCreateCommand = new(user.UserName, message, nameof(IPasswordManager));

            await _auditWorker.CreateAudit(auditCreateCommand);
        }

        public async Task ChangeForgotPasswordToken(string userName, string token, string password)
        {
            User user = await _userGetter.GetUser(userName);

            bool tokenValide = await _identityManagersProvider.ValidateUserForgotPasswordTokenAsync(user, token);

            if (!tokenValide)
            {
                throw new InvalidUserTokenException();
            }

            await ChangeUserPassword(user, password ?? _passwordGenerator.GeneratePassword(), password is null);
        }
    }
}
