using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ci_x_core
{
    public abstract class EnvironmentPluginService<T>
    {
        public abstract Task CreateEnvironment(string projectName, string description = null);
        public abstract Task<string[]> DefinePluginSection(T opts);
        public abstract Task<T> ReadDefinedConfigSections(IEnumerable<string[]> pluginConfigSplited);


        private string appDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string envyxDir = string.Empty;
        private string envyxConfigFile = string.Empty;
        private string _pluginTag;

        public EnvironmentPluginService(string pluginTag)
        {
            _pluginTag = pluginTag;

            envyxDir = Path.Combine(appDataPath, "ci-x");
            Directory.CreateDirectory(envyxDir);

            envyxConfigFile = Path.Combine(envyxDir, "config.");
        }

        public async Task SetPluginConfig(T configSection)
        {
            var newPluginSection = await DefinePluginSection(configSection);

            if (!File.Exists(envyxConfigFile) || ConfigDoesNotExist(envyxConfigFile))
            {
                File.AppendAllLines(envyxConfigFile, newPluginSection);
            }
            else
            {
                var lines = File.ReadAllLines(envyxConfigFile).ToList();

                var indexToBeginSubstitute = lines.IndexOf($"[{_pluginTag}]") + 1;
                var indexEndToSubstitute = (lines.Skip(indexToBeginSubstitute).TakeWhile(line => !line.Contains("[")).Count() + indexToBeginSubstitute);

                for (int i = 1; i <= (newPluginSection.Length - 1); i++)
                {
                    if (indexToBeginSubstitute < indexEndToSubstitute)
                    {
                        lines[indexToBeginSubstitute] = newPluginSection[i];
                        indexToBeginSubstitute++;
                        continue;
                    }

                    lines.Insert(indexToBeginSubstitute, newPluginSection[i]);
                    indexToBeginSubstitute++;
                }

                ClearFileContent(envyxConfigFile);

                File.AppendAllLines(envyxConfigFile, lines);
            }
        }

        public async Task<T> ReadPluginConfig()
        {
            var pluginConfigAsString = File.ReadAllLines(envyxConfigFile);

            var pluginConfigAsStringArray = pluginConfigAsString
             .Where(l => !string.IsNullOrEmpty(l))
             .SkipWhile(line => !line.Contains($"[{_pluginTag}]"))
             .Skip(1)
             .TakeWhile(line => !line.Contains("["));

            var pluginConfigSplited = pluginConfigAsStringArray.Select(c => c.Split('='));

            var config = await ReadDefinedConfigSections(pluginConfigSplited);

            return config;
        }

        private bool ConfigDoesNotExist(string envyxConfigFile)
        {
            var lines = File.ReadAllLines(envyxConfigFile).ToList();
            return lines.IndexOf($"[{_pluginTag}]") == -1;
        }

        public void ClearFileContent(string envyxConfigFile)
        {
            File.WriteAllText(envyxConfigFile, string.Empty);
        }
    }
}
