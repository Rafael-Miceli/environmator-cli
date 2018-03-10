using System;
using System.Collections.Generic;
using System.Text;

namespace environmator_cli.Configuration
{
    public class ConfigRepository
    {
        internal void SetVstsConfig(ConfigVerb.ConfigVstsVerb opts)
        {
            Console.WriteLine($"Setando valor do vsts {opts.Instance} - {opts.Project}");
        }
    }
}
