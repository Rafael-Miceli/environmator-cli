using ci_x_core;
using CommandLine;

namespace vsts_plugin
{
    [Verb("vsts", HelpText = "Set the options to use your VSTS environment.")]
    public class ConfigVstsVerb: ConfigVerb
    {
        [Option('i', "instance", HelpText = "Your vsts instance.", Required = true)]
        public string Instance { get; set; }
        [Option('p', "project", HelpText = "Your vsts project.", Required = true)]
        public string Project { get; set; }
        [Option('t', "token", HelpText = "Your vsts personal token, to see how to generate a personal token follow this link: https://docs.microsoft.com/en-us/vsts/accounts/use-personal-access-tokens-to-authenticate?view=vsts", Required = true)]
        public string Token { get; set; }
    }
}
