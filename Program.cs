using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;

namespace environmator_cli
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseVerbs<ConfigVerb, NewVerb>(args)
                 .MapResult(
                 (ConfigVerb opts) => RunConfigAndReturnExitCode(opts),
                 (NewVerb opts) => RunNewAndReturnExitCode(opts),
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

        private static int RunConfigVstsAndReturnExitCode(ConfigVerb.ConfigVstsVerb opts)
        {
            Console.WriteLine("config vsts foi chamado");
            Console.WriteLine("Instance: " + opts.Instance);
            return 0;
        }        

    }
}
