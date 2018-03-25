using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CommandLine;
using environmator_cli.Configuration;
using environmator_cli.Services;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;


namespace environmator_cli
{
    public class Program
    {
        private static IConfigRepository _configRepository;
        private static IVstsService _vstsService;

        static int Main(string[] args)
        {
            _configRepository = new ConfigRepository();
            _vstsService = new VstsService(_configRepository);

            return Parser.Default.ParseVerbs<ConfigVerb, NewVerb>(args)
                 .MapResult(
                 (ConfigVerb opts) => RunConfigAndReturnExitCode(opts),
                 (NewVerb opts) => RunNewAndReturnExitCode(opts),
                 (NewVerb.ProjectVerb opts) => RunNewProjectAndReturnExitCode(opts),
                 (ConfigVerb.ConfigVstsVerb opts) => RunConfigVstsAndReturnExitCode(opts),
                 errs => 1);            
        }

        private static int RunConfigAndReturnExitCode(object opts)
        {
            return 0;
        }

        private static int RunNewAndReturnExitCode(object opts)
        {
            return 0;
        }

        private static int RunNewProjectAndReturnExitCode(NewVerb.ProjectVerb opts)
        {
            Console.WriteLine($"Creating {opts.Name} repository in vsts.");            
            
            try
            {
                _vstsService.CreateRepository(opts.Name).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sorry, but we had a problem trying to create {opts.Name} repository");
                Console.WriteLine(ex.Message);
                return 1;
            }            

            Console.WriteLine($"{opts.Name} repository created! Ready to work! =D");
            return 0;
        }
        
        private static int RunConfigVstsAndReturnExitCode(ConfigVerb.ConfigVstsVerb opts)
        {   
            _configRepository.SetVstsConfig(opts);
            
            return 0;
        }        

    }
}
