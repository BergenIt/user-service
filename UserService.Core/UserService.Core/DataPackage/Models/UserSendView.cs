using System;
using System.Collections.Generic;

using UserService.Core.Entity;

namespace UserService.Core.Models
{
    public record UserGroupView(Guid ContractProfileId, IEnumerable<TargetNotify> TargetNotifies, IEnumerable<UserSendView> UserSend);

    public class UserSendView
    {
        public string Email { get; set; }
        public string UserName { get; set; }

        public Guid ContractProfileId { get; set; }
        public IEnumerable<TargetNotify> TargetNotifies { get; set; }
    }
}

