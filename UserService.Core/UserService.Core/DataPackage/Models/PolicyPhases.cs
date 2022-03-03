
using Nest;

namespace UserService.Core.Models
{
    public record PolicyPhases(string Id, HotPolicyPhase HotPolicyPhase, WarmPolicyPhase WarmPolicyPhase, DeletePolicyPhase DeletePolicyPhase)
    {
        public static implicit operator Phases(PolicyPhases policyPhases)
        {
            return new()
            {
                Hot = (Phase)policyPhases.HotPolicyPhase,
                Warm = (Phase)policyPhases.WarmPolicyPhase,
                Delete = (Phase)policyPhases.DeletePolicyPhase,
            };
        }
    }
}
