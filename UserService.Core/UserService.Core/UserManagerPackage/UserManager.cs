using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension.Translator;

using UserService.Core.AuditPackage.AuditException;
using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.PasswordGenerator;
using UserService.Core.PasswordManager;
using UserService.Core.PolindromHasher;
using UserService.Core.SenderInteraces;

namespace UserService.Core.UserManager
{
    public class UserManager : IUserManager
    {
        private readonly ITranslator _translator;

        private readonly IEmailSender _emailSender;

        private readonly IDataWorker _dataWorker;
        private readonly IUserGetter _userGetter;

        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IPasswordManager _passwordManager;
        private readonly IPasswordHasher _passwordHasher;

        private readonly IIdentityManagersProvider _identityManagersProvider;

        public UserManager(ITranslator translator, IEmailSender emailSender, IDataWorker dataWorker, IUserGetter userGetter, IPasswordGenerator passwordGenerator, IPasswordManager passwordManager, IPasswordHasher passwordHasher, IIdentityManagersProvider identityManagersProvider)
        {
            _translator = translator;
            _emailSender = emailSender;
            _dataWorker = dataWorker;
            _userGetter = userGetter;
            _passwordGenerator = passwordGenerator;
            _passwordManager = passwordManager;
            _passwordHasher = passwordHasher;
            _identityManagersProvider = identityManagersProvider;
        }

        public async Task<IEnumerable<User>> CreateUsers(IEnumerable<UserCreateCommand> userCreateCommands)
        {
            List<User> users = new();

            foreach (UserCreateCommand command in userCreateCommands)
            {
                string password = command.PasswordAction switch
                {
                    PasswordActionEnum.AutogenerateToEmail => _passwordGenerator.GeneratePassword(),
                    PasswordActionEnum.Password => command.Password,
                    _ => string.Empty,
                };

                User user = new()
                {
                    Id = Guid.NewGuid(),
                    UserName = command.UserName,
                    Description = command.Description,
                    Email = command.Email,
                    FullName = command.FullName,
                    RequestNumber = command.RequestNumber,
                    LastLogin = null,
                    LockoutEnabled = true,
                    LockoutEnd = null,
                    PasswordExpiration = command.PasswordExpirations,
                    UserExpiration = command.UserExpiration,
                    PositionId = command.PositionId,
                    SubdivisionId = command.SubdivisionId,
                    UserLock = command.UserLock,
                    RegistredDate = DateTime.UtcNow,
                };

                await _identityManagersProvider.CreateUserAsync(user, password);

                IEnumerable<UserRole> userRoles = command.RoleIds.Select(r => new UserRole { UserId = user.Id, RoleId = r });

                await _dataWorker.AddRangeAsync(userRoles);

                user.UserRoles = userRoles.ToList();

                users.Add(user);

                if (command.PasswordAction is PasswordActionEnum.AutogenerateToEmail)
                {
                    string subject = _translator.GetUserText<IPasswordManager>(nameof(SenderContract.Subject));
                    string prefixMsg = _translator.GetUserText<IPasswordManager>(nameof(IPasswordManager.ChangeUserPassword));

                    _emailSender.Send(new SenderContract($"{prefixMsg}\n{password}", user.Email, subject));

                    await _identityManagersProvider.AddPasswordClaim(user, string.Empty);
                }
            }

            await _dataWorker.SaveChangesAsync();

            return users;
        }

        public async Task<IEnumerable<User>> RemoveUsers(IEnumerable<Guid> ids)
        {
            IEnumerable<User> users = await _dataWorker.RemoveRangeAsync<User>(ids);

            await _dataWorker.SaveChangesAsync();

            return users;
        }

        public async Task<IEnumerable<User>> UpdateUsers(IEnumerable<UserUpdateCommand> userUpdateCommands)
        {
            List<User> users = new();

            foreach (UserUpdateCommand command in userUpdateCommands)
            {
                string password = command.PasswordAction switch
                {
                    PasswordActionEnum.AutogenerateToEmail => _passwordGenerator.GeneratePassword(),
                    PasswordActionEnum.Password => command.Password,
                    PasswordActionEnum.WithoutChange => null,
                    _ => throw new NotImplementedException(nameof(command.PasswordAction)),
                };

                if (string.IsNullOrWhiteSpace(password) && command.PasswordAction is not PasswordActionEnum.WithoutChange)
                {
                    throw new PasswordInputException();
                }

                User user = await _dataWorker.UpdateAsync<User>(command.Id,
                    u =>
                    {
                        u.FullName = command.FullName;
                        u.Description = command.Description;
                        u.RequestNumber = command.RequestNumber;

                        u.PasswordExpiration = command.PasswordExpirations;
                        u.UserExpiration = command.UserExpiration;
                        u.UserLock = command.UserLock;

                        u.Email = command.Email;
                        u.PositionId = command.PositionId;
                        u.SubdivisionId = command.SubdivisionId;
                    }
                );

                if (password is not null && _passwordHasher.ComparePassword(user.UserName, user.PasswordHash, password))
                {
                    await _passwordManager.ChangeUserPassword(user, password, command.PasswordAction is PasswordActionEnum.AutogenerateToEmail);
                }

                IEnumerable<Role> roles = await _userGetter.GetUserRoles(user.Id);

                IEnumerable<UserRole> rmRoles = roles.Where(r => !command.RoleIds.Contains(r.Id)).SelectMany(r => r.UserRoles);
                IEnumerable<UserRole> addRoles = command.RoleIds.Where(r => !roles.Any(x => x.Id == r)).Select(r => new UserRole { RoleId = r, UserId = command.Id });

                await _dataWorker.AddRangeAsync(addRoles);
                _ = _dataWorker.RemoveRange(rmRoles);

                users.Add(user);
            }

            await _dataWorker.SaveChangesAsync();

            return users;
        }
    }
}
