using System;
using System.Linq;

using AutoMapper;

using UserService.Proto;

namespace UserService.Main.Automapper
{
    internal class UserCommandCoverter : ITypeConverter<UserCreateCommand, Core.UserManager.UserCreateCommand>, ITypeConverter<UserUpdateCommand, Core.UserManager.UserUpdateCommand>
    {
        private const int PasswordExpirationDays = 90;

        public Core.UserManager.UserCreateCommand Convert(UserCreateCommand source, Core.UserManager.UserCreateCommand destination, ResolutionContext context)
        {
            Core.UserManager.PasswordActionEnum passwordActionEnum = source.UserPasswordCase switch
            {
                UserCreateCommand.UserPasswordOneofCase.AutogenerateToEmail => Core.UserManager.PasswordActionEnum.AutogenerateToEmail,
                UserCreateCommand.UserPasswordOneofCase.Password => Core.UserManager.PasswordActionEnum.Password,
                _ => throw new NotImplementedException($"{source.UserPasswordCase}"),
            };

            return new(
                source.UserName,
                source.Email,
                Guid.Parse(source.SubdivisionId),
                Guid.TryParse(source.PositionId, out Guid result) ? result : null,
                source.Description,
                source.RequestNumber,
                source.FullName,
                context.Mapper.Map<DateTime>(source.UserExpiration),
                DateTime.UtcNow.AddDays(PasswordExpirationDays),
                source.RoleIds.Select(i => Guid.Parse(i)),
                source.UserLock,
                passwordActionEnum,
                passwordActionEnum is Core.UserManager.PasswordActionEnum.Password ? source.Password : null
            );
        }

        public Core.UserManager.UserUpdateCommand Convert(UserUpdateCommand source, Core.UserManager.UserUpdateCommand destination, ResolutionContext context)
        {
            Core.UserManager.PasswordActionEnum passwordActionEnum = source.UserPasswordCase switch
            {
                UserUpdateCommand.UserPasswordOneofCase.AutogenerateToEmail => Core.UserManager.PasswordActionEnum.AutogenerateToEmail,
                UserUpdateCommand.UserPasswordOneofCase.Password => Core.UserManager.PasswordActionEnum.Password,
                UserUpdateCommand.UserPasswordOneofCase.WithoutChange => Core.UserManager.PasswordActionEnum.WithoutChange,
                UserUpdateCommand.UserPasswordOneofCase.None => Core.UserManager.PasswordActionEnum.WithoutChange,
                _ => throw new NotImplementedException($"{source.UserPasswordCase}"),
            };

            return new(
                Guid.Parse(source.Id),
                source.Email,
                Guid.Parse(source.SubdivisionId),
                Guid.TryParse(source.PositionId, out Guid result) ? result : null,
                source.Description,
                source.RequestNumber,
                source.FullName,
                context.Mapper.Map<DateTime>(source.UserExpiration),
                DateTime.UtcNow.AddDays(PasswordExpirationDays),
                source.RoleIds.Select(i => Guid.Parse(i)),
                source.UserLock,
                passwordActionEnum,
                passwordActionEnum is Core.UserManager.PasswordActionEnum.Password ? source.Password : null
            );
        }
    }
}
