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

            var newVstsSection = new string[] {"[vsts]", $"instance={opts.Instance}", $"project={opts.Project}"};
            
            if (!File.Exists(envyxConfigFile) || VstsConfigDoesNotExist(envyxConfigFile))
            {                
                File.AppendAllLines(envyxConfigFile, newVstsSection);
            }
            else
            {
                var lines = File.ReadAllLines(envyxConfigFile).ToList();

                var indexToBeginSubstitute = lines.IndexOf("[vsts]") + 1;
                var indexEndToSubstitute = (lines.Skip(indexToBeginSubstitute).TakeWhile(line => !line.Contains("[")).Count() + indexToBeginSubstitute);

                for (int i = 1; i <= (newVstsSection.Length - 1); i++)
                {
                    if (indexToBeginSubstitute < indexEndToSubstitute)
                    {
                        lines[indexToBeginSubstitute] = newVstsSection[i];
                        indexToBeginSubstitute++;
                        continue;
                    }

                    lines.Insert(indexToBeginSubstitute, newVstsSection[i]);
                    indexToBeginSubstitute++;
                }

                ClearFileContent(envyxConfigFile);

                File.AppendAllLines(envyxConfigFile, lines);                
            }

            // .SkipWhile(line => !line.Contains("[vsts-default]"))
            // .Skip(1)                
            // .TakeWhile(line => !line.Contains("["));
        }

        private bool VstsConfigDoesNotExist(string envyxConfigFile)
        {
            var lines = File.ReadAllLines(envyxConfigFile).ToList();
            return lines.IndexOf("[vsts]") == -1;
        }

        public void ClearFileContent(string envyxConfigFile)
        {
            File.WriteAllText(envyxConfigFile, string.Empty);
        }
    }    
}
