using System.Diagnostics;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;

namespace MailObfu
{

    [CmdletBinding]
    [Cmdlet(VerbsCommon.Hide, "MailContent")]
    public class HideMailContent : Cmdlet
    {
        private const char obf = (char)0x2588;
        private const string emailPattern = @"(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))";

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ByContent")]
        public string? Content { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ByDirectory")]
        public string? Directory { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "CheckVersion")]
        [Alias("v")]
        public SwitchParameter Version { get; set; } = false;

        protected override void BeginProcessing()
        {
            if (Version)
            {
                Version v = new Version(1, 0, 0, 4);
                WriteObject(v);
                return;
            }
            if (Content == null)
            {
                throw new NotImplementedException("Not yet implemented");
            }
            else
            {
                Regex r = new Regex(emailPattern);
                MatchCollection mc = r.Matches(Content);
                WriteVerbose(string.Format("{0} matches found.", mc.Count));
                foreach (Match m in mc)
                {
                    WriteVerbose(m.Value);
                }

                WriteObject(r.Replace(Content, ObfuscateMail));

            }
        }

        private string ObfuscateMail(Match match)
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
    }
}