using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Grpc.Core;

using UserService.Core.DataInterfaces;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис работы с правами доступа пользователей к объектам
    /// </summary>
    public class UserAccessObjectServices : UserAccessObjectService.UserAccessObjectServiceBase
    {
        private readonly IMapper _mapper;
        private readonly IAccessObjectManager _accessObjectManager;

        /// <summary>
        /// Сервис работы с правами доступа пользователей к объектам
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="accessObjectManager"></param>
        public UserAccessObjectServices(IMapper mapper, IAccessObjectManager accessObjectManager)
        {
            _mapper = mapper;
            _accessObjectManager = accessObjectManager;
        }

        /// <summary>
        /// Получить права доступа пользователя на объекты
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserAccessObjects> GetUserAccessObjects(GetUserAccessObjectsRequest request, ServerCallContext context)
        {
            Guid userId = Guid.Parse(request.UserId);

            IEnumerable<string> objectIds = await _accessObjectManager.GetUserAccessObjects(userId);

            return new()
            {
                UserId = userId.ToString(),
                AccessObjectIds = { objectIds }
            };
        }

        /// <summary>
        /// Получить пользователей которым доступен объект
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AccessObjectUsers> GetAccessObjectUsers(GetAccessObjectUsersRequest request, ServerCallContext context)
        {
            IEnumerable<Guid> userIds = await _accessObjectManager.GetAccessObjectUsers(request.AccessObjectId);

            return new()
            {
                UserIds = { _mapper.Map<IEnumerable<string>>(userIds) },
                AccessObjectId = request.AccessObjectId,
            };
        }

        /// <summary>
        /// Добавить объекты в доступ пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserAccessObjects> AddAccessObjectsToUser(AddAccessObjectsToUserCommand request, ServerCallContext context)
        {
            Guid userId = Guid.Parse(request.UserId);

            IEnumerable<string> objectIds = await _accessObjectManager.AddAccessObjectsToUser(userId, request.AddAccessObjectIds);

            return new()
            {
                UserId = userId.ToString(),
                AccessObjectIds = { objectIds }
            };
        }

        /// <summary>
        /// Добавить пользователей в права доступа объекта
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AccessObjectUsers> AddUsersToAccessObject(AddUsersToAccessObjectCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> inputUserIds = _mapper.Map<IEnumerable<Guid>>(request.AddUserIds);

            IEnumerable<Guid> userIds = await _accessObjectManager.AddUsersToAccessObject(request.AccessObjectId, inputUserIds);

            return new()
            {
                UserIds = { _mapper.Map<IEnumerable<string>>(userIds) },
                AccessObjectId = request.AccessObjectId,
            };
        }

        /// <summary>
        /// Удалить объекты из доступа юзера
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserAccessObjects> RemoveAccessObjectsFromUser(RemoveAccessObjectsFromUserCommand request, ServerCallContext context)
        {
            Guid userId = Guid.Parse(request.UserId);

            IEnumerable<string> objectIds = await _accessObjectManager.RemoveAccessObjectsFromUser(userId, request.RemoveAccessObjectIds);

            return new()
            {
                UserId = userId.ToString(),
                AccessObjectIds = { objectIds }
            };
        }

        /// <summary>
        /// Удалить права доступа у пользователей на объект
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AccessObjectUsers> RemoveUsersFromAccessObject(RemoveUsersFromAccessObjectCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> inputUserIds = _mapper.Map<IEnumerable<Guid>>(request.RemoveUserIds);

            IEnumerable<Guid> userIds = await _accessObjectManager.RemoveUsersFromAccessObject(request.AccessObjectId, inputUserIds);

            return new()
            {
                UserIds = { _mapper.Map<IEnumerable<string>>(userIds) },
                AccessObjectId = request.AccessObjectId,
            };
        }
    }
}
