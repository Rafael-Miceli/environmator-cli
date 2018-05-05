using ci_x_core;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vsts_plugin
{
    public class Vsts : Command
    {        
        public override string Verb => "vsts";

        private Option Instance;
        private Option Project;
        private Option Token;

        public override Option[] Options => new Option[] {
            new Option("instance", "i", "instance", "Your vsts instance."),
            new Option("project", "p", "projec", "Your vsts project."),
            new Option("token", "t", "token", "Your vsts personal token, to see how to generate a personal token follow this link: https://docs.microsoft.com/en-us/vsts/accounts/use-personal-access-tokens-to-authenticate?view=vsts")
        };
        
        public override string Help => "Set the options to use your VSTS environment";
        
        private GitHttpClient _vstsGitClient;
        private VssConnection _connection;

        public override void CreateEnvironment(string projectName, Option[] optionsWithValues)
        {
            Instance = optionsWithValues.FirstOrDefault(o => o.Name == "instance");
            Project = optionsWithValues.FirstOrDefault(o => o.Name == "project");
            Token = optionsWithValues.FirstOrDefault(o => o.Name == "token");

            Terminal.Output(projectName);

            ConnectToVsts().Wait();

            CreateRepository(projectName).Wait();
        }

        public async Task CreateRepository(string repositoryName)
        {
            _vstsGitClient = _connection.GetClient<GitHttpClient>();

            await _vstsGitClient.CreateRepositoryAsync(new GitRepository() { Name = repositoryName }, Project.Value);
        }

        private async Task ConnectToVsts()
        {
            VssCredentials creds = new VssBasicCredential(string.Empty, Token.Value);
            //creds.Storage = new VssClientCredentialStorage();

            var vstsCollectionUrl = $"https://{Instance.Value}.visualstudio.com";

            _connection = new VssConnection(new Uri(vstsCollectionUrl), creds);
        }
    }
}
