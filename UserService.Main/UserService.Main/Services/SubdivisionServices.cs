using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DatabaseExtension;

using Grpc.Core;

using UserService.Core.DataInterfaces;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис работы с структурными подразделениями
    /// </summary>
    public class SubdivisionServices : SubdivisionService.SubdivisionServiceBase
    {
        private readonly IMapper _mapper;
        private readonly IEntityManager<Core.Entity.Subdivision> _subdivisionManager;

        /// <summary>
        /// Сервис работы с структурными подразделениями
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="subdivisionManager"></param>
        public SubdivisionServices(IMapper mapper, IEntityManager<Core.Entity.Subdivision> subdivisionManager)
        {
            _mapper = mapper;
            _subdivisionManager = subdivisionManager;
        }

        /// <summary>
        /// Получить структурное подразделение по Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Subdivision> GetSubdivisionById(SubdivisionGetRequest request, ServerCallContext context)
        {
            Guid SubdivisionId = Guid.Parse(request.Id);

            Core.Entity.Subdivision Subdivision = await _subdivisionManager.GetEntity(SubdivisionId);

            return _mapper.Map<Subdivision>(Subdivision);
        }

        /// <summary>
        /// Получить структурные подразделения
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<SubdivisionPage> GetSubdivisions(SubdivisionsGetRequest request, ServerCallContext context)
        {
            FilterContract filter = request.Filter.FromProtoFilter<Subdivision, Core.Entity.Subdivision>();

            IPageItems<Core.Entity.Subdivision> subdivisions = await _subdivisionManager.GetEntitites(filter);

            return new()
            {
                SubdivisionList = { _mapper.Map<IEnumerable<Subdivision>>(subdivisions.Items) },
                CountItems = (int)subdivisions.CountItems
            };
        }

        /// <summary>
        /// Создать структурные подразделения
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Subdivisions> CreateSubdivisions(SubdivisionsCreateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.Subdivision> Subdivisions = _mapper.Map<IEnumerable<Core.Entity.Subdivision>>(request.CreateSubdivisionsList);

            IEnumerable<Core.Entity.Subdivision> addedSubdivisions = await _subdivisionManager.AddEntitites(Subdivisions);

            return new()
            {
                SubdivisionList = { _mapper.Map<IEnumerable<Subdivision>>(addedSubdivisions) }
            };
        }

        /// <summary>
        /// Обновить структурные подразделения
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Subdivisions> UpdateSubdivisions(SubdivisionsUpdateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.Subdivision> Subdivisions = _mapper.Map<IEnumerable<Core.Entity.Subdivision>>(request.UpdateSubdivisionsList);

            IEnumerable<Core.Entity.Subdivision> updatedSubdivisions = await _subdivisionManager.UpdateEntitites(Subdivisions);

            return new()
            {
                SubdivisionList = { _mapper.Map<IEnumerable<Subdivision>>(updatedSubdivisions) }
            };
        }

        /// <summary>
        /// Удалить структурные подразделения
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Subdivisions> RemoveSubdivisions(SubdivisionsRemoveCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> SubdivisionIds = request.RemoveSubdivisionsId.Select(r => Guid.Parse(r));

            IEnumerable<Core.Entity.Subdivision> removedSubdivisions = await _subdivisionManager.RemoveEntitites(SubdivisionIds);

            return new()
            {
                SubdivisionList = { _mapper.Map<IEnumerable<Subdivision>>(removedSubdivisions) }
            };
        }
    }
}
