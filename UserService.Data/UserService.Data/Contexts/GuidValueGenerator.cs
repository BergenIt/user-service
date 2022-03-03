using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace UserService.Data
{
    public class GuidValueGenerator : ValueGenerator
    {
        public override bool GeneratesTemporaryValues { get; } = false;  

        protected override object NextValue([NotNullAttribute] EntityEntry entry)
        {
            return Guid.NewGuid();
        }
    }
}
