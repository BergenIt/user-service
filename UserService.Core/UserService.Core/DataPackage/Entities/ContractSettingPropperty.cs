using System;

namespace UserService.Core.Entity
{
    public class ContractSettingPropperty : BaseEntity
    {
        public string ContractName { get; set; }
        public byte Position { get; set; }

        public Guid ContractSettingLineId { get; set; }
        public ContractSettingLine ContractSettingLine { get; set; }
    }
}

