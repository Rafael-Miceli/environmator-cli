using System;
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
                if (commands.Length <= 1 || IsAskingHelp(commands[1]))
                {
                    ShowConfigCommands();
                    return 0;
                }                    

                var selectedConfig = _commands.FirstOrDefault(c => c.Verb == commands[1]);

                if (selectedConfig == null)
                {
                    Console.WriteLine($"Config `{commands[1]}` not found... =/ Here are the configs you have:");
                    ShowConfigCommands();
                    return 0;
                }

                if (commands.Length <= 2 || IsAskingHelp(commands[2]))
                {
                    ShowConfigOptions(selectedConfig);
                    return 0;
                }

            }

            if (IsAskingHelp(commands[0]))
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

        private static bool IsAskingHelp(string command)
        {
            return command == "-h" || command == "--help" || command == "help";
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

        private static void ShowConfigOptions(Command command)
        {
            Console.WriteLine("");
            Console.WriteLine($"config {command.Verb} options:");
            foreach (var option in command.Options)
            {
                Console.WriteLine($"   -{option.TerminalShortName} --{option.TerminalLongName}   {option.Help}.");
            }
        }
    }

    public abstract class Command
    {
        public abstract string Verb { get; }
        public abstract Option[] Options { get; }
        public abstract string Help { get; }

        public abstract void Action();
    }

    public class Vsts : Command
    {
        public override string Verb => "vsts";

        public override Option[] Options => new Option[] {
            new Option("instance", "i", "instance", "Your vsts instance.")
        };

        public override string Help => "Set the options to use your VSTS environment";

        public override void Action()
        {
            Terminal.Output(Help);
        }
    }

    public class Option
    {
        public Option(
            string name, 
            string terminalShortName, 
            string terminalLongName = "", 
            string help = "", 
            bool isRequired = true)
        {
            Name = name;
            TerminalShortName = terminalShortName;
            TerminalLongName = terminalShortName;
            Help = help;
            IsRequired = isRequired;
        }

        public string Name { get; }
        public string TerminalShortName { get; }
        public string TerminalLongName { get; }        
        public string Help { get; }
        public bool IsRequired { get; }
    }

    public static class Terminal
    {
        public static void Output(string value) => Console.WriteLine(value);
    }

}
