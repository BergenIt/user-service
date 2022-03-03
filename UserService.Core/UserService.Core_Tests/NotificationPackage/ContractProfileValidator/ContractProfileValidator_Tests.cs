using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.Entity;

namespace UserService.Core.NotificationPackage.ContractProfileValidator.Tests
{
    [TestClass]
    public class ContractProfileValidator_Tests
    {
        private readonly ContractProfileValidator _contractProfileValidator;

        public ContractProfileValidator_Tests()
        {
            _contractProfileValidator = new();
        }


        [TestMethod]
        public void ContractProfileValidator_ValidateContractProfile_Test()
        {
            Guid guid = Guid.NewGuid();

            ContractProfile contractProfile = GetContractProfile(guid);

            _contractProfileValidator.ValidateContractProfile(contractProfile);
        }

        [TestMethod]
        public void ContractProfileValidator_ValidateContractProfile_Array_Test()
        {
            Guid guid = Guid.NewGuid();

            ContractProfile contractProfile = GetContractProfile(guid);

            _contractProfileValidator.ValidateContractProfile(new ContractProfile[] { contractProfile });
        }

        private static ContractProfile GetContractProfile(Guid guid)
        {
            return new()
            {
                Id = guid,
                Comment = "test",
                Name = "test",
                NotifyEventType = "test",
                ContractSettingLines = new ContractSettingLine[]
                {
                    new ContractSettingLine()
                    {
                        LineNumber = 0,
                        Id = guid,
                        ContractProfileId = guid,
                        Enable = true,
                        UserProppertyName = "Test",
                        UserTemplate = "{1} test {0}",
                        ContractPropperties = new ContractSettingPropperty[]
                        {
                            new ContractSettingPropperty()
                            {
                                Id = guid,
                                ContractSettingLineId = guid,
                                Position = 0,
                                ContractName = "test_1"
                            },
                            new ContractSettingPropperty()
                            {
                                Id = guid,
                                ContractSettingLineId = guid,
                                Position = 1,
                                ContractName = "test_2"
                            }
                        }
                    },
                }
            };
        }

    }
}
