using System;

namespace UserService.Core.Models
{
    public class SystemAuditRecord
    {
        public string Subdivision { get; set; }
        public int CountUsers { get; set; }
        public long CountLogin { get; set; }
        public TimeSpan ScreenTime { get; set; }
    }
}
