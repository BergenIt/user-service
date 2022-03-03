using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using UserService.Core.AuditPackage.AuditException;
using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.JwtGenerator;
using UserService.Core.Models;

namespace UserService.Data.IdentityManagersProvider
{
    public class IdentityManagersProvider : IIdentityManagersProvider
    {
        private const string PasswordTag = JwtRegisteredClaimNames.Prn;

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        private readonly IContextManager _contextManager;

        public IdentityManagersProvider(SignInManager<User> signInManager, UserManager<User> userManager, IContextManager contextManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;

            _contextManager = contextManager;
        }

        public Task<IEnumerable<string>> GetPasswordClaims(User user)
        {
            return _userManager.GetClaimsAsync(user)
                .ContinueWith(c =>
                    c.Result
                        .Where(c => c.ValueType == PasswordTag)
                        .Select(p => p.Value)
                );
        }

        public Task<Claim> GetGeneratrePasswordClaim(User user)
        {
            return _userManager.GetClaimsAsync(user).ContinueWith(r => r.Result.SingleOrDefault(c => c.Type == JwtGenerator.GeneratePassword));
        }

        public Task RemoveGeneratrePasswordClaim(User user)
        {
            return _userManager.RemoveClaimAsync(user, new(JwtGenerator.GeneratePassword, true.ToString()));
        }

        public async Task AddPasswordClaim(User user, string oldPasword)
        {
            if (!string.IsNullOrWhiteSpace(oldPasword))
            {
                _ = await _userManager.AddClaimAsync(user, new(PasswordTag, oldPasword));
            }

            Claim claim = new(JwtGenerator.GeneratePassword, true.ToString());

            _ = await _userManager.AddClaimAsync(user, claim);
        }

        public async Task ChangeUserPasswordAsync(User user, string password)
        {
            bool isValid = true;

            foreach (IPasswordValidator<User> passwordValidator in _userManager.PasswordValidators)
            {
                IdentityResult validatorResult = await passwordValidator.ValidateAsync(_userManager, user, password);

                isValid = isValid && validatorResult.Succeeded;
            }

            if (!isValid)
            {
                throw new PasswordInvalidChangeException(PasswordInvalidChangeVariant.Compare);
            }

            _ = await _userManager.RemovePasswordAsync(user);
            _ = await _userManager.AddPasswordAsync(user, password);
        }

        public async Task CreateUserAsync(User user, string password)
        {
            IdentityResult identityResult = await (
                password == string.Empty
                    ? _userManager.CreateAsync(user)
                    : _userManager.CreateAsync(user, password)
            );

            if (!identityResult.Succeeded)
            {
                throw new Exception(nameof(CreateUserAsync));
            }

            await _contextManager.SaveEntryToCache(new SavedEntry[] { new SavedEntry(Microsoft.EntityFrameworkCore.EntityState.Added, user) });
        }

        public async Task PasswordSignInAsync(string userName, string password)
        {
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(userName, password, isPersistent: false, lockoutOnFailure: true);

            if (!signInResult.Succeeded)
            {
                throw new PasswordInputException();
            }
        }

        public async Task<User> UpdateSecurityStampAsync(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);

            IdentityResult result = await _userManager.UpdateSecurityStampAsync(user);

            if (!result.Succeeded)
            {
                throw new PasswordInvalidChangeException(PasswordInvalidChangeVariant.BasePolicy);
            }

            return user;
        }

        public Task<string> CreateUserForgotPasswordTokenAsync(User user)
        {
            return _userManager.GeneratePasswordResetTokenAsync(user)
                .ContinueWith(u => Convert.ToBase64String(Encoding.UTF8.GetBytes(u.Result)));
        }

        public Task<bool> ValidateUserForgotPasswordTokenAsync(User user, string token)
        {
            string tokenFromBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            return _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", tokenFromBase64);
        }
    }
}
