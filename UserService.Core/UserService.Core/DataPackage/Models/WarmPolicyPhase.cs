using System;

using Nest;

namespace UserService.Core.Models
{
    public record WarmPolicyPhase : PolicyPhase
    {
        public WarmPolicyPhase(TimeSpan MinimumAge) : base(MinimumAge) { }

        public static explicit operator Phase(WarmPolicyPhase policy)
        {
            Phase phase = (Phase)(policy as PolicyPhase);

            phase.Actions = new LifecycleActions();

            return phase;
        }
    }
}
