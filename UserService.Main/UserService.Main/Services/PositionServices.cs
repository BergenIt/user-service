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
    /// Сервис работы с должностями
    /// </summary>
    public class PositionServices : PositionService.PositionServiceBase
    {
        private readonly IMapper _mapper;
        private readonly IEntityManager<Core.Entity.Position> _positionManager;

        /// <summary>
        /// Сервис работы с должностями
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="positionManager"></param>
        public PositionServices(IMapper mapper, IEntityManager<Core.Entity.Position> positionManager)
        {
            _mapper = mapper;
            _positionManager = positionManager;
        }

        /// <summary>
        /// Получить должность по id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Position> GetPositionById(PositionGetRequest request, ServerCallContext context)
        {
            Guid PositionId = Guid.Parse(request.Id);

            Core.Entity.Position Position = await _positionManager.GetEntity(PositionId);

            return _mapper.Map<Position>(Position);
        }

        /// <summary>
        /// Получить должности
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<PositionPage> GetPositions(PositionsGetRequest request, ServerCallContext context)
        {
            FilterContract filter = request.Filter.FromProtoFilter<Position, Core.Entity.Position>();

            IPageItems<Core.Entity.Position> positions = await _positionManager.GetEntitites(filter);

            return new()
            {
                PositionList = { _mapper.Map<IEnumerable<Position>>(positions.Items) },
                CountItems = (int)positions.CountItems
            };
        }

        /// <summary>
        /// Создать должности
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Positions> CreatePositions(PositionsCreateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.Position> Positions = _mapper.Map<IEnumerable<Core.Entity.Position>>(request.CreatePositionsList);

            IEnumerable<Core.Entity.Position> addedPositions = await _positionManager.AddEntitites(Positions);

            return new()
            {
                PositionList = { _mapper.Map<IEnumerable<Position>>(addedPositions) }
            };
        }

        /// <summary>
        /// Обновить должности
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Positions> UpdatePositions(PositionsUpdateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.Position> Positions = _mapper.Map<IEnumerable<Core.Entity.Position>>(request.UpdatePositionsList);

            IEnumerable<Core.Entity.Position> updatedPositions = await _positionManager.UpdateEntitites(Positions);

            return new()
            {
                PositionList = { _mapper.Map<IEnumerable<Position>>(updatedPositions) }
            };
        }

        /// <summary>
        /// Удалить должности
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Positions> RemovePositions(PositionsRemoveCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> PositionIds = request.RemovePositionsId.Select(r => Guid.Parse(r));

            IEnumerable<Core.Entity.Position> removedPositions = await _positionManager.RemoveEntitites(PositionIds);

            return new()
            {
                PositionList = { _mapper.Map<IEnumerable<Position>>(removedPositions) }
            };
        }
    }
}
