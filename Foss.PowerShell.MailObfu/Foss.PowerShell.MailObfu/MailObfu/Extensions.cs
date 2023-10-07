using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MailObfu
{
    public static class Extensions
    {
        public static string Desensitize (this string input, Regex regx, MatchEvaluator evl)
        {
            return regx.Replace(input, evl);
        }
    }
}
