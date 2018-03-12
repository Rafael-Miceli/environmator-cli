using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace environmator_cli.Configuration
{
    public class ConfigRepository
    {
        internal void SetVstsConfig(ConfigVerb.ConfigVstsVerb opts)
        {
            Console.WriteLine($"Setando valor do vsts {opts.Instance} - {opts.Project}");

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var envyxConfigFile = Path.Combine(appData, "envyx", "config");

            Console.WriteLine("write config vsts in file " + envyxConfigFile);
            
            if (!File.Exists(envyxConfigFile))
            {
                string vstsSection = $@"[vsts-default]
instance={opts.Instance}
project={opts.Project}";
                File.AppendAllText(envyxConfigFile, vstsSection);
            }
            else
            {
                File.ReadLines(envyxConfigFile)
                .SkipWhile(line => !line.Contains("CustomerEN"))
                .Skip(1) // optional
                .TakeWhile(line => !line.Contains("CustomerCh"));
            }
        }
    }
}
