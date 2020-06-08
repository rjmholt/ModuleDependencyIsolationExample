using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using Microsoft.PowerShell.Commands;
using Newtonsoft.Json;

namespace JsonModule
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
            using (var writer = new StreamWriter(fileStream))
            using (var jsonWriter = new JsonTextWriter(writer){ Formatting = Formatting.Indented })
            {
                var jsonSerializer = new JsonSerializer();

                if (_values.Count == 1)
                {
                    jsonSerializer.Serialize(jsonWriter, _values[0]);
                }
                else
                {
                    jsonSerializer.Serialize(jsonWriter, _values);
                }
            }
        }
    }
}