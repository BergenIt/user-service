using System;

using Grpc.Core;

namespace UserService.Core.JwtGenerator
{
    public record TokenMeta(string UserName, Guid UserId, string UserKey, DateTime ValidTo, bool HasJti)
    {
        public void AddToMeta(Metadata metadata)
        {
            metadata.Add(JwtGenerator.UserKey, UserKey);
            metadata.Add(JwtGenerator.UserName, UserName);
            metadata.Add(JwtGenerator.UserId, UserId.ToString());
            metadata.Add(JwtGenerator.ValidTo, ValidTo.ToString());
        }
    }
}
