using System;
using System.IO;
using CommandLine;
using environmator_cli.Configuration;


namespace environmator_cli
{
    class Program
    {
        private static ConfigRepository _configRepository;

        static int Main(string[] args)
        {
            _configRepository = new ConfigRepository();

            return Parser.Default.ParseVerbs<ConfigVerb, NewVerb>(args)
                 .MapResult(
                 (ConfigVerb opts) => RunConfigAndReturnExitCode(opts),
                 (NewVerb opts) => RunNewAndReturnExitCode(opts),
                 (ConfigVerb.ConfigVstsVerb opts) => RunConfigVstsAndReturnExitCode(opts),
                 errs => 1);            
        }

        private static int RunConfigAndReturnExitCode(object opts)
        {
            Console.WriteLine("config foi chamado");
            return 0;
        }

        private static int RunNewAndReturnExitCode(object opts)
        {
            Console.WriteLine("new foi chamado");
            return 0;
        }

        private static int RunConfigVstsAndReturnExitCode(ConfigVerb.ConfigVstsVerb opts)
        {
            Console.WriteLine("config vsts foi chamado");

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var envyxConfigFile = Path.Combine(appData, "envyx", "config.json");

            Console.WriteLine("write config vsts in file " + appData);
            
            _configRepository.SetVstsConfig(opts);
            
            return 0;
        }        

    }
}
