using System.Text.RegularExpressions;
using System.Text;

namespace Foss.PowerShell.MailObfu.Core
{
    public class MailDesensitizer
    {
        private static readonly char obf = (char)0x2588;
        private static readonly string emailPattern = @"(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))";
        private string emailEvaluator(Match match)
        {
            string m = match.Value;
            StringBuilder sb = new StringBuilder();
            int len = m.Length;
            for (int i = 0; i < len; i++)
            {
                if (i == 0 || i == len - 1 || m[i] == '@')
                {
                    sb.Append(m[i]);
                }
                else
                {
                    sb.Append(obf);
                }

            }
            return sb.ToString();
        }

        public string ObfuscateMail(string input)
        {
            Regex r1 = new Regex(emailPattern);

            return r1.Replace(input, emailEvaluator);
        }

    }
}