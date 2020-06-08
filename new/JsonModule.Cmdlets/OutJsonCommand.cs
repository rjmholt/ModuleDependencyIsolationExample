using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using JsonModule.Engine;

namespace JsonModule.Cmdlets
{
    [Cmdlet(VerbsData.Out, "Json")]
    public class OutJsonCommand : PSCmdlet
    {
        private List<object> _values;

        public OutJsonCommand()
        {
            _values = new List<object>();
        }

        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string OutFile { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public object[] Value { get; set; }

        protected override void ProcessRecord()
        {
            _values.AddRange(Value);
        }

        protected override void EndProcessing()
        {
            if (_values.Count == 0)
            {
                return;
            }

            using (FileStream fileStream = new FileStream(GetUnresolvedProviderPathFromPSPath(OutFile), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                new JsonWriter().WriteToStream(fileStream, _values.Count > 1 ? _values : _values[0]);
            }
        }
    }
}