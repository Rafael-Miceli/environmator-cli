using ci_x_core;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vsts_plugin
{
    public class VstsService : EnvironmentPluginService
    {
        private ConfigVstsVerb _vstsConfig;
        private VssConnection _connection;
        private GitHttpClient _vstsGitClient;

        public VstsService(): base("vsts")
        {
            _vstsConfig = (ConfigVstsVerb)ReadPluginConfig().Result;
            ConnectToVsts();
        }

        public override async Task CreateEnvironment(string projectName, string description = null)
        {
            await CreateRepository(projectName);
        }        

        public async Task CreateRepository(string repositoryName)
        {
            _vstsGitClient = _connection.GetClient<GitHttpClient>();

            await _vstsGitClient.CreateRepositoryAsync(new GitRepository() { Name = repositoryName }, _vstsConfig.Project);
        }

        public override async Task<string[]> DefinePluginSection(ConfigVstsVerb opts)
        {
            return new string[] { "[vsts]", $"instance={opts.Instance}", $"project={opts.Project}", $"token={opts.Token}" };
        }

        public override async Task<ConfigVstsVerb> ReadDefinedConfigSections(IEnumerable<string[]> pluginConfigSplited)
        {
            var vstsConfig = new ConfigVstsVerb();

            foreach (var config in pluginConfigSplited)
            {
                if (config[0] == "instance")
                {
                    vstsConfig.Instance = config[1];
                    continue;
                }

                if (config[0] == "project")
                {
                    vstsConfig.Project = config[1];
                    continue;
                }

                vstsConfig.Token = config[1];
            }

            return vstsConfig;
        }

        private async Task ConnectToVsts()
        {
            VssCredentials creds = new VssBasicCredential(string.Empty, _vstsConfig.Token);
            //creds.Storage = new VssClientCredentialStorage();

            var vstsCollectionUrl = $"https://{_vstsConfig.Instance}.visualstudio.com";

            _connection = new VssConnection(new Uri(vstsCollectionUrl), creds);
        }
    }
}
