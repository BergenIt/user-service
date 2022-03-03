using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.EntityFrameworkCore;

using UserService.Core.DataInterfaces;
using UserService.Core.DataPackage;
using UserService.Core.Entity;
using UserService.Core.NotificationPackage.ContractProfileValidator;

namespace UserService.Data.EntityWorkers
{
    public class ContractProfileManager : BasePackOperation, IContractProfileManager
    {
        private readonly IDataWorker _dataWorker;
        private readonly IInternalDataGetter _dataGetter;

        private readonly IContractProfileValidator _contractProfileValidator;

        public ContractProfileManager(IDataWorker dataWorker, IInternalDataGetter dataGetter, IContractProfileValidator contractProfileValidator)
        {
            _dataWorker = dataWorker;
            _dataGetter = dataGetter;
            _contractProfileValidator = contractProfileValidator;
        }

        public async Task<ContractProfile> RemoveContractProfile(Guid contractProfileId)
        {
            ContractProfile entity = await _dataWorker.RemoveAsync<ContractProfile>(contractProfileId);
            await _dataWorker.SaveChangesAsync();

            return entity;
        }

        public async Task<ContractProfile> AddContractProfile(ContractProfile contractProfile)
        {
            _contractProfileValidator.ValidateContractProfile(contractProfile);

            contractProfile.RoleNotificationSettings = null;

            contractProfile.Id = Guid.NewGuid();

            foreach (ContractSettingLine contractConfig in contractProfile.ContractSettingLines)
            {
                contractConfig.Id = Guid.NewGuid();
                contractConfig.ContractProfileId = contractProfile.Id;
                foreach (ContractSettingPropperty contractPropperty in contractConfig.ContractPropperties)
                {
                    contractPropperty.Id = Guid.NewGuid();
                    contractPropperty.ContractSettingLineId = contractConfig.Id;
                }
            }

            _ = await _dataWorker.AddAsync(contractProfile);
            await _dataWorker.SaveChangesAsync();

            return contractProfile;
        }

        public async Task<ContractProfile> UpdateContractProfile(ContractProfile contractProfile)
        {
            _contractProfileValidator.ValidateContractProfile(contractProfile);

            ContractProfile entity = await _dataGetter
                .GetQueriable<ContractProfile>()
                .Include(s => s.ContractSettingLines)
                    .ThenInclude(c => c.ContractPropperties)
                .SingleAsync(s => s.Id == contractProfile.Id);

            entity.NotifyEventType = contractProfile.NotifyEventType;
            entity.Name = contractProfile.Name;
            entity.Comment = contractProfile.Comment;

            IEnumerable<ContractSettingLine> updatingConfigs = contractProfile.ContractSettingLines.Where(c => entity.ContractSettingLines.Any(j => j.Id == c.Id));
            IEnumerable<ContractSettingLine> newConfigs = contractProfile.ContractSettingLines.Where(c => !entity.ContractSettingLines.Any(j => j.Id == c.Id));
            ContractSettingLine[] rmConfigs = entity.ContractSettingLines.Where(c => !contractProfile.ContractSettingLines.Any(j => j.Id == c.Id)).ToArray();

            foreach (ContractSettingLine rmConfig in rmConfigs)
            {
                _ = entity.ContractSettingLines.Remove(rmConfig);
            }

            foreach (ContractSettingLine updatingConfig in updatingConfigs)
            {
                ContractSettingLine updatingEntity = entity.ContractSettingLines.Single(j => j.Id == updatingConfig.Id);
                updatingEntity.UserProppertyName = updatingConfig.UserProppertyName;
                updatingEntity.UserTemplate = updatingConfig.UserTemplate;
                updatingEntity.Enable = updatingConfig.Enable;
                updatingEntity.LineNumber = updatingConfig.LineNumber;

                IEnumerable<ContractSettingPropperty> newContractPropperties = updatingConfig.ContractPropperties.Where(c => c.Id == Guid.Empty || !updatingEntity.ContractPropperties.Any(j => j.Id == c.Id));
                IEnumerable<ContractSettingPropperty> updateingContractPropperties = updatingConfig.ContractPropperties.Where(c => updatingEntity.ContractPropperties.Any(j => j.Id == c.Id));
                ContractSettingPropperty[] rmContractPropperties = updatingEntity.ContractPropperties.Where(c => !updatingConfig.ContractPropperties.Any(j => j.Id == c.Id)).ToArray();

                foreach (ContractSettingPropperty rmContractPropperty in rmContractPropperties)
                {
                    _ = updatingEntity.ContractPropperties.Remove(rmContractPropperty);
                }

                foreach (ContractSettingPropperty updateingContractPropperty in updateingContractPropperties)
                {
                    ContractSettingPropperty contractPropperty = updatingEntity.ContractPropperties.Single(c => c.Id == updateingContractPropperty.Id);
                    contractPropperty.Position = updateingContractPropperty.Position;
                    contractPropperty.ContractName = updateingContractPropperty.ContractName;
                }

                foreach (ContractSettingPropperty newContractPropperty in newContractPropperties)
                {
                    updatingEntity.ContractPropperties.Add(newContractPropperty);
                }
            }

            foreach (ContractSettingLine newConfig in newConfigs)
            {
                entity.ContractSettingLines.Add(newConfig);
            }

            _ = _dataWorker.Update(entity);
            await _dataWorker.SaveChangesAsync();

            return entity;
        }

        public Task<IEnumerable<ContractProfile>> RemoveContractProfile(IEnumerable<Guid> contractProfileIds) =>
            EntityPackOperationAsync(contractProfileIds, RemoveContractProfile);

        public Task<IEnumerable<ContractProfile>> UpdateContractProfile(IEnumerable<ContractProfile> contractProfiles) =>
            EntityPackOperationAsync<ContractProfile>(contractProfiles, UpdateContractProfile);

        public Task<IEnumerable<ContractProfile>> AddContractProfile(IEnumerable<ContractProfile> contractProfiles) =>
            EntityPackOperationAsync<ContractProfile>(contractProfiles, AddContractProfile);
    }
}
