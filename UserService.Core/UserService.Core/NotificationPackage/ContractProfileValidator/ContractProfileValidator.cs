using System.Collections.Generic;
using System.Linq;

using Grpc.Core;

using UserService.Core.AuditPackage.AuditException;
using UserService.Core.Entity;

namespace UserService.Core.NotificationPackage.ContractProfileValidator
{
    public class ContractProfileValidator : IContractProfileValidator
    {
        public void ValidateContractProfile(IEnumerable<ContractProfile> contractProfiles)
        {
            foreach (ContractProfile notificationSetting in contractProfiles)
            {
                ValidateContractProfile(notificationSetting);
            }
        }

        public void ValidateContractProfile(ContractProfile contractProfile)
        {
            bool lineNumberNotValide = contractProfile.ContractSettingLines
                .GroupBy(c => c.LineNumber)
                .Any(g => g.Count() > 1);

            if (lineNumberNotValide)
            {
                throw new RpcException(new(StatusCode.InvalidArgument, nameof(lineNumberNotValide)));
            }

            foreach (ContractSettingLine settingLine in contractProfile.ContractSettingLines)
            {
                List<int> templatePositions = new();

                for (int i = 1; i < settingLine.UserTemplate.Length - 1; i++)
                {
                    string part = settingLine.UserTemplate[(i - 1)..(i + 2)];
                    if (part[0] != '{' || !int.TryParse($"{part[1]}", out int position) || part[2] != '}')
                    {
                        continue;
                    }

                    templatePositions.Add(position);
                }

                bool containsAllPosition = settingLine.ContractPropperties.All(p => settingLine.UserTemplate.Contains($"{{{p.Position}}}"))
                    && templatePositions.All(p => settingLine.ContractPropperties.Any(c => c.Position == p));

                if (!containsAllPosition)
                {
                    throw new ValidateNotificationSettingException();
                }
            }
        }
    }
}
