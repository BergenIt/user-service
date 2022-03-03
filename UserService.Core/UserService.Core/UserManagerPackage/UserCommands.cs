using System;
using System.Collections.Generic;

namespace UserService.Core.UserManager
{
    public record UserUpdateCommand(
        Guid Id,
        string Email,
        Guid SubdivisionId,
        Guid? PositionId,
        string Description,
        string RequestNumber,
        string FullName,
        DateTime UserExpiration,
        DateTime PasswordExpirations,
        IEnumerable<Guid> RoleIds,
        bool UserLock,
        PasswordActionEnum PasswordAction,
        string Password
    ) : UserCommand(Email, SubdivisionId, PositionId, Description, RequestNumber, FullName, UserExpiration, PasswordExpirations, RoleIds, UserLock, PasswordAction, Password);

    public record UserCreateCommand(
        string UserName,
        string Email,
        Guid SubdivisionId,
        Guid? PositionId,
        string Description,
        string RequestNumber,
        string FullName,
        DateTime UserExpiration,
        DateTime PasswordExpirations,
        IEnumerable<Guid> RoleIds,
        bool UserLock,
        PasswordActionEnum PasswordAction,
        string Password
    ) : UserCommand(Email, SubdivisionId, PositionId, Description, RequestNumber, FullName, UserExpiration, PasswordExpirations, RoleIds, UserLock, PasswordAction, Password);

    public abstract record UserCommand(
        string Email,
        Guid SubdivisionId,
        Guid? PositionId,
        string Description,
        string RequestNumber,
        string FullName,
        DateTime UserExpiration,
        DateTime PasswordExpirations,
        IEnumerable<Guid> RoleIds,
        bool UserLock,
        PasswordActionEnum PasswordAction,
        string Password
    );
}
