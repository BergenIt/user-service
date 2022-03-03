using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserService.Core.PasswordGenerator
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private static readonly char[] s_symbol = { '=', ':', '(', ')', '-', '!', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public string GeneratePassword()
        {
            Random random = new(DateTime.UtcNow.Millisecond);

            IEnumerable<string> guidSplit = Guid.NewGuid().ToString().Split("-");

            return string.Concat(PasswordParser(random, guidSplit));
        }

        private static IEnumerable<string> PasswordParser(Random random, IEnumerable<string> guidSplit)
        {
            string selector(string s) => Convert.ToBase64String(Encoding.UTF8.GetBytes(s))[..6] + s_symbol[random.Next(0, s_symbol.Length)];

            return guidSplit.Select(selector);
        }
    }
}
