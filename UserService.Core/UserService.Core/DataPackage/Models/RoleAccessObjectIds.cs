using System;
using System.Collections.Generic;

namespace UserService.Core.Models
{
    public record RoleAccessObjectIds(Guid RoleId, IEnumerable<string> AccessObjectIds);
}
