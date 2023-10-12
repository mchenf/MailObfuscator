using System.Diagnostics;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;
using Foss.PowerShell.MailObfu.Core;

namespace MailObfu
{
    [CmdletBinding(DefaultParameterSetName = "ByContent")]
    [Cmdlet(VerbsCommon.Hide, "MailContent")]
    public class HideMailContent : Cmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ParameterSetName = "ByContent", ValueFromPipeline = true)]
        [Alias("c")]
        public string? Content { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "CheckVersion")]
        [Alias("v")]
        public SwitchParameter Version { get; set; } = false;

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
            var mailDes = new MailDesensitizer();
            WriteObject(mailDes.ObfuscateMail(Content));
        }
    }
}