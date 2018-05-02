using ci_x_core;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vsts_plugin
{
    public class Vsts : Command
    {
        public override string Verb => "vsts";

        public override Option[] Options => new Option[] {
            new Option("instance", "i", "instance", "Your vsts instance."),
            new Option("project", "p", "projec", "Your vsts project."),
            new Option("token", "t", "token", "Your vsts personal token, to see how to generate a personal token follow this link: https://docs.microsoft.com/en-us/vsts/accounts/use-personal-access-tokens-to-authenticate?view=vsts")
        };

        public override string Help => "Set the options to use your VSTS environment";

        public override void CreateEnvironment()
        {
            Terminal.Output(Help);
        }
    }

    //public class VstsService : EnvironmentPluginService
    //{
    //    private ConfigVstsVerb _vstsConfig;
    //    private VssConnection _connection;
    //    private GitHttpClient _vstsGitClient;

    //    public VstsService(): base("vsts")
    //    {
    //        _vstsConfig = (ConfigVstsVerb)ReadPluginConfig().Result;
    //        ConnectToVsts();
    //    }

    //    public override async Task CreateEnvironment(string projectName, string description = null)
    //    {
    //        await CreateRepository(projectName);
    //    }        

    //    public async Task CreateRepository(string repositoryName)
    //    {
    //        _vstsGitClient = _connection.GetClient<GitHttpClient>();

    //        await _vstsGitClient.CreateRepositoryAsync(new GitRepository() { Name = repositoryName }, _vstsConfig.Project);
    //    }

    //    //public override async Task<string[]> DefinePluginSection(ConfigVstsVerb opts)
    //    //{
    //    //    return new string[] { "[vsts]", $"instance={opts.Instance}", $"project={opts.Project}", $"token={opts.Token}" };
    //    //}

    //    public override Task<string[]> DefinePluginSection(ConfigVerb opts)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override Task<ConfigVerb> ReadDefinedConfigSections(IEnumerable<string[]> pluginConfigSplited)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //public override async Task<ConfigVstsVerb> ReadDefinedConfigSections(IEnumerable<string[]> pluginConfigSplited)
    //    //{
    //    //    var vstsConfig = new ConfigVstsVerb();

    //    //    foreach (var config in pluginConfigSplited)
    //    //    {
    //    //        if (config[0] == "instance")
    //    //        {
    //    //            vstsConfig.Instance = config[1];
    //    //            continue;
    //    //        }

    //    //        if (config[0] == "project")
    //    //        {
    //    //            vstsConfig.Project = config[1];
    //    //            continue;
    //    //        }

    //    //        vstsConfig.Token = config[1];
    //    //    }

    //    //    return vstsConfig;
    //    //}

    //    private async Task ConnectToVsts()
    //    {
    //        VssCredentials creds = new VssBasicCredential(string.Empty, _vstsConfig.Token);
    //        //creds.Storage = new VssClientCredentialStorage();

    //        var vstsCollectionUrl = $"https://{_vstsConfig.Instance}.visualstudio.com";

    //        _connection = new VssConnection(new Uri(vstsCollectionUrl), creds);
    //    }
    //}
}
