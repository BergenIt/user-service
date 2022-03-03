using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Grpc.Core;

using UserService.Core.Elasticsearch;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    ///  Сервис работы с настройками политик эластика
    /// </summary>
    public class LifecycleIndexServices : LifecycleIndexService.LifecycleIndexServiceBase
    {
        private readonly ILifecycleIndexManager _lifecycleIndexManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// Сервис работы с настройками политик эластика
        /// </summary>
        /// <param name="lifecycleIndexManager"></param>
        /// <param name="mapper"></param>
        public LifecycleIndexServices(ILifecycleIndexManager lifecycleIndexManager, IMapper mapper)
        {
            _lifecycleIndexManager = lifecycleIndexManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить политики аудита
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<LifecycleIndices> GetAuditLifecycleIndex(GetLifecycleIndexRequest request, ServerCallContext context)
        {
            IEnumerable<Core.Models.PolicyPhases> policyPhases = await _lifecycleIndexManager.GetAuditPhases();

            IEnumerable<LifecycleIndex> lifecycleIndices = _mapper.Map<IEnumerable<LifecycleIndex>>(policyPhases);

            return new()
            {
                LifecycleIndexList = { lifecycleIndices }
            };
        }

        /// <summary>
        /// Получить политики уведомлений
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<LifecycleIndices> GetNotificationLifecycleIndex(GetLifecycleIndexRequest request, ServerCallContext context)
        {
            IEnumerable<Core.Models.PolicyPhases> policyPhases = await _lifecycleIndexManager.GetNotificationPhases();

            IEnumerable<LifecycleIndex> lifecycleIndices = _mapper.Map<IEnumerable<LifecycleIndex>>(policyPhases);

            return new()
            {
                LifecycleIndexList = { lifecycleIndices }
            };
        }

        /// <summary>
        /// Обновить политики
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<LifecycleIndices> UpdateLifecycleIndex(LifecycleIndices request, ServerCallContext context)
        {
            IEnumerable<Core.Models.PolicyPhases> requestPolicyPhases = _mapper.Map<IEnumerable<Core.Models.PolicyPhases>>(request.LifecycleIndexList);

            await _lifecycleIndexManager.UpdatePhases(requestPolicyPhases);

            return new()
            {
                LifecycleIndexList = { request.LifecycleIndexList }
            };
        }
    }
}
