﻿using System;
using System.Collections.Generic;
using ci_x_core;
using System.Linq;

namespace environmator_cli
{
    public class Program
    {
        private static IEnumerable<EnvironmentPluginService> _plugins;
        private static IEnumerable<Command> _commands;

        static int Main(string[] args)
        {
            InitializeCommands();


            if (args.Length <= 0)
            {
                Console.WriteLine("Command not found try one of these: ");
                ShowCommands();
                return -1;
            }

            var commands = args.Select(c => c.ToLower()).ToArray();

            if (commands[0] == "new-project")
            {
                //Apply new project creation
                return 0;
            }

            if (commands[0] == "config")
            {
                ShowConfigCommands();

                //Search if next verb exist in config
            }

            if (commands[0] == "-h" || commands[0] == "--help" || commands[0] == "help")
            {
                ShowCommands();
                return 0;
            }

            return 0;
        }

        private static void InitializeCommands()
        {
            _commands = new List<Command>
            {
                new Vsts()
            };
        }

        private static void ShowCommands()
        {
            Console.WriteLine("");
            Console.WriteLine("Commands:");
            Console.WriteLine("   new-project   Create a new project in all environments defined in default configuration or by the parameters.");
            Console.WriteLine("   config        Show the configuration to access you repositories, platforms, and C.I. builders.");
        }

        private static void ShowConfigCommands()
        {
            Console.WriteLine("");
            Console.WriteLine("configs:");
            foreach (var command in _commands)
            {
                Console.WriteLine($"   {command.Verb}   {command.Help}.");
            }            
        }
    }

    public abstract class Command
    {
        public abstract string Verb { get; }
        public abstract string[] Options { get; }
        public abstract string Help { get; }

        public abstract void Action();
    }

    public class Vsts : Command
    {
        public override string Verb => "vsts";

        public override string[] Options => new[] { "-i", "-t" };

        public override string Help => "Set your vsts configuration";

        public override void Action()
        {            

            Terminal.Output(Help);
        }
    }

    public static class Terminal
    {
        public static void Output(string value) => Console.WriteLine(value);
    }

}
