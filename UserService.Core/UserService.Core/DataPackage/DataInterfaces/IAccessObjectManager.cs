using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserService.Core.DataInterfaces
{
    public interface IAccessObjectManager
    {
        Task<IEnumerable<string>> GetUserAccessObjects(Guid userId);
        Task<IEnumerable<Guid>> GetAccessObjectUsers(string accessObjectId);

        Task<IEnumerable<string>> AddAccessObjectsToUser(Guid userId, IEnumerable<string> accessObjectIds);
        Task<IEnumerable<Guid>> AddUsersToAccessObject(string accessObjectId, IEnumerable<Guid> userIds);

        Task<IEnumerable<string>> RemoveAccessObjectsFromUser(Guid userId, IEnumerable<string> accessObjectIds);
        Task<IEnumerable<Guid>> RemoveUsersFromAccessObject(string accessObjectId, IEnumerable<Guid> userIds);
    }
}
