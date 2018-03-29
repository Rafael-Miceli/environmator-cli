using ci_x_core;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Threading.Tasks;

namespace vsts_plugin
{
    public class VstsService : EnvironmentPluginService
    {
        private ConfigVstsVerb _vstsConfig;
        private VssConnection _connection;
        private GitHttpClient _vstsGitClient;

        public VstsService()
        {
            _vstsConfig = ReadEnvironmentConfig<ConfigVstsVerb>().Result;
            ConnectToVsts();
        }

        public override async Task CreateEnvironment(string projectName, string description = null)
        {
            await CreateEnvironment(projectName);
        }

        public override async Task<ConfigVstsVerb> ReadEnvironmentConfig<ConfigVstsVerb>()
        {
            var vstsConfigAsString = File.ReadAllLines(envyxConfigFile);

            var vstsConfigAsStringArray = vstsConfigAsString
             .Where(l => !string.IsNullOrEmpty(l))
             .SkipWhile(line => !line.Contains("[vsts]"))
             .Skip(1)
             .TakeWhile(line => !line.Contains("["));

            var vstsConfigSplited = vstsConfigAsStringArray.Select(c => c.Split('='));

            var vstsConfig = new ConfigVerb.ConfigVstsVerb();

            foreach (var config in vstsConfigSplited)
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

        public override async Task SetEnvironmentConfig<ConfigVstsVerb>(ConfigVstsVerb opts)
        {
            throw new NotImplementedException();
        }

        public async Task CreateRepository(string repositoryName)
        {
            _vstsGitClient = _connection.GetClient<GitHttpClient>();

            await _vstsGitClient.CreateRepositoryAsync(new GitRepository() { Name = repositoryName }, _vstsConfig.Project);
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
