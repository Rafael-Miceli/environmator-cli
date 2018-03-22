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
                 (NewVerb.ProjectVerb opts) => RunNewProjectAndReturnExitCode(opts),
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

        private static int RunNewProjectAndReturnExitCode(NewVerb.ProjectVerb opts)
        {
            Console.WriteLine("Criando projeto com nome " + opts.Name);

            var vstsConfig = _configRepository.ReadVstsConfig();            

            var repos = GetVstsRepos(opts, vstsConfig);

            //string[] vstsConfig = _configRepository.ReadVstsConfig(opts);

            return 0;
        }

        private static IEnumerable<string> GetVstsRepos(NewVerb.ProjectVerb opts, ConfigVerb.ConfigVstsVerb vstsConfig)
        {
            // Interactively ask the user for credentials, caching them so the user isn't constantly prompted
            VssCredentials creds = new VssBasicCredential(string.Empty, "token");
            //creds.Storage = new VssClientCredentialStorage();

            var vstsCollectionUrl = $"https://{vstsConfig.Instance}.visualstudio.com";

            VssConnection connection = new VssConnection(new Uri(vstsCollectionUrl), creds);

            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();

            var repo = gitClient.GetRepositoriesAsync().Result;

            //HttpClient client = new HttpClient();
            //var result = client.GetAsync($"{vstsCollectionUrl}/DefaultCollection/{vstsConfig.Project}/_apis/git/repositories?api-version=1.0").Result;

            Console.WriteLine(repo.Count);
            Console.WriteLine(repo.FirstOrDefault()?.Name);
            return null;
        }

        private static int RunConfigVstsAndReturnExitCode(ConfigVerb.ConfigVstsVerb opts)
        {
            Console.WriteLine("config vsts foi chamado");           
            
            _configRepository.SetVstsConfig(opts);
            
            return 0;
        }        

    }
}
