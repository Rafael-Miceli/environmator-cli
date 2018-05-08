using ci_x_core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vsts_plugin
{
    public class Jenkins : Command
    {        
        public override string Verb => "jenkins";

        private Option Host;

        public override Option[] Options => new Option[] {
            new Option("host", "h", "host", "Your jenkins host."),
        };
        
        public override string Help => "Set the options to use your Jenkins Host";
        
        public override void CreateEnvironment(string projectName, Option[] optionsWithValues)
        {
            Host = optionsWithValues.FirstOrDefault(o => o.Name == "host");

            Terminal.Output(Host.Name);
        }
    }
}
