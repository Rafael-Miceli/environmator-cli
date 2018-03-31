using ci_x_core;
using System;
using System.Collections.Generic;
using System.Text;

namespace vsts_plugin
{
    public class VstsConfig : ConfigRepository<ConfigVstsVerb>
    {
        public VstsConfig(): base("vsts")
        {

        }

        public override ConfigVstsVerb ReadDefinedConfigSections(IEnumerable<string[]> pluginConfigSplited)
        {
            throw new NotImplementedException();
        }

        protected override string[] DefinePluginSection()
        {
            throw new NotImplementedException();
        }
    }
}
