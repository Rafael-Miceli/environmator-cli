using System;
using System.Collections.Generic;
using ci_x_core;
using System.Linq;
using System.Threading.Tasks;

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

                Dictionary<string, string> optionsAndValues = GetOptionsAndItsValues(commands);
                List<string> requiredOptionsNotFilled = RequiredOptionsNotFilled(optionsAndValues, selectedConfig);

                if (requiredOptionsNotFilled.Any())
                {
                    Console.Write("Required options: ");
                    foreach (var requiredOptionNotFilled in requiredOptionsNotFilled)
                        Console.Write($"{requiredOptionNotFilled},");
                    Console.WriteLine(" not filled");
                    
                    return -1;
                }

                selectedConfig.WriteConfig(optionsAndValues).Wait();
            }

            if (IsAskingHelp(commands[0]))
            {
                ShowCommands();
                return 0;
            }

            return 0;
        }

        private static List<string> RequiredOptionsNotFilled(Dictionary<string, string> optionsAndValues, Command selectedConfig)
        {
            var requiredOptionsShortNames = selectedConfig.Options.Where(o => o.IsRequired).Select(o => o.TerminalShortName);
            var requiredOptionsLongNames = selectedConfig.Options.Where(o => o.IsRequired).Select(o => o.TerminalLongName);


        }

        private static Dictionary<string, string> GetOptionsAndItsValues(string[] commands)
        {
            var optionsAndVaues = new Dictionary<string, string>();

            for (int i = 2; i <= commands.Length; i++)
            {
                var option = commands[i];
                i++;

                var value = "";
                if (i <= commands.Length)
                {
                    value = commands[i];
                }

                optionsAndVaues.Add(option, value);
            }

            return optionsAndVaues;
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
                Console.Write($"   -{option.TerminalShortName} ");
                if (!string.IsNullOrEmpty(option.TerminalLongName))
                    Console.Write($"--{option.TerminalLongName}");

                Console.WriteLine($"    {option.Help}");
            }
        }
    }

    public abstract class Command
    {
        public abstract string Verb { get; }
        public abstract Option[] Options { get; }
        public abstract string Help { get; }

        public abstract void CreateEnvironment();

        public async Task WriteConfig(Dictionary<string, string> optionsAndValues)
        {

        }
    }

    public class Vsts : Command
    {
        public override string Verb => "vsts";

        public override Option[] Options => new Option[] {
            new Option("instance", "i", "instance", "Your vsts instance."),
            new Option("project", "p", "project", "Your vsts project."),
            new Option("token", "t", "token", "Your vsts personal token, to see how to generate a personal token follow this link: https://docs.microsoft.com/en-us/vsts/accounts/use-personal-access-tokens-to-authenticate?view=vsts")
        };

        public override string Help => "Set the options to use your VSTS environment";

        public override void CreateEnvironment()
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
            TerminalLongName = terminalLongName;
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
