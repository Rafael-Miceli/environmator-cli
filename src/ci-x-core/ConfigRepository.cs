using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ci_x_core
{
    public abstract class ConfigRepository<T>
    {
        private string appDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string envyxDir = string.Empty;
        private string envyxConfigFile = string.Empty;
        private string _pluginTag;

        //Plugin tag to read in config, for example: vsts
        public ConfigRepository(string pluginTag)
        {
            _pluginTag = pluginTag;

            envyxDir = Path.Combine(appDataPath, "ci-x");
            Directory.CreateDirectory(envyxDir);

            envyxConfigFile = Path.Combine(envyxDir, "config.");
        }

        public void SetVstsConfig()
        {
            var newPluginSection = DefinePluginSection();

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

        protected abstract string[] DefinePluginSection(T opts);

        public T ReadVstsConfig()
        {
            var pluginConfigAsString = File.ReadAllLines(envyxConfigFile);

            var pluginConfigAsStringArray = pluginConfigAsString
             .Where(l => !string.IsNullOrEmpty(l))
             .SkipWhile(line => !line.Contains($"[{_pluginTag}]"))
             .Skip(1)
             .TakeWhile(line => !line.Contains("["));

            var pluginConfigSplited = pluginConfigAsStringArray.Select(c => c.Split('='));

            var config = ReadDefinedConfigSections(pluginConfigSplited);

            //Na mão do usuário

            //foreach (var config in pluginConfigSplited)
            //{
            //    if (config[0] == "instance")
            //    {
            //        config.Instance = config[1];
            //        continue;
            //    }

            //    if (config[0] == "project")
            //    {
            //        config.Project = config[1];
            //        continue;
            //    }

            //    config.Token = config[1];
            //}

            return config;
        }

        public abstract T ReadDefinedConfigSections(IEnumerable<string[]> pluginConfigSplited);

        private bool ConfigDoesNotExist(string envyxConfigFile)
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
