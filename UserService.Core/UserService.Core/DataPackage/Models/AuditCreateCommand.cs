using System;
using System.Collections.Generic;

namespace UserService.Core.Models
{
    public record AuditCreateCommand(string UserName, string Message, string Action);

    public class UserAuditRecord
    {
        public string FullName { get; set; }
        public string Subdivision { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> ConnectionType { get; set; }
        public DateTimeOffset RegistredDate { get; set; }
        public IEnumerable<string> Ips { get; set; }
        public DateTimeOffset? LastLogin { get; set; }
        public TimeSpan ScreenTime { get; set; }
        public long CountLogin { get; set; }
    }
}
