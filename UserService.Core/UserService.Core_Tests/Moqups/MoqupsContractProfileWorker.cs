using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsContractProfileGetter : IContractProfileGetter
    {
        public Task<IEnumerable<ContractProfile>> GetContractProfilesWithWebhooks(string eventType)
        {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(eventType));

            Guid guid = Guid.NewGuid();

            ContractProfile contractProfile = GetDefaultProfile(guid);

            return Task.FromResult<IEnumerable<ContractProfile>>(new ContractProfile[] { contractProfile });
        }

        public Task<IEnumerable<ContractProfile>> GetUserContractProfiles(string userName)
        {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(userName));

            Guid guid = Guid.NewGuid();

            ContractProfile contractProfile = GetDefaultProfile(guid);

            return Task.FromResult<IEnumerable<ContractProfile>>(new ContractProfile[] { contractProfile });
        }

        public Task<IEnumerable<UserSendView>> GetUsersFromContractProfiles(IEnumerable<Guid> contractProfileIds, string _)
        {
            Assert.IsTrue(contractProfileIds.Any());

            IEnumerable<UserSendView> userSendViews = contractProfileIds.Select(id => new UserSendView
            {
                Email = "testEmail",
                UserName = "testName",
                ContractProfileId = id,
                TargetNotifies = new TargetNotify[] { TargetNotify.Email, TargetNotify.Socket }
            });

            return Task.FromResult(userSendViews);
        }

        private static ContractProfile GetDefaultProfile(Guid guid)
        {
            return new()
            {
                Id = guid,
                NotifyEventType = "test",
                RoleNotificationSettings = new RoleNotificationSetting[]
                {
                    new RoleNotificationSetting
                    {
                        Id = guid,
                        ContractProfileId = guid,
                        Enable = true,
                        RoleId = guid,
                        SubdivisionId = guid,
                        TargetNotifies = new TargetNotify[] { TargetNotify.Email, TargetNotify.Socket }
                    }
                },
                WebHooks = new WebHook[]
                {
                    new WebHook
                    {
                        Id = guid,
                        Enable = true,
                        ContractProfileId = guid,
                        Url = "testUrl",
                        WebHookContractType = WebHookContractType.Json
                    },
                    new WebHook
                    {
                        Id = guid,
                        Enable = true,
                        ContractProfileId = guid,
                        Url = "testUrl",
                        WebHookContractType = WebHookContractType.StringArray
                    }
                }
            };
        }

        public Task<IPageItems<ContractProfile>> GetContractProfiles(FilterContract filterContract)
        {
            Guid guid = Guid.NewGuid();

            ContractProfile contractProfile = GetDefaultProfile(guid);

            return Task.FromResult<IPageItems<ContractProfile>>(new PageItems<ContractProfile>(new ContractProfile[] { contractProfile }, 3));
        }

        public Task<ContractProfile> GetContractProfile(Guid contractProfilesId)
        {
            ContractProfile contractProfile = GetDefaultProfile(contractProfilesId);

            return Task.FromResult(contractProfile);
        }

        public Task<IEnumerable<ContractProfile>> GetUserContractProfiles(Guid userId)
        {
            Assert.IsTrue(userId != Guid.Empty);

            Guid guid = Guid.NewGuid();

            ContractProfile contractProfile = GetDefaultProfile(guid);

            return Task.FromResult<IEnumerable<ContractProfile>>(new ContractProfile[] { contractProfile });
        }
    }
}
