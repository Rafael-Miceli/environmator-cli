﻿using System;
using System.Collections.Generic;
using CommandLine;
using environmator_cli.Configuration;
using environmator_cli.Services;
using ci_x_core;


namespace environmator_cli
{
    public class Program
    {
        private static IEnumerable<EnvironmentPluginService> _plugins;

        static int Main(string[] args)
        {
            // Descobrir como pegar plugins dinamicamente
            _plugins = new List<EnvironmentPluginService>
            {
                new vsts_plugin.VstsService()
            };
            
        }

        private static Func<dynamic, int> GetPluginDefineConfigType(EnvironmentPluginService plugin)
        {
            throw new NotImplementedException();
        }

        private static int RunConfigAndReturnExitCode(object opts)
        {
            return 0;
        }

        private static int RunNewAndReturnExitCode(object opts)
        {
            return 0;
        }

        private static int RunNewProjectAndReturnExitCode(NewVerb.ProjectVerb opts)
        {
            //var result = CreateVstsRepo(opts.Name);

            //result = CreateJenkinsJob(opts.Name);

            return 0;
        }


        private static int RunCreateEnvironments(NewVerb.ProjectVerb opts)
        {
            //Dreaming:
            foreach (var creatorPlugin in _plugins)
            {
                try
                {
                    creatorPlugin.CreateEnvironment(opts.Name);
                }
                catch (Exception ex)
                {
                    //Log error
                }
                
            }

            return 0;
        }


        private static int CreateJenkinsJob(string name)
        {
            return 0;
        }

        //private static int CreateVstsRepo(string repositoryName)
        //{
        //    Console.WriteLine($"Creating {repositoryName} repository in vsts.");

        //    try
        //    {
        //        _vstsService.CreateRepository(repositoryName).Wait();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Sorry, but we had a problem trying to create {repositoryName} repository");
        //        Console.WriteLine(ex.Message);
        //        return 1;
        //    }

        //    Console.WriteLine($"{repositoryName} repository created!");

        //    return 0;
        //}
        
        //private static int RunConfigVstsAndReturnExitCode(ConfigVerb.ConfigVstsVerb opts)
        //{
        //    //_configRepository.SetVstsConfig(opts);
            
        //    return 0;
        //}

        //private static int RunSetEnvironmentsConfig(ConfigVerb opts)
        //{
        //    //Dreaming:
        //    foreach (var configPlugin in _plugins)
        //    {
        //        try
        //        {
        //            configPlugin.SetPluginConfig(opts);
        //        }
        //        catch (Exception ex)
        //        {
        //            //Log error
        //        }                
        //    }

        //    return 0;
        //}
    }
}
