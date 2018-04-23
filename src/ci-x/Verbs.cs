using System;
using CommandLine;

namespace environmator_cli
{
    //[Verb("config", HelpText = "Show the configuration to access you repositories, platforms, and C.I. builders. 'config' command still under construction! Today we only support set your VSTS code repo environment.")]
    //[ChildVerbs(typeof(ConfigVstsVerb))]
    //public class ConfigVerb
    //{
    //    public ConfigVerb()
    //    {
    //        Console.WriteLine("config Verb called");
    //    }

    //    [Verb("vsts", HelpText = "Set the options to use your VSTS environment.")]
    //    public class ConfigVstsVerb
    //    {
    //        [Option('i', "instance", HelpText = "Your vsts instance.", Required = true)]
    //        public string Instance { get; set; }
    //        [Option('p', "project", HelpText = "Your vsts project.", Required = true)]
    //        public string Project { get; set; }
    //        [Option('t', "token", HelpText = "Your vsts personal token, to see how to generate a personal token follow this link: https://docs.microsoft.com/en-us/vsts/accounts/use-personal-access-tokens-to-authenticate?view=vsts", Required = true)]
    //        public string Token { get; set; }
    //    }
    //}

    [Verb("new", HelpText = "Create a new project in all environments defined in default configuration or by the parameters.")]
    [ChildVerbs(typeof(ProjectVerb))]
    public class NewVerb
    {
        public NewVerb()
        {
            Console.WriteLine("new Verb called");
        }

        [Verb("project", HelpText = "Create a new project in all environments defined in default configuration or by the parameters.")]
        public class ProjectVerb
        {
            [Option('n', "name", HelpText = "Your project name.", Required = true)]
            public string Name { get; set; }
        }
    }

    
}