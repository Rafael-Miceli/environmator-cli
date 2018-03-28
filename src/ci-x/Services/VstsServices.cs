using environmator_cli.Configuration;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace environmator_cli.Services
{
    public class VstsService : IVstsService
    {
        private readonly IConfigRepository _configRepository;
        private ConfigVerb.ConfigVstsVerb _vstsConfig;
        private VssConnection _connection;
        private GitHttpClient _vstsGitClient;

        public VstsService(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
            _vstsConfig = _configRepository.ReadVstsConfig();
            ConnectToVsts();
        }
        
        public async Task CreateRepository(string repositoryName)
        {
            _vstsGitClient = _connection.GetClient<GitHttpClient>();
            
            await _vstsGitClient.CreateRepositoryAsync(new GitRepository() { Name = repositoryName }, _vstsConfig.Project);            
        }

        private void ConnectToVsts()
        {
            VssCredentials creds = new VssBasicCredential(string.Empty, _vstsConfig.Token);
            //creds.Storage = new VssClientCredentialStorage();

            var vstsCollectionUrl = $"https://{_vstsConfig.Instance}.visualstudio.com";

            _connection = new VssConnection(new Uri(vstsCollectionUrl), creds);
        }
    }
}
