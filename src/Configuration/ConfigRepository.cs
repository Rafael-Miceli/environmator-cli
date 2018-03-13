using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace environmator_cli.Configuration
{
    public class ConfigRepository
    {
        private string appDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string envyxDir = string.Empty;
        private string envyxConfigFile = string.Empty;

        public ConfigRepository()
        {
            envyxDir = Path.Combine(appDataPath, "envyx");
            Directory.CreateDirectory(envyxDir);

            envyxConfigFile = Path.Combine(envyxDir, "config.");
        }
        public void SetVstsConfig(ConfigVerb.ConfigVstsVerb opts)
        {
            Console.WriteLine($"Setando valor do vsts {opts.Instance} - {opts.Project}");

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
                var lines = File.ReadLines(envyxConfigFile)
                .SkipWhile(line => !line.Contains("[vsts-default]"))
                .Skip(1)                
                .TakeWhile(line => !line.Contains("["));
                
                var vstsConfigInOneLine = string.Join(string.Empty, lines);                
            }
        }
    }
}
