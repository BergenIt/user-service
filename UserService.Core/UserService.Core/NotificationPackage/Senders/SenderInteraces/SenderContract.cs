using System.Collections.Generic;

namespace UserService.Core.SenderInteraces
{
    public record SenderContract(IEnumerable<string> Msgs, IEnumerable<string> Receivers, string Subject)
    {
        public SenderContract(string msg, string receiver, string subject = null) :
            this(new string[] { msg }, new string[] { receiver }, subject ?? string.Empty)
        { }

        public SenderContract(string msg, IEnumerable<string> receivers, string subject = null) :
            this(new string[] { msg }, receivers, subject ?? string.Empty)
        { }

        public SenderContract(IEnumerable<string> msg, string receiver, string subject = null) :
            this(msg, new string[] { receiver }, subject ?? string.Empty)
        { }
    }
}
