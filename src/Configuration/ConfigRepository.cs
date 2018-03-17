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

            var newVstsSection = new string[] {"[vsts-default]", $"instance={opts.Instance}", $"project={opts.Project}"};
            
            if (!File.Exists(envyxConfigFile))
            {                
                File.AppendAllLines(envyxConfigFile, newVstsSection);
            }
            else
            {
                var lines = File.ReadAllLines(envyxConfigFile).ToList();

                var indexToBeginSubstitute = lines.IndexOf("[vsts-default]") + 1;
                var indexEndToSubstitute = (lines.Skip(indexToBeginSubstitute).TakeWhile(line => !line.Contains("[")).Count() + indexToBeginSubstitute);

                for (int i = 0; i <= (newVstsSection.Length - 2); i++)
                {
                    if (indexToBeginSubstitute < indexEndToSubstitute)
                    {
                        lines[indexToBeginSubstitute] = newVstsSection[i + 1];
                        indexToBeginSubstitute++;
                        continue;
                    }

                    lines.Insert(indexToBeginSubstitute, newVstsSection[i + 1]);
                    indexToBeginSubstitute++;
                }

                //var existentVstsSection = GetVstsConfigSections(lines);

                //existentVstsSection = newVstsSection.Skip(1).ToArray();

                ClearFileContent(envyxConfigFile);

                File.AppendAllLines(envyxConfigFile, lines);                
            }
        }

        private string[] GetVstsConfigSections(string[] lines)
        {
            var vstsConfigSectionTag = "[vsts-default]";
            return lines.SkipWhile(l => !l.Contains(vstsConfigSectionTag))
            .Skip(1)
            .TakeWhile(line => !line.Contains("[")).ToArray();
        }

        public void ClearFileContent(string envyxConfigFile)
        {
            File.WriteAllText(envyxConfigFile, string.Empty);
        }
    }    
}
