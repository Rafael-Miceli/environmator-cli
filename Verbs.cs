using System;
using CommandLine;

namespace environmator_cli
{
    [Verb("config", HelpText = "Show the configuration to access you repositories, platforms, and C.I. builders. 'config' command still under construction! Today we only support set your VSTS code repo environment.")]
    [ChildVerbs(typeof(ConfigVstsVerb))]
    public class ConfigVerb
    {
        public ConfigVerb()
        {
            Console.WriteLine("config Verb called");
        }

        [Verb("vsts", HelpText = "Set the options to use your VSTS environment.")]
        public class ConfigVstsVerb
        {
            [Option('i', "instance", HelpText = "Your vsts instance.")]
            public string Instance { get; set; }
        }
    }

    [Verb("new", HelpText = "Create a new project in all environments defined in default configuration or by the parameters.")]
    public class NewVerb
    {
        public NewVerb()
        {
            Console.WriteLine("new Verb called");
        }
    }

    
}