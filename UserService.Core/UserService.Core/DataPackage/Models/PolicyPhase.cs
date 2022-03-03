using System;

using Nest;

namespace UserService.Core.Models
{
    public abstract record PolicyPhase(TimeSpan MinimumAge)
    {
        public static explicit operator Phase(PolicyPhase policy)
        {
            return new()
            {
                MinimumAge = policy.MinimumAge,
            };
        }
    }
}
