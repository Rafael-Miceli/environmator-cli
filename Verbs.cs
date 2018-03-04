using System;
using CommandLine;

namespace environmator_cli
{
    [Verb("config", HelpText = "Show the configuration to access you repositories, platforms, and C.I. builders.")]
    public class ConfigVerb
    {
        public ConfigVerb()
        {
            //Console.WriteLine("Show the configuration to access you repositories, platforms, and C.I. builders.");
        }

    }
}