using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DatabaseExtension;

using Grpc.Core;

using UserService.Core.DataInterfaces;
using UserService.Core.NotifyEventTypeGetter;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис работы с контрактами уведомлений
    /// </summary>
    public class ContractProfileServices : ContractProfileService.ContractProfileServiceBase
    {
        private readonly IMapper _mapper;

        private readonly IContractProfileManager _contractProfileManager;
        private readonly IContractProfileGetter _contractProfileGetter;

        private readonly INotifyEventTypeGetter _notifyEventTypeGetter;

        /// <summary>
        /// Сервис работы с контрактами уведомлений
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="contractProfileManager"></param>
        /// <param name="contractProfileGetter"></param>
        /// <param name="notifyEventTypeGetter"></param>
        public ContractProfileServices(IMapper mapper, IContractProfileManager contractProfileManager, IContractProfileGetter contractProfileGetter, INotifyEventTypeGetter notifyEventTypeGetter)
        {
            _mapper = mapper;
            _contractProfileManager = contractProfileManager;
            _contractProfileGetter = contractProfileGetter;
            _notifyEventTypeGetter = notifyEventTypeGetter;
        }

        /// <summary>
        /// Получить профиль контракта по его Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ContractProfile> GetContractProfileById(ContractProfileGetRequest request, ServerCallContext context)
        {
            Guid contractProfileId = Guid.Parse(request.Id);

            Core.Entity.ContractProfile contractProfile = await _contractProfileGetter.GetContractProfile(contractProfileId);

            return _mapper.Map<ContractProfile>(contractProfile);
        }

        /// <summary>
        /// Получить профили контрактов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ContractProfilePage> GetContractProfiles(ContractProfilesGetRequest request, ServerCallContext context)
        {
            FilterContract filter = request.Filter.FromProtoFilter<ContractProfile, Core.Entity.ContractProfile>();

            IPageItems<Core.Entity.ContractProfile> contractProfiles = await _contractProfileGetter.GetContractProfiles(filter);

            return new()
            {
                ContractProfileList = { _mapper.Map<IEnumerable<ContractProfile>>(contractProfiles.Items) },
                CountItems = (int)contractProfiles.CountItems,
            };
        }

        /// <summary>
        /// Создать профили контрактов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ContractProfiles> CreateContractProfiles(ContractProfilesCreateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.ContractProfile> contractProfiles = _mapper.Map<IEnumerable<Core.Entity.ContractProfile>>(request.CreateContractProfilesList);

            IEnumerable<Core.Entity.ContractProfile> addedContractProfiles = await _contractProfileManager.AddContractProfile(contractProfiles);

            return new()
            {
                ContractProfileList = { _mapper.Map<IEnumerable<ContractProfile>>(addedContractProfiles) }
            };
        }

        /// <summary>
        /// Обновить профили контрактов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ContractProfiles> UpdateContractProfiles(ContractProfilesUpdateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.ContractProfile> contractProfiles = _mapper.Map<IEnumerable<Core.Entity.ContractProfile>>(request.UpdateContractProfilesList);

            IEnumerable<Core.Entity.ContractProfile> updatedContractProfiles = await _contractProfileManager.UpdateContractProfile(contractProfiles);

            return new()
            {
                ContractProfileList = { _mapper.Map<IEnumerable<ContractProfile>>(updatedContractProfiles) }
            };
        }

        /// <summary>
        /// Удалить профили контрактов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ContractProfiles> RemoveContractProfiles(ContractProfilesRemoveCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> contractProfileIds = request.RemoveContractProfilesId.Select(r => Guid.Parse(r));

            IEnumerable<Core.Entity.ContractProfile> removedContractProfiles = await _contractProfileManager.RemoveContractProfile(contractProfileIds);

            return new()
            {
                ContractProfileList = { _mapper.Map<IEnumerable<ContractProfile>>(removedContractProfiles) }
            };
        }

        /// <summary>
        /// Получить шаблон имен переменных для работы с профилем контракта
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ContractProfileProppertyTemplate> GetContractProppertyTemplate(NotifyTypeProppertyRequest request, ServerCallContext context)
        {
            string sourceEventType = _notifyEventTypeGetter.GetSourceNotifyEventType(request.NotifyType);

            IDictionary<string, string> propperties = _notifyEventTypeGetter.GetNotifyEventTypePropperties(sourceEventType);

            return Task.FromResult<ContractProfileProppertyTemplate>(new()
            {
                ContractName = { propperties.Select(p => p.Value) }
            });
        }
    }
}
