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

            string vstsSection = $@"[vsts-default]
instance={opts.Instance}
project={opts.Project}";
            
            if (!File.Exists(envyxConfigFile))
            {                
                File.AppendAllText(envyxConfigFile, vstsSection);
            }
            else
            {
                var lines = File.ReadAllLines(envyxConfigFile);
                
                // .SkipWhile(line => !line.Contains("[vsts-default]"))
                // .Skip(1)                
                // .TakeWhile(line => !line.Contains("["));

                ClearFileContent(envyxConfigFile);

                File.AppendAllText(envyxConfigFile, vstsSection);

                // using(TextWriter tw = new StreamWriter(envyxConfigFile))
                // {
                //     foreach (String s in vstsSection)
                //         tw.WriteLine(s);
                // }
            }
        }

        public void ClearFileContent(string envyxConfigFile)
        {
            File.WriteAllText(envyxConfigFile, string.Empty);
        }
    }
}
