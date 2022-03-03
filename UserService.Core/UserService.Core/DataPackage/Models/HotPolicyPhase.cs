using System;

using Nest;

namespace UserService.Core.Models
{
    public record HotPolicyPhase : PolicyPhase
    {
        public HotPolicyPhase(TimeSpan MaximumAge, string MaximumSize, long? MaximumDocuments) : base(new TimeSpan(0))
        {
            this.MaximumAge = MaximumAge;
            this.MaximumSize = MaximumSize;
            this.MaximumDocuments = MaximumDocuments;
        }

        public TimeSpan MaximumAge { get; init; }
        public string MaximumSize { get; init; }
        public long? MaximumDocuments { get; init; }

        public static explicit operator Phase(HotPolicyPhase policy)
        {
            Phase phase = (Phase)(policy as PolicyPhase);

            LifecycleActions hotLifecycleActions = new();

            RolloverLifecycleAction hotRolloverLifecycleAction = new()
            {
                MaximumAge = policy.MaximumAge,
                MaximumSize = policy.MaximumSize,
                MaximumDocuments = policy.MaximumDocuments,
            };

            hotLifecycleActions.Add(hotRolloverLifecycleAction);

            phase.Actions = hotLifecycleActions;

            return phase;
        }
    }
}
