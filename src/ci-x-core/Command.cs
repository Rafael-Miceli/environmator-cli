using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ci_x_core
{
    public abstract class Command
    {
        public Command()
        {
            envyxDir = Path.Combine(AppDataPath, "ci-x");
            Directory.CreateDirectory(envyxDir);

            envyxConfigFile = Path.Combine(envyxDir, "config.");
        }

        private string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string envyxDir = string.Empty;
        private string envyxConfigFile = string.Empty;

        public abstract string Verb { get; }
        public abstract Option[] Options { get; }
        public abstract string Help { get; }

        public abstract void CreateEnvironment();

        public async Task WriteConfig(Dictionary<Option, string> optionsAndValues)
        {            
            var pluginSection = new List<string> { $"[{Verb}]" };

            foreach (var option in optionsAndValues)
            {
                pluginSection.Add($"{option.Key.Name}={option.Value}");
            }

            if (!File.Exists(envyxConfigFile) || ConfigDoesNotExist(envyxConfigFile))
            {
                File.AppendAllLines(envyxConfigFile, pluginSection);
                return;
            }
            
            var lines = File.ReadAllLines(envyxConfigFile).ToList();

            var indexToBeginSubstitute = lines.IndexOf($"[{Verb}]") + 1;
            var indexEndToSubstitute = (lines.Skip(indexToBeginSubstitute).TakeWhile(line => !line.Contains("[")).Count() + indexToBeginSubstitute);

            for (int i = 1; i <= (pluginSection.Count - 1); i++)
            {
                if (indexToBeginSubstitute < indexEndToSubstitute)
                {
                    lines[indexToBeginSubstitute] = pluginSection[i];
                    indexToBeginSubstitute++;
                    continue;
                }

                lines.Insert(indexToBeginSubstitute, pluginSection[i]);
                indexToBeginSubstitute++;                    
            }
                
            ClearFileContent(envyxConfigFile);
                
            File.AppendAllLines(envyxConfigFile, lines);
            
        }

        //public async Task<ConfigVerb> ReadPluginConfig()
        //{
        //    var pluginConfigAsString = File.ReadAllLines(envyxConfigFile);

        //    var pluginConfigAsStringArray = pluginConfigAsString
        //     .Where(l => !string.IsNullOrEmpty(l))
        //     .SkipWhile(line => !line.Contains($"[{_pluginTag}]"))
        //     .Skip(1)
        //     .TakeWhile(line => !line.Contains("["));

        //    var pluginConfigSplited = pluginConfigAsStringArray.Select(c => c.Split('='));

        //    var config = await ReadDefinedConfigSections(pluginConfigSplited);

        //    return config;
        //}

        private bool ConfigDoesNotExist(string envyxConfigFile)
        {
            var lines = File.ReadAllLines(envyxConfigFile).ToList();
            return lines.IndexOf($"[{Verb}]") == -1;
        }

        public void ClearFileContent(string envyxConfigFile)
        {
            File.WriteAllText(envyxConfigFile, string.Empty);
        }
    }
}
