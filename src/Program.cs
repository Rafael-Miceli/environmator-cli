using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CommandLine;
using environmator_cli.Configuration;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;


namespace environmator_cli
{
    public class Program
    {
        private static ConfigRepository _configRepository;
        public static ConfigVerb.ConfigVstsVerb _vstsConfig;
        private static GitHttpClient _vstsGitClient;

        static int Main(string[] args)
        {
            _configRepository = new ConfigRepository();

            _vstsConfig = _configRepository.ReadVstsConfig();

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

            var vstsConfig = _configRepository.ReadVstsConfig();

            if (RepositoryExists(opts.Name))
                return 1;
            try
            {
                _vstsGitClient.CreateRepositoryAsync(new GitRepository() { Name = opts.Name }, _vstsConfig.Project).Wait();
            }
            catch (Exception)
            {
                //Validate permission exception
                Console.WriteLine($"Sorry, but we had a problem trying to create {opts.Name} repository");
                return 1;
            }
            

            Console.WriteLine($"{opts.Name} repository created! Ready to work! =D");
            return 0;
        }

        public static bool RepositoryExists(string repositoryName)
        {            
            VssCredentials creds = new VssBasicCredential(string.Empty, _vstsConfig.Token);
            //creds.Storage = new VssClientCredentialStorage();

            var vstsCollectionUrl = $"https://{_vstsConfig.Instance}.visualstudio.com";

            VssConnection connection = new VssConnection(new Uri(vstsCollectionUrl), creds);

            _vstsGitClient = connection.GetClient<GitHttpClient>();

            GitRepository repo;

            try
            {
                repo = _vstsGitClient.GetRepositoryAsync(_vstsConfig.Project, repositoryName).Result;
            }
            catch (Exception ex)
            {
                //Validate permission exception
                return false;
            }            

            Console.WriteLine($"Repository {repositoryName} already exists.");

            return repo != null;
        }

        private static int RunConfigVstsAndReturnExitCode(ConfigVerb.ConfigVstsVerb opts)
        {   
            _configRepository.SetVstsConfig(opts);
            
            return 0;
        }        

    }
}
