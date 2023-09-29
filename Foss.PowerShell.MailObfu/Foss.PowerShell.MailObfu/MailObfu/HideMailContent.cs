using System.Management.Automation;

namespace MailObfu
{
    [Cmdlet(VerbsCommon.Hide, "MailContent")]
    public class HideMailContent : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ByContent")]
        public string? Content { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ByDirectory")]
        public string? Directory { get; set; }

        protected override void BeginProcessing()
        {
            if (Content == null)
            {
                WriteObject("ParmSet: ByDirectory is used.");
            }
            else
            {
                WriteObject("ParmSet: ByContent is used.");
            }
        }
    }
}