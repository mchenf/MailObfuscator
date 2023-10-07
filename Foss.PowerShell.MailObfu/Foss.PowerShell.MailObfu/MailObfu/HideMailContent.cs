using System.Diagnostics;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;

namespace MailObfu
{
    [CmdletBinding(DefaultParameterSetName = "ByContent")]
    [Cmdlet(VerbsCommon.Hide, "MailContent")]
    public class HideMailContent : Cmdlet
    {
        private const char obf = (char)0x2588;
        private const string emailPattern = @"(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))";
        private const string phonePattern = @"([0-9]{2,}\s)*[0-9]{2,}";

        [Parameter(Mandatory = false, Position = 0, ParameterSetName = "ByContent", ValueFromPipeline = true)]
        [Alias("c")]
        public string? Content { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "CheckVersion")]
        [Alias("v")]
        public SwitchParameter Version { get; set; } = false;

        protected override void BeginProcessing()
        {
         
        }

        protected override void ProcessRecord()
        {
            if (Version)
            {
                Version v = new Version(1, 0, 0, 4);
                WriteObject(v);
                return;
            }
            if (Content == null)
            {
                return;
            }
            WriteObject(ObfuscateMail(Content));
        }

        private string ObfuscateMail(string input)
        {
            Regex r1 = new Regex(emailPattern);
            Regex r2 = new Regex(phonePattern);

            return input
                .Desensitize(r1, ObfuscateMailEvaluator)
                .Desensitize(r2, ObfuscateMailEvaluator);
        }

        private string ObfuscateMailEvaluator(Match match)
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