using System;
using System.Collections.Generic;

namespace UserService.Core.Models
{
    public class SubdivisionAuditRecord
    {
        public string Subdivision { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Position { get; set; }
        public IEnumerable<string> ConnectionType { get; set; }
        public IEnumerable<string> Ips { get; set; }
        public TimeSpan ScreenTime { get; set; }
        public long CountLogin { get; set; }
    }
}
